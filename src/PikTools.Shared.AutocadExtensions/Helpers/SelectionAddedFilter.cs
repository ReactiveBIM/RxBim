namespace PikTools.Shared.AutocadExtensions.Helpers
{
    using System;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;

    /// <summary>
    /// Вспомогательный класс для фильтрации добавляемого в пользовательский выбор объекта
    /// </summary>
    internal class SelectionAddedFilter : IDisposable
    {
        private readonly Func<ObjectId, bool> _checkFunc;
        private readonly Editor _editor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionAddedFilter"/> class.
        /// </summary>
        /// <param name="editor">Редактор</param>
        /// <param name="checkFunc">
        /// Функция проверки идентификатора, которая возвращает истину,
        /// если объект является подходящим для добавления в выбор
        /// </param>
        public SelectionAddedFilter(Editor editor, Func<ObjectId, bool> checkFunc)
        {
            _editor = editor;
            _checkFunc = checkFunc;
            _editor.SelectionAdded += EditorSelectionAdded;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _editor.SelectionAdded -= EditorSelectionAdded;
        }

        /// <summary>
        /// Метод фильтрации выбираемых с помощью GetSelection объектов в режиме реального времени
        /// </summary>
        private void EditorSelectionAdded(object sender, SelectionAddedEventArgs e)
        {
            var selIds = e.AddedObjects.GetObjectIds();
            for (var i = 0; i < selIds.Length; i++)
            {
                if (!_checkFunc(selIds[i]))
                {
                    e.Remove(i);
                }
            }
        }
    }
}