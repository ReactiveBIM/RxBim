namespace PikTools.Shared.RevitExtensions.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Abstractions;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using CSharpFunctionalExtensions;
    using Models;
    using Result = CSharpFunctionalExtensions.Result;

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
        public Result AddSharedParameter(
            DefinitionFile definitionFile,
            SharedParameterInfo sharedParameterInfo,
            bool fullMatch,
            bool useTransaction = false)
        {
            if (sharedParameterInfo.Definition == null)
                return Result.Failure($"Не задано описание общего параметра \"{sharedParameterInfo.Definition.ParameterName}\"");
            if (sharedParameterInfo.CreateData == null)
                return Result.Failure($"Не заданы данные для создания общего параметра \"{sharedParameterInfo.Definition.ParameterName}\"");
            if (ParameterExistsInDocument(sharedParameterInfo.Definition, fullMatch))
                return Result.Failure($"Параметр \"{sharedParameterInfo.Definition.ParameterName}\" уже добавлен в модель");
            if (sharedParameterInfo.CreateData.CategoriesForBind == null
                || !sharedParameterInfo.CreateData.CategoriesForBind.Any())
                return Result.Failure($"Не указаны категории для привязки параметра \"{sharedParameterInfo.Definition.ParameterName}\"");

            var doc = _uiApplication.ActiveUIDocument.Document;

            Result result;
            if (useTransaction)
            {
                using var tr = new Transaction(doc, "Добавление параметров");
                tr.Start();
                result = AddSharedParameter(
                    doc, definitionFile, sharedParameterInfo, fullMatch);
                tr.Commit();
            }
            else
            {
                result = AddSharedParameter(
                    doc, definitionFile, sharedParameterInfo, fullMatch);
            }

            return result;
        }

        /// <inheritdoc />
        public Result AddOrUpdateParameter(
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

            if (!externalDefinitionInFile.Any())
                return Result.Failure($"Параметр \"{sharedParameterInfo.Definition.ParameterName}\" не найден ни в одном из ФОП");

            if (externalDefinitionInFile.Length > 1 && !fullMatch)
            {
                return Result.Failure(
                    "Параметр с одинаковым именем " +
                    $"\"{sharedParameterInfo.Definition.ParameterName}\" " +
                    "обнаружен в нескольких ФОП!");
            }

            if (existsInDocument)
            {
                return UpdateParameterBindings(
                    externalDefinitionInFile[0].ExternalDefinition,
                    sharedParameterInfo.CreateData);
            }

            return AddSharedParameter(
                externalDefinitionInFile[0].DefinitionFile,
                sharedParameterInfo,
                fullMatch,
                false);
        }

        /// <inheritdoc />
        public Result ParameterExistsInDefinitionFile(
            DefinitionFile definitionFile,
            SharedParameterInfo sharedParameterInfo,
            bool fullMatch)
        {
            try
            {
                return Result.SuccessIf(
                    GetSharedExternalDefinition(sharedParameterInfo, fullMatch, definitionFile) != null,
                    "Параметр не найден в ФОП");
            }
            catch (Exception exception)
            {
                return Result.Failure(exception.ToString());
            }
        }

        /// <inheritdoc />
        public Result<DefinitionFile> GetDefinitionFile(Document document = null)
        {
            var doc = document ?? _uiApplication.ActiveUIDocument.Document;
            var sharedParameterFilename = doc.Application.SharedParametersFilename;

            if (string.IsNullOrEmpty(sharedParameterFilename)
                || !File.Exists(sharedParameterFilename))
                Result.Failure("Не найден файл общих параметров");

            return doc.Application.OpenSharedParameterFile();
        }

        /// <inheritdoc/>
        public DefinitionFile[] TryGetDefinitionFiles(SharedParameterFileSource fileSource)
        {
            var document = _uiApplication.ActiveUIDocument.Document;

            var oldDefinitionFilePath = string.Empty;

            bool wasInitiallySet;
            try
            {
                oldDefinitionFilePath = document.Application.SharedParametersFilename;

                wasInitiallySet = true;
            }
            catch
            {
                wasInitiallySet = false;
            }

            var definitionFiles = new List<DefinitionFile>();

            foreach (var filePath in fileSource.FilePaths)
            {
                try
                {
                    var fileInfo = new FileInfo(filePath);

                    document.Application.SharedParametersFilename = fileInfo.FullName;
                    definitionFiles.Add(
                        document.Application.OpenSharedParameterFile());
                }
                catch
                {
                    // ignore
                }
            }

            if (wasInitiallySet)
                document.Application.SharedParametersFilename = oldDefinitionFilePath;

            return definitionFiles.ToArray();
        }

        private Result AddSharedParameter(
            Document document,
            DefinitionFile definitionFile,
            SharedParameterInfo sharedParameterInfo,
            bool fullMatch)
        {
            var categorySet =
                GetCategorySet(sharedParameterInfo.CreateData.CategoriesForBind.Select(c => Category.GetCategory(document, c)));

            var externalDefinition = GetSharedExternalDefinition(sharedParameterInfo, fullMatch, definitionFile);

            if (externalDefinition == null)
                return Result.Failure($"Параметр \"{sharedParameterInfo.Definition.ParameterName}\" не найден в файле общих параметров \"{definitionFile.Filename}\"");

            Binding binding;
            if (sharedParameterInfo.CreateData.IsCreateForInstance)
                binding = document.Application.Create.NewInstanceBinding(categorySet);
            else
                binding = document.Application.Create.NewTypeBinding(categorySet);

            var map = document.ParameterBindings;

            var inserted = map.Insert(externalDefinition, binding, sharedParameterInfo.CreateData.ParameterGroup);

            if (sharedParameterInfo.CreateData.AllowVaryBetweenGroups)
                SetAllowVaryBetweenGroups(sharedParameterInfo.Definition.ParameterName);

            return inserted
                ? Result.Success()
                : Result.Failure($"Не удалось добавить параметр \"{sharedParameterInfo.Definition.ParameterName}\"");
        }

        private Result UpdateParameterBindings(
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
                return Result.Failure($"В проекте не найдены категории для обновляемого параметра \"{definition.Name}\"");

            foreach (var category in categories.Where(c => !set.Contains(c)))
                set.Insert(category);

            ElementBinding updatedBinding;

            var isInstance = createData.IsCreateForInstance;

            if (binding is InstanceBinding || isInstance)
                updatedBinding = createService.NewInstanceBinding(set);
            else
                updatedBinding = createService.NewTypeBinding(set);

            map.Remove(definition);

            return Result.SuccessIf(
                map.Insert(definition, updatedBinding, createData.ParameterGroup),
                $"Не удалось обновить параметр \"{definition.Name}\"");
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
                if (category != null
                    && category.AllowsBoundParameters)
                    categorySet.Insert(category);
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
            foreach (var defGroup in definitionFile.Groups)
            {
                foreach (var def in defGroup.Definitions)
                {
                    if (!(def is ExternalDefinition externalDefinition))
                        continue;

                    if (!fullMatch
                        && sharedParameterInfo.Definition.ParameterName == externalDefinition.Name)
                        return externalDefinition;

                    if (fullMatch)
                    {
                        if (!IsFullMatch(sharedParameterInfo.Definition, externalDefinition))
                            continue;

                        return externalDefinition;
                    }
                }
            }

            return null;
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
            if (sharedParameterDefinition.Guid.HasValue
                && externalDefinition.GUID != sharedParameterDefinition.Guid.Value)
                return false;
            if (sharedParameterDefinition.DataType.HasValue
                && externalDefinition.ParameterType != sharedParameterDefinition.DataType.Value)
                return false;
            if (!string.IsNullOrEmpty(sharedParameterDefinition.OwnerGroupName)
                && !externalDefinition.OwnerGroup.Name.Equals(sharedParameterDefinition.OwnerGroupName, StringComparison.OrdinalIgnoreCase))
                return false;
            if (sharedParameterDefinition.Visible.HasValue
                && externalDefinition.Visible != sharedParameterDefinition.Visible.Value)
                return false;
            if (sharedParameterDefinition.UserModifiable.HasValue
                && externalDefinition.UserModifiable != sharedParameterDefinition.UserModifiable.Value)
                return false;
            if (!string.IsNullOrEmpty(sharedParameterDefinition.Description)
                && !externalDefinition.Description.Equals(sharedParameterDefinition.Description, StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        private bool IsFullMatch(
            SharedParameterDefinition sharedParameterDefinition, SharedParameterElement sharedParameterElement)
        {
            var internalDefinition = sharedParameterElement.GetDefinition();
            if (internalDefinition.Name != sharedParameterDefinition.ParameterName)
                return false;
            if (sharedParameterDefinition.Guid.HasValue
                && sharedParameterElement.GuidValue != sharedParameterDefinition.Guid.Value)
                return false;
            if (sharedParameterDefinition.DataType.HasValue
                && internalDefinition.ParameterType != sharedParameterDefinition.DataType.Value)
                return false;

            return true;
        }
    }
}
