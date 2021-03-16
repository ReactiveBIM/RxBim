namespace PikTools.Shared.RevitExtensions.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Autodesk.Revit.DB;

    /// <summary>
    /// Расширения для параметра элемента Revit
    /// </summary>
    public static class ParameterExtensions
    {
        /// <summary>
        /// Получает параметр из экземпляра или типа элемента
        /// </summary>
        /// <param name="elem">Element</param>
        /// <param name="parameterName">Имя параметра</param>
        public static Parameter GetParameterFromInstanceOrType(
            this Element elem,
            string parameterName)
        {
            if (!elem.IsValidObject)
                return null;

            var param = elem.LookupParameter(parameterName);
            if (param != null)
                return param;

            var typeId = elem.GetTypeId();
            if (typeId == null)
                return null;

            var type = elem.Document?.GetElement(typeId);

            param = type?.LookupParameter(parameterName);
            return param;
        }

        /// <summary>
        /// Возвращает значение параметра
        /// </summary>
        /// <param name="param">Параметр</param>
        /// <param name="digits">Количество цифр дробной части в возвращаемом значении</param>
        /// <param name="getFeet">Получить значение во внутренних единицах измерения Revit</param>
        /// <returns>Значение параметра</returns>
        public static object GetParameterValue(
            this Parameter param,
            int digits = 4,
            bool getFeet = false)
        {
            if (param == null)
                return default;

            var paramName = param.Definition.Name;
            var storageType = param.StorageType;
            if (paramName == "Рабочий набор"
                || paramName == "Workset")
                storageType = StorageType.None;

            object value = null;
            switch (storageType)
            {
                case StorageType.String:
                    value = param.AsString() ?? string.Empty;
                    break;

                case StorageType.Double:
                    try
                    {
                        value = getFeet
                            ? Math.Round(param.AsDouble(), digits)
                            : Math.Round(
                                UnitUtils.ConvertFromInternalUnits(
                                    param.AsDouble(),
                                    param.DisplayUnitType), digits);
                    }
                    catch (Autodesk.Revit.Exceptions.InvalidOperationException)
                    {
                        value = Math.Round(
                            UnitUtils.ConvertFromInternalUnits(
                                param.AsDouble(),
                                DisplayUnitType.DUT_GENERAL), digits);
                    }

                    break;

                case StorageType.Integer:
                    value = param.HasValue ? param.AsInteger() : 0;
                    break;

                case StorageType.ElementId:
                    value = param.HasValue ? param.AsElementId() : ElementId.InvalidElementId;
                    break;

                case StorageType.None:
                    value = param.AsValueString() ?? string.Empty;
                    break;
            }

            return value;
        }

        /// <summary>
        /// Возвращает значение заданного параметра
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения</typeparam>
        /// <param name="element">Элемент Revit</param>
        /// <param name="parameterName">Имя параметра элемента</param>
        /// <param name="digits">Количество цифр дробной части в возвращаемом значении</param>
        /// <param name="getFeet">Получить значение во внутренних единицах измерения Revit</param>
        /// <returns>Значение параметра</returns>
        public static T GetParameterValue<T>(
            this Element element,
            string parameterName,
            int digits = 4,
            bool getFeet = false)
        {
            var foundParameter = element.GetParameterFromInstanceOrType(parameterName);

            try
            {
                if (foundParameter == null)
                    return default;

                var value = foundParameter.GetParameterValue(digits, getFeet);

                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// Задать значение параметру элемента
        /// </summary>
        /// <param name="element">Элемент Revit</param>
        /// <param name="parameterName">Название параметра</param>
        /// <param name="value">Значение</param>
        /// <returns>true - значение задано, иначе - false</returns>
        public static bool SetParameterValue(
            this Element element,
            string parameterName,
            object value)
        {
            if (element == null
                || !element.IsValidObject)
                return false;

            var parameter = element.LookupParameter(parameterName);
            return parameter != null
                   && SetParameterValue(parameter, value);
        }

        /// <summary>
        /// Задать значение параметру
        /// </summary>
        /// <param name="parameter">Параметр</param>
        /// <param name="value">Значение</param>
        /// <returns>true - значение задано, иначе - false</returns>
        public static bool SetParameterValue(
            this Parameter parameter,
            object value)
        {
            if (parameter == null
                || parameter.IsReadOnly)
                return false;

            switch (parameter.StorageType)
            {
                case StorageType.String:
                    return parameter.Set(value.ToString());

                case StorageType.Integer:
                    if (!(value is int iValue))
                        return false;
                    return parameter.Set(iValue);

                case StorageType.Double:
                    if (!(value is double dValue))
                        return false;
                    return parameter.Set(UnitUtils.ConvertToInternalUnits(dValue, parameter.DisplayUnitType));

                case StorageType.ElementId:
                    if (!(value is ElementId idValue))
                        return false;
                    return parameter.Set(idValue);
            }

            return false;
        }

        /// <summary>
        /// Копирование значения между параметрами
        /// </summary>
        /// <param name="fromParameter">Из параметра</param>
        /// <param name="toParameter">В параметр</param>
        public static void CopyParameterValue(this Parameter fromParameter, Parameter toParameter)
        {
            if (fromParameter == null
                || toParameter == null
                || toParameter.IsReadOnly)
                return;

            switch (fromParameter.StorageType)
            {
                case StorageType.Double:
                    if (toParameter.StorageType == StorageType.Double)
                    {
                        toParameter.Set(UnitUtils.ConvertFromInternalUnits(fromParameter.AsDouble(), fromParameter.DisplayUnitType));
                    }
                    else
                    {
                        var asValueString = fromParameter.AsValueString() ?? string.Empty;
                        var firstOrDefault = asValueString.Split(' ').FirstOrDefault();
                        toParameter.Set(firstOrDefault);
                    }

                    break;
                case StorageType.ElementId:
                    toParameter.Set(fromParameter.AsElementId());
                    break;
                case StorageType.Integer:
                    if (fromParameter.Definition.ParameterType == ParameterType.YesNo
                        && toParameter.StorageType == StorageType.String)
                        toParameter.Set(fromParameter.AsValueString());
                    else
                        toParameter.Set(fromParameter.AsInteger());

                    break;
                case StorageType.String:
                    toParameter.Set(fromParameter.AsString());
                    break;
                case StorageType.None:
                    toParameter.SetValueString(fromParameter.AsValueString());
                    break;
            }
        }
    }
}