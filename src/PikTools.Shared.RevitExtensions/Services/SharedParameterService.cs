namespace PikTools.Shared.RevitExtensions.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using PikTools.Shared.RevitExtensions.Abstractions;
    using PikTools.Shared.RevitExtensions.Models;

    /// <summary>
    /// Сервис по работе с общими параметрами
    /// </summary>
    public class SharedParameterService : ISharedParameterService
    {
        private readonly UIApplication _uiApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedParameterService"/> class.
        /// </summary>
        /// <param name="uiApplication">Current <see cref="UIApplication"/></param>
        public SharedParameterService(UIApplication uiApplication)
        {
            _uiApplication = uiApplication;
        }

        /// <inheritdoc />
        public bool AddSharedParameter(
            DefinitionFile definitionFile,
            SharedParameterInfo sharedParameterInfo,
            bool fullMatch,
            bool useTransaction = false)
        {
            bool inserted = false;

            void InternalAddSharedParameter(Document document, DefinitionFile definitionFile1)
            {
                var categorySet =
                    GetCategorySet(sharedParameterInfo.CreateData.CategoriesForBind.Select(c => Category.GetCategory(document, c)));

                var externalDefinition = GetSharedExternalDefinition(sharedParameterInfo, fullMatch, definitionFile1);

                if (externalDefinition == null)
                {
                    return;
                }

                Binding binding;
                if (sharedParameterInfo.CreateData.IsCreateForInstance)
                    binding = document.Application.Create.NewInstanceBinding(categorySet);
                else
                    binding = document.Application.Create.NewTypeBinding(categorySet);

                var map = document.ParameterBindings;

                inserted = map.Insert(externalDefinition, binding, sharedParameterInfo.CreateData.ParameterGroup);

                if (sharedParameterInfo.CreateData.AllowVaryBetweenGroups)
                    SetAllowVaryBetweenGroups(sharedParameterInfo.Definition.ParameterName);
            }

            if (sharedParameterInfo.Definition == null)
                throw new ArgumentNullException(nameof(sharedParameterInfo.Definition), "Не задано описание общего параметра");
            if (sharedParameterInfo.CreateData == null)
                throw new ArgumentNullException(nameof(sharedParameterInfo.CreateData), "Не заданы данные для создания общего параметра");
            if (ParameterExistsInDocument(sharedParameterInfo.Definition, fullMatch))
                return false;

            if (sharedParameterInfo.CreateData.CategoriesForBind == null ||
                !sharedParameterInfo.CreateData.CategoriesForBind.Any())
                throw new ArgumentException("Не указаны категории для привязки параметра");

            var doc = _uiApplication.ActiveUIDocument.Document;

            if (useTransaction)
            {
                using (var tr = new Transaction(doc, "Добавление параметров"))
                {
                    tr.Start();

                    InternalAddSharedParameter(doc, definitionFile);

                    tr.Commit();
                }
            }
            else
            {
                InternalAddSharedParameter(doc, definitionFile);
            }

            return inserted;
        }

        /// <inheritdoc />
        public bool ParameterExistsInDefinitionFile(
            DefinitionFile definitionFile, SharedParameterInfo sharedParameterInfo, bool fullMatch)
        {
            try
            {
                return GetSharedExternalDefinition(sharedParameterInfo, fullMatch, definitionFile) != null;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc />
        public DefinitionFile GetDefinitionFile(Document document = null)
        {
            var doc = document ?? _uiApplication.ActiveUIDocument.Document;
            var sharedParameterFilename = doc.Application.SharedParametersFilename;

            if (string.IsNullOrEmpty(sharedParameterFilename) || !File.Exists(sharedParameterFilename))
            {
                throw new FileNotFoundException("Не найден файл общих параметров");
            }

            return doc.Application.OpenSharedParameterFile();
        }

        /// <summary>
        /// Считывает файлы общих параметров используя информацию
        /// из <see cref="SharedParameterFileSource"/>
        /// </summary>
        /// <param name="fileSource"><see cref="SharedParameterFileSource"/></param>
        public DefinitionFile[] TryGetDefinitionFiles(
            SharedParameterFileSource fileSource)
        {
            var document = _uiApplication
                .ActiveUIDocument
                .Document;

            var oldDefinitionFilePath = string.Empty;

            bool wasInitiallySet;

            try
            {
                oldDefinitionFilePath =
                    document
                        .Application
                        .SharedParametersFilename;

                wasInitiallySet = true;
            }
            catch (Exception ex)
            {
                wasInitiallySet = false;
            }

            var definitionFiles = new List<DefinitionFile>();

            foreach (var filePath in fileSource.FilePaths)
            {
                try
                {
                    var fileInfo = new FileInfo(filePath);

                    document
                        .Application
                        .SharedParametersFilename =
                        fileInfo.FullName;

                    definitionFiles.Add(
                        document
                            .Application
                            .OpenSharedParameterFile());
                }
                catch (Exception ex)
                {
                }
            }

            if (wasInitiallySet)
            {
                document
                    .Application
                    .SharedParametersFilename =
                    oldDefinitionFilePath;
            }

            return definitionFiles.ToArray();
        }

        /// <inheritdoc />
        public bool AddOrUpdateParameter(
            DefinitionFile[] definitionFiles,
            SharedParameterInfo sharedParameterInfo,
            bool fullMatch)
        {
            bool existsInDocument = ParameterExistsInDocument(
                sharedParameterInfo.Definition,
                fullMatch);

            var externalDefinitionInFile = definitionFiles
                .Select(df => new
                {
                    ExternalDefinition = GetSharedExternalDefinition(
                        sharedParameterInfo,
                        fullMatch,
                        df),
                    DefinitionFile = df
                })
                .Where(a => a.ExternalDefinition != null)
                .ToArray();

            if (externalDefinitionInFile.Length < 1)
            {
                return false;
            }

            if (externalDefinitionInFile.Length > 1 && !fullMatch)
            {
                throw new ApplicationException(
                    $"Параметр с одинаковым именем " +
                    $"{sharedParameterInfo.Definition.ParameterName} " +
                    $"обнаружен в нескольких ФОП!");
            }

            if (existsInDocument)
            {
                return UpdateParameterBindings(
                    externalDefinitionInFile[0].ExternalDefinition,
                    sharedParameterInfo.CreateData);
            }
            else
            {
                return AddSharedParameter(
                    externalDefinitionInFile[0].DefinitionFile,
                    sharedParameterInfo,
                    fullMatch,
                    useTransaction: false);
            }
        }

        private bool UpdateParameterBindings(
            ExternalDefinition definition,
            SharedParameterCreateData createData)
        {
            var document = _uiApplication.ActiveUIDocument.Document;

            var createService = document.Application.Create;

            var map = document.ParameterBindings;

            var binding = (ElementBinding)map.get_Item(definition);

            var set = binding?.Categories ?? new CategorySet();

            var categories = createData
                .CategoriesForBind
                .Select(bic => Category.GetCategory(document, bic))
                .ToArray();

            if (categories.All(c => set.Contains(c)))
            {
                return false;
            }

            categories
                .Where(c => !set.Contains(c))
                .Select(c => set.Insert(c))
                .ToArray();

            ElementBinding updatedBinding;

            var isInstance = createData.IsCreateForInstance;

            if (binding is InstanceBinding || isInstance)
            {
                updatedBinding = createService.NewInstanceBinding(set);
            }
            else if (binding is TypeBinding || !isInstance)
            {
                updatedBinding = createService.NewTypeBinding(set);
            }
            else
            {
                throw new Exception();
            }

            map.Remove(definition);

            var updated = map.Insert(
                definition,
                updatedBinding,
                createData.ParameterGroup);

            return updated;
        }

        /// <summary>
        /// Проверка существования параметра указанного имени в документе
        /// </summary>
        /// <param name="sharedParameterDefinition">Данные об общем параметре</param>
        /// <param name="fullMatch">True - параметр должен совпасть со всеми заполненными значениями
        /// sharedParameterInfo, доступными для проверки через SharedParameterElement (Имя, Guid, DataType).
        /// False - параметр ищется только по имени</param>
        private bool ParameterExistsInDocument(SharedParameterDefinition sharedParameterDefinition, bool fullMatch)
        {
            var doc = _uiApplication.ActiveUIDocument.Document;
            foreach (var sharedParameterElement in new FilteredElementCollector(doc)
                .OfClass(typeof(SharedParameterElement))
                .Cast<SharedParameterElement>())
            {
                if (!fullMatch && sharedParameterElement.Name == sharedParameterDefinition.ParameterName)
                    return true;

                if (fullMatch && IsFullMatch(sharedParameterDefinition, sharedParameterElement))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Конвертирование списка категорий в экземпляр <see cref="CategorySet"/>
        /// </summary>
        /// <param name="categories">Список категорий</param>
        private CategorySet GetCategorySet(IEnumerable<Category> categories)
        {
            var categorySet = new CategorySet();
            foreach (var category in categories)
            {
                if (category != null && category.AllowsBoundParameters)
                {
                    categorySet.Insert(category);
                }
            }

            return categorySet;
        }

        /// <summary>
        /// Возвращает определение общего параметра <see cref="ExternalDefinition"/> из текущего ФОП по имени
        /// </summary>
        /// <param name="sharedParameterInfo">Данные об общем параметре</param>
        /// <param name="fullMatch">True - параметр ФОП должен совпасть со всеми заполненными
        /// значениями sharedParameterInfo. False - параметр ищется только по имени</param>
        /// <param name="definitionFile">ФОП</param>
        private ExternalDefinition GetSharedExternalDefinition(
            SharedParameterInfo sharedParameterInfo,
            bool fullMatch,
            DefinitionFile definitionFile)
        {
            ExternalDefinition definition = null;

            foreach (var defGroup in definitionFile.Groups)
            {
                foreach (var def in defGroup.Definitions)
                {
                    if (!(def is ExternalDefinition externalDefinition))
                        continue;

                    if (!fullMatch && sharedParameterInfo.Definition.ParameterName == externalDefinition.Name)
                        return externalDefinition;

                    if (fullMatch)
                    {
                        if (!IsFullMatch(sharedParameterInfo.Definition, externalDefinition))
                            continue;

                        return externalDefinition;
                    }
                }
            }

            return definition;
        }

        /// <summary>
        /// Установить для параметров свойство "Значения могут меняться по экземплярам групп". Метод должен использоваться
        /// внутри запущенной транзакции
        /// </summary>
        /// <param name="parameterName">Имя параметра</param>
        private void SetAllowVaryBetweenGroups(string parameterName)
        {
            var doc = _uiApplication.ActiveUIDocument.Document;
            var map = doc.ParameterBindings;
            var it = map.ForwardIterator();
            it.Reset();
            while (it.MoveNext())
            {
                var definition = it.Key;
                if (parameterName != definition.Name)
                    continue;

                if (definition is InternalDefinition internalDef)
                {
                    try
                    {
                        internalDef.SetAllowVaryBetweenGroups(doc, true);
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }
        }

        private bool IsFullMatch(
            SharedParameterDefinition sharedParameterDefinition, ExternalDefinition externalDefinition)
        {
            if (sharedParameterDefinition.ParameterName != externalDefinition.Name)
                return false;
            if (sharedParameterDefinition.Guid.HasValue &&
                externalDefinition.GUID != sharedParameterDefinition.Guid.Value)
                return false;
            if (sharedParameterDefinition.DataType.HasValue &&
                externalDefinition.ParameterType != sharedParameterDefinition.DataType.Value)
                return false;
            if (!string.IsNullOrEmpty(sharedParameterDefinition.OwnerGroupName) &&
                !externalDefinition.OwnerGroup.Name.Equals(sharedParameterDefinition.OwnerGroupName, StringComparison.OrdinalIgnoreCase))
                return false;
            if (sharedParameterDefinition.Visible.HasValue &&
                externalDefinition.Visible != sharedParameterDefinition.Visible.Value)
                return false;
            if (sharedParameterDefinition.UserModifiable.HasValue &&
                externalDefinition.UserModifiable != sharedParameterDefinition.UserModifiable.Value)
                return false;
            if (!string.IsNullOrEmpty(sharedParameterDefinition.Description) &&
                !externalDefinition.Description.Equals(sharedParameterDefinition.Description, StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        private bool IsFullMatch(
            SharedParameterDefinition sharedParameterDefinition, SharedParameterElement sharedParameterElement)
        {
            var internalDefinition = sharedParameterElement.GetDefinition();
            if (internalDefinition.Name != sharedParameterDefinition.ParameterName)
                return false;
            if (sharedParameterDefinition.Guid.HasValue &&
                sharedParameterElement.GuidValue != sharedParameterDefinition.Guid.Value)
                return false;
            if (sharedParameterDefinition.DataType.HasValue &&
                internalDefinition.ParameterType != sharedParameterDefinition.DataType.Value)
                return false;

            return true;
        }
    }
}
