namespace RxBim.Shared.RevitExtensions.Services
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
        private readonly ITransactionService _transactions;

        /// <inheritdoc />
        public SharedParameterService(UIApplication uiApplication, ITransactionService transactions)
        {
            _uiApplication = uiApplication;
            _transactions = transactions;
        }

        /// <inheritdoc />
        public Result AddSharedParameter(
            DefinitionFile definitionFile,
            SharedParameterInfo sharedParameterInfo,
            bool fullMatch,
            bool useTransaction = false)
        {
            if (sharedParameterInfo == null)
                return Result.Failure("Данные об общем параметре не заданы");
            if (sharedParameterInfo.Definition == null)
                return Result.Failure("Не заданы данные для определения общего параметра");
            if (sharedParameterInfo.CreateData == null)
                return Result.Failure($"Не заданы данные для создания общего параметра '{sharedParameterInfo.Definition.ParameterName}'");
            if (ParameterExistsInDocument(sharedParameterInfo.Definition, fullMatch))
                return Result.Failure($"Параметр '{sharedParameterInfo.Definition.ParameterName}' уже добавлен в модель");
            if (sharedParameterInfo.CreateData.CategoriesForBind?.Any() != true)
                return Result.Failure($"Не указаны категории для привязки параметра '{sharedParameterInfo.Definition.ParameterName}'");

            var doc = _uiApplication.ActiveUIDocument.Document;

            if (useTransaction)
            {
                return _transactions.RunInTransaction(
                    () => AddSharedParameter(doc, definitionFile, sharedParameterInfo, fullMatch),
                    "Добавление параметров");
            }

            return AddSharedParameter(doc, definitionFile, sharedParameterInfo, fullMatch);
        }

        /// <inheritdoc />
        public Result AddOrUpdateParameter(
            DefinitionFile[] definitionFiles,
            SharedParameterInfo sharedParameterInfo,
            bool fullMatch)
        {
            var existsInDocument = ParameterExistsInDocument(
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
                .Where(x => x.ExternalDefinition != null)
                .ToList();

            if (!externalDefinitionInFile.Any())
                return Result.Failure($"Параметр '{sharedParameterInfo.Definition.ParameterName}' не найден ни в одном из ФОП");

            if (externalDefinitionInFile.Count > 1 && !fullMatch)
                return Result.Failure($"Параметр '{sharedParameterInfo.Definition.ParameterName}' обнаружен в нескольких ФОП");

            if (existsInDocument)
            {
                return UpdateParameterBindings(
                        externalDefinitionInFile[0].ExternalDefinition,
                        sharedParameterInfo.CreateData)
                    .TapIf(
                        sharedParameterInfo.CreateData.AllowVaryBetweenGroups,
                        () => SetAllowVaryBetweenGroups(sharedParameterInfo.Definition.ParameterName));
            }

            return AddSharedParameter(
                externalDefinitionInFile[0].DefinitionFile,
                sharedParameterInfo,
                fullMatch,
                false);
        }

        /// <inheritdoc />
        public Result<DefinitionFile> GetDefinitionFile(Document document = null)
        {
            var doc = document ?? _uiApplication.ActiveUIDocument.Document;
            var sharedParameterFilename = doc.Application.SharedParametersFilename;

            if (string.IsNullOrWhiteSpace(sharedParameterFilename))
                return Result.Failure<DefinitionFile>("Файл общих параметров не задан");

            return File.Exists(sharedParameterFilename)
                ? doc.Application.OpenSharedParameterFile()
                : Result.Failure<DefinitionFile>($"Не найден файл общих параметров '{sharedParameterFilename}'");
        }

        /// <inheritdoc/>
        public DefinitionFile[] TryGetDefinitionFiles(SharedParameterFileSource fileSource)
        {
            var document = _uiApplication.ActiveUIDocument.Document;
            var oldDefinitionFilePath = string.Empty;

            bool initialized;
            try
            {
                oldDefinitionFilePath = document.Application.SharedParametersFilename;
                initialized = true;
            }
            catch
            {
                initialized = false;
            }

            var definitionFiles = new List<DefinitionFile>();

            foreach (var filePath in fileSource.FilePaths)
            {
                try
                {
                    document.Application.SharedParametersFilename = new FileInfo(filePath).FullName;
                    definitionFiles.Add(document.Application.OpenSharedParameterFile());
                }
                catch
                {
                    // ignore
                }
            }

            if (initialized)
                document.Application.SharedParametersFilename = oldDefinitionFilePath;

            return definitionFiles.ToArray();
        }

        /// <inheritdoc/>
        public bool ParameterExistsInDocument(SharedParameterDefinition definition, bool fullMatch)
        {
            var doc = _uiApplication.ActiveUIDocument.Document;
            foreach (var sharedParameterElement in new FilteredElementCollector(doc)
                .OfClass(typeof(SharedParameterElement))
                .Cast<SharedParameterElement>())
            {
                if (!fullMatch && string.Equals(
                    sharedParameterElement.Name,
                    definition.ParameterName,
                    StringComparison.InvariantCultureIgnoreCase))
                    return true;

                if (fullMatch && IsFullMatch(definition, sharedParameterElement))
                    return true;
            }

            return false;
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
                return Result.Failure("Ошибка проверки параметра ФОП. " + exception.Message);
            }
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
                return Result.Failure($"Параметр '{sharedParameterInfo.Definition.ParameterName}' не найден в ФОП '{definitionFile.Filename}'");

            var binding = sharedParameterInfo.CreateData.IsCreateForInstance
                ? (Binding)document.Application.Create.NewInstanceBinding(categorySet)
                : document.Application.Create.NewTypeBinding(categorySet);

            var result = Result.SuccessIf(
                    document.ParameterBindings.Insert(externalDefinition, binding, sharedParameterInfo.CreateData.ParameterGroup),
                    $"Не удалось добавить параметр '{sharedParameterInfo.Definition.ParameterName}'")
                .TapIf(
                    sharedParameterInfo.CreateData.AllowVaryBetweenGroups,
                    () => SetAllowVaryBetweenGroups(sharedParameterInfo.Definition.ParameterName));

            return result;
        }

        private Result UpdateParameterBindings(
            Definition definition,
            SharedParameterCreateData createData)
        {
            var document = _uiApplication.ActiveUIDocument.Document;
            var creationService = document.Application.Create;
            var parameterBindings = document.ParameterBindings;

            var binding = (ElementBinding)parameterBindings.get_Item(definition);
            var existCategories = binding?.Categories ?? new CategorySet();

            var newCategories = createData.CategoriesForBind
                .Select(bic => Category.GetCategory(document, bic))
                .ToList();

            if (newCategories.All(existCategories.Contains))
                return Result.Success();

            foreach (var category in newCategories.Where(c => !existCategories.Contains(c)))
                existCategories.Insert(category);

            var newBinding = binding is InstanceBinding || createData.IsCreateForInstance
                ? (ElementBinding)creationService.NewInstanceBinding(existCategories)
                : creationService.NewTypeBinding(existCategories);

            parameterBindings.Remove(definition);

            return Result.SuccessIf(
                parameterBindings.Insert(definition, newBinding, createData.ParameterGroup),
                $"Не удалось обновить параметр '{definition.Name}'");
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
                if (category is { AllowsBoundParameters: true })
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

                if (!(definition is InternalDefinition internalDef))
                    continue;

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

        private bool IsFullMatch(SharedParameterDefinition sharedParameterDefinition, ExternalDefinition externalDefinition)
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
