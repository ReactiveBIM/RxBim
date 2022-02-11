namespace RxBim.Nuke.Revit.TestHelpers
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Integration tests result
    /// </summary>
    public class TestResultData
    {
        /// <summary>
        /// Test fixtures
        /// </summary>
        public List<TestFixtureData> Fixtures { get; set; } = new List<TestFixtureData>();

        /// <summary>
        /// Name of test assembly
        /// </summary>
        public string AssemblyName { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var fixtures = string.Join("\n", Fixtures.Select(x => x.ToString()));
            return $"{AssemblyName}\n{fixtures}";
        }
    }
}