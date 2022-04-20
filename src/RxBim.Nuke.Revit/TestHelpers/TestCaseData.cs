namespace RxBim.Nuke.Revit.TestHelpers
{
    /// <summary>
    /// Test case result.
    /// </summary>
    public class TestCaseData
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is case success.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Execution time.
        /// </summary>
        public string ExecutionTime { get; set; }

        /// <summary>
        /// Failure message.
        /// </summary>
        public string Failure { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var success = Success ? "✔" : "❌";
            var failure = Success ? string.Empty : $"\n\t{Failure}";
            return $"{Name} - {success} - {ExecutionTime}{failure}";
        }
    }
}