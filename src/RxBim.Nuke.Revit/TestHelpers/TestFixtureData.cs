namespace RxBim.Nuke.Revit.TestHelpers
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Test fixture result
    /// </summary>
    public class TestFixtureData
    {
        /// <summary>
        /// Test cases
        /// </summary>
        public List<TestCaseData> Cases { get; set; } = new List<TestCaseData>();

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is fixture success
        /// </summary>
        public bool Success => Cases.All(x => x.Success);

        /// <inheritdoc/>
        public override string ToString()
        {
            var success = Success ? "✔" : "❌";
            var cases = string.Join("\n", Cases.Select(x => x.ToString()));
            return $"{Name} - {success}\n{cases}";
        }
    }
}