namespace RxBim.Application.Ui.Api.Abstractions
{
    using System;

    /// <summary>
    /// Кнопка
    /// </summary>
    public interface IButton
    {
        /// <summary>
        /// Добавляет всплывающее описание кнопки
        /// </summary>
        /// <param name="toolTip">Текст всплывающего описания</param>
        /// <param name="addVersion">Флаг добавления версии</param>
        IButton SetToolTip(string toolTip, bool addVersion = true);

        /// <summary>
        /// Устанавливает большое изображение
        /// </summary>
        /// <param name="imageUri">imageUri</param>
        IButton SetLargeImage(Uri imageUri);

        /// <summary>
        /// Устанавливает маленькое изображение
        /// </summary>
        /// <param name="imageUri">изображение</param>
        IButton SetSmallImage(Uri imageUri);

        /// <summary>
        /// Устанавливает описание
        /// </summary>
        /// <param name="description">описание</param>
        IButton SetLongDescription(string description);

        /// <summary>
        /// Устанавливает url справки
        /// </summary>
        /// <param name="url">url</param>
        IButton SetHelpUrl(string url);
    }
}