namespace PikTools.Shared.RevitExtensions.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Abstractions;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Models;

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
        public void AddSharedParameter(SharedParameterInfo sharedParameterInfo, bool fullMatch, bool useTransaction = false)
        {
            void InternalAddSharedParameter(Document document, DefinitionFile definitionFile1)
            {
                var categorySet =
                    GetCategorySet(sharedParameterInfo.CreateData.CategoriesForBind.Select(c => Category.GetCategory(document, c)));

                var externalDefinition = GetSharedExternalDefinition(sharedParameterInfo, fullMatch, definitionFile1);

                Binding binding;
                if (sharedParameterInfo.CreateData.IsCreateForInstance)
                    binding = document.Application.Create.NewInstanceBinding(categorySet);
                else
                    binding = document.Application.Create.NewTypeBinding(categorySet);

                var map = document.ParameterBindings;
                map.Insert(externalDefinition, binding, sharedParameterInfo.CreateData.ParameterGroup);

                if (sharedParameterInfo.CreateData.AllowVaryBetweenGroups)
                    SetAllowVaryBetweenGroups(sharedParameterInfo.Definition.ParameterName);
            }

            if (sharedParameterInfo.Definition == null)
                throw new ArgumentNullException(nameof(sharedParameterInfo.Definition), "Не задано описание общего параметра");
            if (sharedParameterInfo.CreateData == null)
                throw new ArgumentNullException(nameof(sharedParameterInfo.CreateData), "Не заданы данные для создания общего параметра");
            if (ParameterExists(sharedParameterInfo.Definition, fullMatch))
                return;

            if (sharedParameterInfo.CreateData.CategoriesForBind == null ||
                !sharedParameterInfo.CreateData.CategoriesForBind.Any())
                throw new ArgumentException("Не указаны категории для привязки параметра");

            var doc = _uiApplication.ActiveUIDocument.Document;
            var definitionFile = GetDefinitionFile();

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
        }

        /// <inheritdoc />
        public bool ParameterExistsInSpf(SharedParameterInfo sharedParameterInfo, bool fullMatch)
        {
            try
            {
                var definitionFile = GetDefinitionFile();
                return GetSharedExternalDefinition(sharedParameterInfo, fullMatch, definitionFile) != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка существования параметра указанного имени в документе
        /// </summary>
        /// <param name="sharedParameterDefinition">Данные об общем параметре</param>
        /// <param name="fullMatch">True - параметр должен совпасть со всеми заполненными значениями
        /// sharedParameterInfo, доступными для проверки через SharedParameterElement (Имя, Guid, DataType).
        /// False - параметр ищется только по имени</param>
        private bool ParameterExists(SharedParameterDefinition sharedParameterDefinition, bool fullMatch)
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

            throw new KeyNotFoundException($"В ФОП не найден параметр \"{sharedParameterInfo.Definition.ParameterName}\"");
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

        private DefinitionFile GetDefinitionFile()
        {
            var doc = _uiApplication.ActiveUIDocument.Document;
            var sharedParameterFilename = doc.Application.SharedParametersFilename;

            if (string.IsNullOrEmpty(sharedParameterFilename) || !File.Exists(sharedParameterFilename))
            {
                throw new FileNotFoundException("Не найден файл общих параметров");
            }

            return doc.Application.OpenSharedParameterFile();
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
