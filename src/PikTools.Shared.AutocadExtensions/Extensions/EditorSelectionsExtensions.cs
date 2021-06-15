namespace PikTools.Shared.AutocadExtensions.Extensions
{
    using System;
    using System.Linq;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Runtime;
    using Helpers;
    using JetBrains.Annotations;
    using Models;
    using AcRtException = Autodesk.AutoCAD.Runtime.Exception;

    /// <summary>
    /// Расширения редактора для выбора объектов
    /// </summary>
    [PublicAPI]
    public static class EditorSelectionsExtensions
    {
        private static readonly Func<ObjectId, bool> DefaultCheckFunc = x => true;
        private static Func<ObjectId, bool> _checkIdFunc = DefaultCheckFunc;

        /// <summary>
        /// Выбор объектов на чертеже с учетом предварительного выбора, фильтра и ключевых слов
        /// </summary>
        /// <param name="editor">Редактор документа</param>
        /// <param name="selectionOptions">Опции выбора</param>
        /// <param name="checkIdFunc">Функция проверки Id. Если возвращает true - объект выбирается</param>
        public static SelectionResult SelectObjects(
            this Editor editor,
            PromptSelectionOptions selectionOptions,
            Func<ObjectId, bool>? checkIdFunc)
        {
            _checkIdFunc = checkIdFunc ?? DefaultCheckFunc;
            selectionOptions.KeywordInput += (sender, e) => throw new AcRtException(ErrorStatus.OK, e.Input);

            // Обработка предварительного выбора
            var selectionResult = editor.SelectImplied();
            if (selectionResult.Status == PromptStatus.OK)
            {
                var trueObjIds = selectionResult.Value.GetObjectIds().Where(_checkIdFunc).ToArray();
                editor.SetImpliedSelection(trueObjIds);
            }

            using (new SelectionAddedFilter(editor, _checkIdFunc))
            {
                try
                {
                    selectionResult = editor.GetSelection(selectionOptions);
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
                    { IsKeyword = false, SelectedObjects = selectionResult.Value.GetObjectIds() };
            }

            return SelectionResult.Empty;
        }
    }
}