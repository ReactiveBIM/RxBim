namespace PikTools.Shared.AutocadExtensions.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Runtime;
    using Helpers;
    using Models;
    using AcRtException = Autodesk.AutoCAD.Runtime.Exception;

    /// <inheritdoc />
    public class ObjectsSelectionService : IObjectsSelectionService
    {
        private readonly Editor _editor;
        private readonly PromptSelectionOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectsSelectionService"/> class.
        /// </summary>
        /// <param name="editor">Редактор документа</param>
        public ObjectsSelectionService(Editor editor)
        {
            _editor = editor;
            _options = new PromptSelectionOptions();
            _options.KeywordInput += (sender, e) => throw new AcRtException(ErrorStatus.OK, e.Input);
        }

        /// <inheritdoc />
        public Func<ObjectId, bool> CanBeSelected { get; set; } = x => true;

        /// <inheritdoc />
        public IObjectsSelectionResult RunSelection()
        {
            // Обработка предварительного выбора
            var selectionResult = _editor.SelectImplied();
            if (selectionResult.Status == PromptStatus.OK)
            {
                var trueObjIds = selectionResult.Value.GetObjectIds().Where(CanBeSelected).ToArray();
                _editor.SetImpliedSelection(trueObjIds);
            }

            using (new SelectionAddedFilter(_editor, CanBeSelected))
            {
                try
                {
                    selectionResult = _editor.GetSelection(_options);
                }
                catch (AcRtException e)
                {
                    if (e.ErrorStatus == ErrorStatus.OK)
                    {
                        return new ObjectsSelectionResult { IsKeyword = true, Keyword = e.Message };
                    }
                }
            }

            if (selectionResult.Status == PromptStatus.OK)
            {
                return new ObjectsSelectionResult
                {
                    IsKeyword = false,
                    SelectedObjects = selectionResult.Value.GetObjectIds()
                };
            }

            return ObjectsSelectionResult.Empty;
        }

        /// <inheritdoc />
        public void SetMessageAndKeywords(string message, Dictionary<string, string>? keywordGlobalAndLocalNames = null)
        {
            _options.Keywords.Clear();
            _options.MessageForAdding = message;

            if (keywordGlobalAndLocalNames != null && keywordGlobalAndLocalNames.Count > 0)
            {
                foreach (var globalAndLocalName in keywordGlobalAndLocalNames)
                {
                    _options.Keywords.Add(globalAndLocalName.Key, globalAndLocalName.Value);
                }

                _options.MessageForAdding += _options.Keywords.GetDisplayString(true);
            }
        }
    }
}