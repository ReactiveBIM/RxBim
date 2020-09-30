namespace PikTools.Shared.RevitExtensions.Extensions
{
    using System;
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
        /// <returns>Значение параметра</returns>
        public static object GetParameterValue(
            this Parameter param)
        {
            if (param == null)
                return string.Empty;

            var doc = param.Element.Document;
            var paramName = param.Definition.Name;
            var stp = param.StorageType;
            if (paramName == "Рабочий набор"
                || paramName == "Workset")
                stp = StorageType.None;

            string value;
            switch (stp)
            {
                case StorageType.Integer:
                    return !param.HasValue ? 0 : param.AsInteger();
                case StorageType.Double:
                    {
                        var valueDouble = param.AsDouble();
                        try
                        {
                            valueDouble = UnitUtils.ConvertFromInternalUnits(param.AsDouble(), param.DisplayUnitType);
                            valueDouble = Math.Round(valueDouble, 4, MidpointRounding.ToEven);
                        }
                        catch
                        {
                            // некоторые параметры падают в исключение при вызове DisplayUnitType
                        }

                        return valueDouble;
                    }

                case StorageType.String:
                    {
                        value = param.AsString();
                        break;
                    }

                case StorageType.ElementId:
                    {
                        var el = doc.GetElement(param.AsElementId());
                        return el == null ? param.AsValueString() : el.Name;
                    }

                default:
                    value = param.AsValueString();
                    break;
            }

            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
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
            if (element == null)
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
            if (parameter == null)
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
                || toParameter == null)
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