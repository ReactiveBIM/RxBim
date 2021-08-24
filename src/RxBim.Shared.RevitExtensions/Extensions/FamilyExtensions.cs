namespace RxBim.Shared.RevitExtensions.Extensions
{
    using System.Collections.Generic;
    using Autodesk.Revit.DB;
    using Helpers;

    /// <summary>
    /// Расширения для работы с семействами Revit
    /// </summary>
    public static class FamilyExtensions
    {
        /// <summary>
        /// Добавление параметров в семейства
        /// </summary>
        /// <param name="families">Семейства</param>
        /// <param name="doc">Текущий документ Revit</param>
        /// <param name="parameters">Список имен параметров</param>
        public static void AddFamilyParameters(
            this IEnumerable<Family> families,
            Document doc,
            IEnumerable<ExternalDefinition> parameters)
        {
            foreach (var family in families)
            {
                var familyDoc = doc.EditFamily(family);
                using (var trans = new Transaction(familyDoc, "Добавление параметров в семейство"))
                {
                    trans.Start();

                    var fm = familyDoc.FamilyManager;
                    foreach (var parameter in parameters)
                    {
                        if (fm.get_Parameter(parameter.Name) == null)
                            fm.AddParameter(parameter, BuiltInParameterGroup.INVALID, true);
                    }

                    trans.Commit();
                }

                familyDoc.LoadFamily(doc, new FamilyLoadOptions());
            }
        }
    }
}
