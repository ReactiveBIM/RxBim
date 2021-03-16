namespace PikTools.Shared.TableBuilder
{
    using PikTools.Shared.TableBuilder.Abstractions;

    /// <inheritdoc />
    public class ValueCellData : ICellData
    {
        /// <summary> ctor. </summary>
        /// <param name="value"> Value </param>
        public ValueCellData(object value)
        {
            Value = value;
        }

        /// <summary> Value </summary>
        public object Value { get; set; }
    }
}