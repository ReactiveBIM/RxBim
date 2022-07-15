namespace RxBim.Nuke.Revit.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml;
    using RazorLight;
    using Serilog;

    /// <summary>
    /// Converts RTF xml result into html.
    /// </summary>
    public class ResultConverter
    {
        /// <summary>
        /// Converts RTF xml result into html.
        /// </summary>
        /// <param name="resultSourcePath">RTF result file path.</param>
        /// <param name="resultPath">converted result path.</param>
        public async Task Convert(string resultSourcePath, string resultPath)
        {
            var doc = await LoadSource(resultSourcePath);

            var testResultData = CreateTestResultData(doc);

            Log.Information("{Result}", testResultData.ToString());

            var result = await RenderResult(testResultData);

            await SaveResult(resultPath, result);

            Log.Information("Test results has been saved into {ResultPath}", resultPath);
        }

        private TestResultData CreateTestResultData(XmlDocument doc)
        {
            var testResultData = InitTestResultData(doc);
            var fixturesData = CreateFixturesData(doc);
            testResultData.Fixtures.AddRange(fixturesData);
            return testResultData;
        }

        private TestResultData InitTestResultData(XmlDocument doc)
        {
            var xmlAttributeCollection = doc.SelectSingleNode("/test-results")?.Attributes;
            if (xmlAttributeCollection != null)
            {
                var assemblyPath = xmlAttributeCollection["name"]?.Value;
                var assemblyName = Path.GetFileName(assemblyPath);
                var testResultData = new TestResultData
                {
                    AssemblyName = assemblyName
                };
                return testResultData;
            }

            throw new ArgumentException($"Result document has wrong structure!");
        }

        private IEnumerable<TestFixtureData> CreateFixturesData(XmlDocument doc)
        {
            var fixturesData = doc.DocumentElement?.SelectNodes("/test-results/test-suite/results/test-suite");
            if (fixturesData == null)
            {
                yield break;
            }

            foreach (XmlElement node in fixturesData)
            {
                var testFixtureData = new TestFixtureData
                {
                    Name = node.Attributes["name"]?.Value
                };
                var cases = node.FirstChild?.ChildNodes;
                if (cases != null)
                {
                    foreach (XmlElement @case in cases)
                    {
                        var testCaseData = CreateTestCaseData(@case);
                        testFixtureData.Cases.Add(testCaseData);
                    }
                }

                yield return testFixtureData;
            }
        }

        private TestCaseData CreateTestCaseData(XmlElement @case)
        {
            var testCaseData = new TestCaseData
            {
                Name = @case.Attributes["name"]?.Value,
                Success = @case.Attributes["success"]?.Value == "True",
                ExecutionTime = @case.Attributes["time"]?.Value
            };
            testCaseData.Failure = testCaseData.Success
                ? "-"
                : @case.FirstChild?.FirstChild?.InnerText + @case.FirstChild?.LastChild?.InnerText;
            return testCaseData;
        }

        private async Task<XmlDocument> LoadSource(string resultSourcePath)
        {
            var doc = new XmlDocument();
            doc.LoadXml(await File.ReadAllTextAsync(resultSourcePath));
            return doc;
        }

        private async Task<string> RenderResult(TestResultData testResultData)
        {
            var engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetExecutingAssembly())
                .UseMemoryCachingProvider()
                .UseOptions(new RazorLightOptions
                {
                    DisableEncoding = true
                })
                .Build();

            var result = await engine.CompileRenderAsync(typeof(Result).FullName, testResultData);
            return result;
        }

        private async Task SaveResult(string resultPath, string result)
        {
            await File.WriteAllTextAsync(resultPath, result);
        }
    }
}