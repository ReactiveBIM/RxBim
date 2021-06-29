namespace PikTools.Shared.AutocadExtensions.Services
{
    using System;
    using System.Linq;
    using Abstractions;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Runtime;
    using Helpers;
    using Models;
    using AcRtException = Autodesk.AutoCAD.Runtime.Exception;

    /// <summary>
    /// Сервис выбора объектов
    /// </summary>
    public class SelectionService : ISelectionService<SelectionResult>
    {
        private readonly Editor _editor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionService"/> class.
        /// </summary>
        /// <param name="editor">Редактор документа</param>
        public SelectionService(Editor editor)
        {
            _editor = editor;
            Options = new PromptSelectionOptions();
            Options.KeywordInput += (sender, e) => throw new AcRtException(ErrorStatus.OK, e.Input);
        }

        /// <summary>
        /// Опции для выбора
        /// </summary>
        public PromptSelectionOptions Options { get; }

        /// <summary>
        /// Функция проверки объекта по идентификатору. Если возвращает истину - объект может быть выбран.
        /// По умолчанию - возвращает истину для любого объекта.
        /// </summary>
        public Func<ObjectId, bool> CheckIdFunction { get; set; } = x => true;

        /// <summary>
        /// Выбор объектов на чертеже с учетом предварительного выбора, фильтра и ключевых слов
        /// </summary>
        public SelectionResult Select()
        {
            // Обработка предварительного выбора
            var selectionResult = _editor.SelectImplied();
            if (selectionResult.Status == PromptStatus.OK)
            {
                var trueObjIds = selectionResult.Value.GetObjectIds().Where(CheckIdFunction).ToArray();
                _editor.SetImpliedSelection(trueObjIds);
            }

            using (new SelectionAddedFilter(_editor, CheckIdFunction))
            {
                try
                {
                    selectionResult = _editor.GetSelection(Options);
                }
                catch (AcRtException e)
                {
                    if (e.ErrorStatus == ErrorStatus.OK)
                    {
                        return new SelectionResult { IsKeyword = true, Keyword = e.Message };
                    }
                }
            }

            if (selectionResult.Status == PromptStatus.OK)
            {
                return new SelectionResult
                {
                    IsKeyword = false,
                    SelectedObjects = selectionResult.Value.GetObjectIds()
                };
            }

            return SelectionResult.Empty;
        }
    }
}