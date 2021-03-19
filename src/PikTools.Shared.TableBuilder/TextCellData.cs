namespace PikTools.Shared.TableBuilder
{
    using PikTools.Shared.TableBuilder.Abstractions;

    /// <summary> Текстовое значение ячейки </summary>
    public class TextCellData : ICellData
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="value"> Значение </param>
        public TextCellData(string value)
        {
            Value = value;
        }

        /// <summary> Значение ячейки </summary>
        public string Value { get; set; }
    }
}