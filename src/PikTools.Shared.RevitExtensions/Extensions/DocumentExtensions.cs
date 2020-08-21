namespace PikTools.Shared.RevitExtensions.Extensions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Autodesk.Revit.DB;

    /// <summary>
    /// Расширения для документа Revit
    /// </summary>
    public static class DocumentExtensions
    {
        /// <summary>
        /// Получить открытый связанный документ Revit
        /// </summary>
        /// <param name="rootDoc">Основной документ Revit</param>
        /// <param name="linkedDocTitle">Название связанного документа</param>
        /// <returns>Открытый связанный документ Revit</returns>
        public static Document GetOpenedLinkedDocument(this Document rootDoc, string linkedDocTitle)
        {
            var linkedDoc = rootDoc.Application.Documents
                .Cast<Document>()
                .FirstOrDefault(doc => doc.Title.Equals(linkedDocTitle));
            if (linkedDoc == null)
                return null;
            if (!linkedDoc.IsLinked)
                return linkedDoc;

            var linkType = rootDoc.GetLinkType(linkedDocTitle);
            if (linkType == null)
                return null;

            var fileName = string.Empty;
            ModelPath modelPath = null;

            if (linkedDoc.IsWorkshared)
                modelPath = linkedDoc.GetWorksharingCentralModelPath();

            if (modelPath == null
                || modelPath.Empty)
                fileName = linkedDoc.PathName;

            linkType.Unload(null);

            Document lDoc = null;
            try
            {
                lDoc = modelPath != null && !modelPath.Empty
                    ? rootDoc.Application.OpenDocumentFile(modelPath, new OpenOptions())
                    : rootDoc.Application.OpenDocumentFile(fileName);
            }
            catch
            {
                // Подавление исключений
            }

            return lDoc;
        }

        /// <summary>
        /// Закрытие связаного документа
        /// </summary>
        /// <param name="rootDoc">Основной документ Revit</param>
        /// <param name="linkedDocument">Ранее открытый связанный документ</param>
        public static void CloseLinkedDocument(this Document rootDoc, Document linkedDocument)
        {
            var linkType = rootDoc.GetLinkType(linkedDocument.Title);
            if (linkType == null)
                return;

            var transactOptions = new TransactWithCentralOptions();
            var syncOptions = new SynchronizeWithCentralOptions();

            var relOpt = new RelinquishOptions(true);
            syncOptions.SetRelinquishOptions(relOpt);

            try
            {
                /*linkedDocument.SynchronizeWithCentral(transactOptions, syncOptions);*/
                linkedDocument.Close(true);
            }
            catch
            {
                // Подавление исключений
            }

            // Обновляем связанный документ, чтобы он остался доступен из основного файла
            var method = linkType.GetType().GetMethod("Reload");
            method?.Invoke(linkType, null);
        }

        /// <summary>
        /// Получить тип связанного документа
        /// </summary>
        /// <param name="rootDoc">Основной документ</param>
        /// <param name="linkedDocTitle">Название связаного документа</param>
        /// <returns>Тип связанного документа</returns>
        public static RevitLinkType GetLinkType(this Document rootDoc, string linkedDocTitle)
        {
            return new FilteredElementCollector(rootDoc)
                .OfCategory(BuiltInCategory.OST_RvtLinks)
                .OfClass(typeof(RevitLinkType))
                .FirstOrDefault(lt => linkedDocTitle.Equals(Path.GetFileNameWithoutExtension(lt.Name))) as RevitLinkType;
        }

        /// <summary>
        /// Получение списка идентификаторов категорий, имеющихся в проекте
        /// </summary>
        /// <param name="doc">Документ</param>
        /// <param name="excludeCategories">Список идентификаторов категорий, которые требуется пропустить</param>
        /// <param name="includeSubCategories">Включая подкатегории</param>
        /// <returns></returns>
        public static IEnumerable<int> GetCategoriesIds(
            this Document doc,
            IEnumerable<int> excludeCategories,
            bool includeSubCategories = true)
        {
            return doc.GetCategoriesIdsIEnumerable(includeSubCategories)
                .Except(excludeCategories)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Получение списка идентификаторов категорий, имеющихся в проекте
        /// </summary>
        /// <param name="doc">Документ</param>
        /// <param name="onlyAllowsBoundParameters">Только категории, к которым можно привязать общие параметры</param>
        /// <param name="includeSubCategories">Включая подкатегории</param>
        /// <returns></returns>
        private static IEnumerable<int> GetCategoriesIdsIEnumerable(
            this Document doc,
            bool onlyAllowsBoundParameters = false,
            bool includeSubCategories = true)
        {
            foreach (Category category in doc.Settings.Categories)
            {
                if (onlyAllowsBoundParameters)
                {
                    if (category.AllowsBoundParameters)
                        yield return category.Id.IntegerValue;
                }
                else
                {
                    yield return category.Id.IntegerValue;
                }

                if (!includeSubCategories)
                    continue;

                foreach (Category subCategory in category.SubCategories)
                {
                    if (onlyAllowsBoundParameters)
                    {
                        if (subCategory.AllowsBoundParameters)
                            yield return subCategory.Id.IntegerValue;
                    }
                    else
                    {
                        yield return subCategory.Id.IntegerValue;
                    }
                }
            }
        }
    }
}
