namespace RxBim.Shared.RevitExtensions.Models
{
    using System;
    using Autodesk.Revit.DB;

    /// <summary>
    /// Данные общего параметра, описывающие его в ФОП
    /// </summary>
    public class SharedParameterDefinition
    {
        /// <summary>
        /// Имя параметра
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Guid
        /// </summary>
        public Guid? Guid { get; set; }

        /// <summary>
        /// Тип данных <see cref="ParameterType"/>
        /// </summary>
        public ParameterType? DataType { get; set; }

        /// <summary>
        /// Имя группы в которую должен входить параметр
        /// </summary>
        public string OwnerGroupName { get; set; }

        /// <summary>
        /// Описание параметра
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Видимость параметра в проекте
        /// </summary>
        public bool? Visible { get; set; }

        /// <summary>
        /// Доступность параметра для редактирования
        /// </summary>
        public bool? UserModifiable { get; set; }
    }
}
