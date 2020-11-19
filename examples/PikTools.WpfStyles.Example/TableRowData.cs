namespace PikTools.WpfStyles.Example
{
    public class TableRowData
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Access { get; set; }
        public bool IsChecked { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as TableRowData;
            if (other == null)
                return false;

            return Name?.Equals(other.Name) ?? false;
        }
    }
}