// Generated from https://raw.githubusercontent.com/ReactiveBIM/RxBim/feature/RevitIntegrationTests/src/RxBim.Nuke.Revit/RevitTestFramework.json

using JetBrains.Annotations;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Tooling;
using Nuke.Common.Tools;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

/// <summary>
///   <p>The Revit Test Framework (RTF) allows you to conduct remote unit testing on Revit. RTF takes care of creating a journal file for running revit which can specify a model to start Revit, and a specific test or fixture of tests to run. You can even specify a model to open before testing and RTF will do that as well.</p>
///   <p>For more details, visit the <a href="https://github.com/DynamoDS/RevitTestFramework">official website</a>.</p>
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class RevitTestTasks
{
    /// <summary>
    ///   Path to the RevitTest executable.
    /// </summary>
    public static string RevitTestPath =>
        ToolPathResolver.TryGetEnvironmentExecutable("REVITTEST_EXE") ??
        ToolPathResolver.GetPackageExecutable("RevitTestFramework", "RevitTestFrameworkConsole.exe");
    public static Action<OutputType, string> RevitTestLogger { get; set; } = ProcessTasks.DefaultLogger;
    /// <summary>
    ///   <p>The Revit Test Framework (RTF) allows you to conduct remote unit testing on Revit. RTF takes care of creating a journal file for running revit which can specify a model to start Revit, and a specific test or fixture of tests to run. You can even specify a model to open before testing and RTF will do that as well.</p>
    ///   <p>For more details, visit the <a href="https://github.com/DynamoDS/RevitTestFramework">official website</a>.</p>
    /// </summary>
    public static IReadOnlyCollection<Output> RevitTest(string arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, bool? logTimestamp = null, string logFile = null, Func<string, string> outputFilter = null)
    {
        using var process = ProcessTasks.StartProcess(RevitTestPath, arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logTimestamp, logFile, RevitTestLogger, outputFilter);
        process.AssertZeroExitCode();
        return process.Output;
    }
    /// <summary>
    ///   <p>The Revit Test Framework (RTF) allows you to conduct remote unit testing on Revit. RTF takes care of creating a journal file for running revit which can specify a model to start Revit, and a specific test or fixture of tests to run. You can even specify a model to open before testing and RTF will do that as well.</p>
    ///   <p>For more details, visit the <a href="https://github.com/DynamoDS/RevitTestFramework">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>--assembly</c> via <see cref="RevitTestSettings.Assembly"/></li>
    ///     <li><c>--category</c> via <see cref="RevitTestSettings.Category"/></li>
    ///     <li><c>--clean</c> via <see cref="RevitTestSettings.Clean"/></li>
    ///     <li><c>--concatenate</c> via <see cref="RevitTestSettings.Concatenate"/></li>
    ///     <li><c>--continuous</c> via <see cref="RevitTestSettings.Continuous"/></li>
    ///     <li><c>--copyAddins</c> via <see cref="RevitTestSettings.CopyAddins"/></li>
    ///     <li><c>--debug</c> via <see cref="RevitTestSettings.Debug"/></li>
    ///     <li><c>--dir</c> via <see cref="RevitTestSettings.Dir"/></li>
    ///     <li><c>--dry</c> via <see cref="RevitTestSettings.Dry"/></li>
    ///     <li><c>--exclude</c> via <see cref="RevitTestSettings.Exclude"/></li>
    ///     <li><c>--fixture</c> via <see cref="RevitTestSettings.Fixture"/></li>
    ///     <li><c>--groupByModel</c> via <see cref="RevitTestSettings.GroupByModel"/></li>
    ///     <li><c>--help</c> via <see cref="RevitTestSettings.Help"/></li>
    ///     <li><c>--results</c> via <see cref="RevitTestSettings.Results"/></li>
    ///     <li><c>--revit</c> via <see cref="RevitTestSettings.Revit"/></li>
    ///     <li><c>--testName</c> via <see cref="RevitTestSettings.TestName"/></li>
    ///     <li><c>--time</c> via <see cref="RevitTestSettings.Time"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> RevitTest(RevitTestSettings toolSettings = null)
    {
        toolSettings = toolSettings ?? new RevitTestSettings();
        using var process = ProcessTasks.StartProcess(toolSettings);
        process.AssertZeroExitCode();
        return process.Output;
    }
    /// <summary>
    ///   <p>The Revit Test Framework (RTF) allows you to conduct remote unit testing on Revit. RTF takes care of creating a journal file for running revit which can specify a model to start Revit, and a specific test or fixture of tests to run. You can even specify a model to open before testing and RTF will do that as well.</p>
    ///   <p>For more details, visit the <a href="https://github.com/DynamoDS/RevitTestFramework">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>--assembly</c> via <see cref="RevitTestSettings.Assembly"/></li>
    ///     <li><c>--category</c> via <see cref="RevitTestSettings.Category"/></li>
    ///     <li><c>--clean</c> via <see cref="RevitTestSettings.Clean"/></li>
    ///     <li><c>--concatenate</c> via <see cref="RevitTestSettings.Concatenate"/></li>
    ///     <li><c>--continuous</c> via <see cref="RevitTestSettings.Continuous"/></li>
    ///     <li><c>--copyAddins</c> via <see cref="RevitTestSettings.CopyAddins"/></li>
    ///     <li><c>--debug</c> via <see cref="RevitTestSettings.Debug"/></li>
    ///     <li><c>--dir</c> via <see cref="RevitTestSettings.Dir"/></li>
    ///     <li><c>--dry</c> via <see cref="RevitTestSettings.Dry"/></li>
    ///     <li><c>--exclude</c> via <see cref="RevitTestSettings.Exclude"/></li>
    ///     <li><c>--fixture</c> via <see cref="RevitTestSettings.Fixture"/></li>
    ///     <li><c>--groupByModel</c> via <see cref="RevitTestSettings.GroupByModel"/></li>
    ///     <li><c>--help</c> via <see cref="RevitTestSettings.Help"/></li>
    ///     <li><c>--results</c> via <see cref="RevitTestSettings.Results"/></li>
    ///     <li><c>--revit</c> via <see cref="RevitTestSettings.Revit"/></li>
    ///     <li><c>--testName</c> via <see cref="RevitTestSettings.TestName"/></li>
    ///     <li><c>--time</c> via <see cref="RevitTestSettings.Time"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> RevitTest(Configure<RevitTestSettings> configurator)
    {
        return RevitTest(configurator(new RevitTestSettings()));
    }
    /// <summary>
    ///   <p>The Revit Test Framework (RTF) allows you to conduct remote unit testing on Revit. RTF takes care of creating a journal file for running revit which can specify a model to start Revit, and a specific test or fixture of tests to run. You can even specify a model to open before testing and RTF will do that as well.</p>
    ///   <p>For more details, visit the <a href="https://github.com/DynamoDS/RevitTestFramework">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>--assembly</c> via <see cref="RevitTestSettings.Assembly"/></li>
    ///     <li><c>--category</c> via <see cref="RevitTestSettings.Category"/></li>
    ///     <li><c>--clean</c> via <see cref="RevitTestSettings.Clean"/></li>
    ///     <li><c>--concatenate</c> via <see cref="RevitTestSettings.Concatenate"/></li>
    ///     <li><c>--continuous</c> via <see cref="RevitTestSettings.Continuous"/></li>
    ///     <li><c>--copyAddins</c> via <see cref="RevitTestSettings.CopyAddins"/></li>
    ///     <li><c>--debug</c> via <see cref="RevitTestSettings.Debug"/></li>
    ///     <li><c>--dir</c> via <see cref="RevitTestSettings.Dir"/></li>
    ///     <li><c>--dry</c> via <see cref="RevitTestSettings.Dry"/></li>
    ///     <li><c>--exclude</c> via <see cref="RevitTestSettings.Exclude"/></li>
    ///     <li><c>--fixture</c> via <see cref="RevitTestSettings.Fixture"/></li>
    ///     <li><c>--groupByModel</c> via <see cref="RevitTestSettings.GroupByModel"/></li>
    ///     <li><c>--help</c> via <see cref="RevitTestSettings.Help"/></li>
    ///     <li><c>--results</c> via <see cref="RevitTestSettings.Results"/></li>
    ///     <li><c>--revit</c> via <see cref="RevitTestSettings.Revit"/></li>
    ///     <li><c>--testName</c> via <see cref="RevitTestSettings.TestName"/></li>
    ///     <li><c>--time</c> via <see cref="RevitTestSettings.Time"/></li>
    ///   </ul>
    /// </remarks>
    public static IEnumerable<(RevitTestSettings Settings, IReadOnlyCollection<Output> Output)> RevitTest(CombinatorialConfigure<RevitTestSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false)
    {
        return configurator.Invoke(RevitTest, RevitTestLogger, degreeOfParallelism, completeOnFailure);
    }
}
#region RevitTestSettings
/// <summary>
///   Used within <see cref="RevitTestTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Serializable]
public partial class RevitTestSettings : ToolSettings
{
    /// <summary>
    ///   Path to the RevitTest executable.
    /// </summary>
    public override string ProcessToolPath => base.ProcessToolPath ?? RevitTestTasks.RevitTestPath;
    public override Action<OutputType, string> ProcessCustomLogger => RevitTestTasks.RevitTestLogger;
    /// <summary>
    ///   The full path to the working directory. The working directory is the directory in which RTF will generate the journal and the addin to Run Revit. Revit's run-by-journal capability requires that all addins which need to be loaded are in the same directory as the journal file. So, if you're testing other addins on top of Revit using RTF, you'll need to put those addins in whatever directory you specify as the working directory.
    /// </summary>
    public virtual string Dir { get; internal set; }
    /// <summary>
    ///   The full path to the assembly containing your tests.
    /// </summary>
    public virtual string Assembly { get; internal set; }
    /// <summary>
    ///   The full path to an .xml file that will contain the results.
    /// </summary>
    public virtual string Results { get; internal set; }
    /// <summary>
    ///   The full name (with namespace) of a test fixture to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly.(OPTIONAL)
    /// </summary>
    public virtual IReadOnlyList<string> Fixture => FixtureInternal.AsReadOnly();
    internal List<string> FixtureInternal { get; set; } = new List<string>();
    /// <summary>
    ///   The name of a test to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)
    /// </summary>
    public virtual IReadOnlyList<string> TestName => TestNameInternal.AsReadOnly();
    internal List<string> TestNameInternal { get; set; } = new List<string>();
    /// <summary>
    ///   The name of a test category to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)
    /// </summary>
    public virtual IReadOnlyList<string> Category => CategoryInternal.AsReadOnly();
    internal List<string> CategoryInternal { get; set; } = new List<string>();
    /// <summary>
    ///   The name of a test category to exclude. This has a higher priority than other settings. If a specified category is set here, any test cases that belongs to that category will not be run. (OPTIONAL)
    /// </summary>
    public virtual IReadOnlyList<string> Exclude => ExcludeInternal.AsReadOnly();
    internal List<string> ExcludeInternal { get; set; } = new List<string>();
    /// <summary>
    ///   Concatenate the results from this run of RTF with an existing results file if one exists at the path specified. The default behavior is to replace the existing results file. (OPTIONAL)
    /// </summary>
    public virtual bool? Concatenate { get; internal set; }
    /// <summary>
    ///   Specify whether to copy the addins from the Revit folder to the current working directory. Copying the addins from the Revit folder will cause the test process to simulate the typical setup on your machine. (OPTIONAL)
    /// </summary>
    public virtual bool? CopyAddins { get; internal set; }
    /// <summary>
    ///   Run all selected tests in one Revit session. (OPTIONAL)
    /// </summary>
    public virtual bool? Continuous { get; internal set; }
    /// <summary>
    ///   The time, in milliseconds, after which RTF will close the testing process automatically. (OPTIONAL)
    /// </summary>
    public virtual int? Time { get; internal set; }
    /// <summary>
    ///   Run tests with same model without reopening the model for faster execution, requires continuous. (OPTIONAL)
    /// </summary>
    public virtual bool? GroupByModel { get; internal set; }
    /// <summary>
    ///   Conduct a dry run. (OPTIONAL)
    /// </summary>
    public virtual bool? Dry { get; internal set; }
    /// <summary>
    ///   Cleanup journal files after test completion. (OPTIONAL)
    /// </summary>
    public virtual bool? Clean { get; internal set; }
    /// <summary>
    ///   Should RTF attempt to attach to a debugger?. (OPTIONAL)
    /// </summary>
    public virtual bool? Debug { get; internal set; }
    /// <summary>
    ///   The Revit executable to be used for testing. If no executable is specified, RTF will use the first version of Revit that is found on the machine using the RevitAddinUtility. (OPTIONAL)
    /// </summary>
    public virtual string Revit { get; internal set; }
    /// <summary>
    ///   Show this message and exit. (OPTIONAL)
    /// </summary>
    public virtual bool? Help { get; internal set; }
    protected override Arguments ConfigureProcessArguments(Arguments arguments)
    {
        arguments
          .Add("--dir={value}", Dir)
          .Add("--assembly={value}", Assembly)
          .Add("--results={value}", Results)
          .Add("--fixture={value}", Fixture, separator: ' ')
          .Add("--testName={value}", TestName, separator: ' ')
          .Add("--category={value}", Category)
          .Add("--exclude={value}", Exclude, separator: ' ')
          .Add("--concatenate", Concatenate)
          .Add("--copyAddins", CopyAddins)
          .Add("--continuous", Continuous)
          .Add("--time", Time)
          .Add("--groupByModel", GroupByModel)
          .Add("--dry", Dry)
          .Add("--clean", Clean)
          .Add("--debug", Debug)
          .Add("--revit={value}", Revit)
          .Add("--help", Help);
        return base.ConfigureProcessArguments(arguments);
    }
}
#endregion
#region RevitTestSettingsExtensions
/// <summary>
///   Used within <see cref="RevitTestTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class RevitTestSettingsExtensions
{
    #region Dir
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Dir"/></em></p>
    ///   <p>The full path to the working directory. The working directory is the directory in which RTF will generate the journal and the addin to Run Revit. Revit's run-by-journal capability requires that all addins which need to be loaded are in the same directory as the journal file. So, if you're testing other addins on top of Revit using RTF, you'll need to put those addins in whatever directory you specify as the working directory.</p>
    /// </summary>
    [Pure]
    public static T SetDir<T>(this T toolSettings, string dir) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Dir = dir;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="RevitTestSettings.Dir"/></em></p>
    ///   <p>The full path to the working directory. The working directory is the directory in which RTF will generate the journal and the addin to Run Revit. Revit's run-by-journal capability requires that all addins which need to be loaded are in the same directory as the journal file. So, if you're testing other addins on top of Revit using RTF, you'll need to put those addins in whatever directory you specify as the working directory.</p>
    /// </summary>
    [Pure]
    public static T ResetDir<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Dir = null;
        return toolSettings;
    }
    #endregion
    #region Assembly
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Assembly"/></em></p>
    ///   <p>The full path to the assembly containing your tests.</p>
    /// </summary>
    [Pure]
    public static T SetAssembly<T>(this T toolSettings, string assembly) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Assembly = assembly;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="RevitTestSettings.Assembly"/></em></p>
    ///   <p>The full path to the assembly containing your tests.</p>
    /// </summary>
    [Pure]
    public static T ResetAssembly<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Assembly = null;
        return toolSettings;
    }
    #endregion
    #region Results
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Results"/></em></p>
    ///   <p>The full path to an .xml file that will contain the results.</p>
    /// </summary>
    [Pure]
    public static T SetResults<T>(this T toolSettings, string results) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Results = results;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="RevitTestSettings.Results"/></em></p>
    ///   <p>The full path to an .xml file that will contain the results.</p>
    /// </summary>
    [Pure]
    public static T ResetResults<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Results = null;
        return toolSettings;
    }
    #endregion
    #region Fixture
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Fixture"/> to a new list</em></p>
    ///   <p>The full name (with namespace) of a test fixture to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly.(OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetFixture<T>(this T toolSettings, params string[] fixture) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.FixtureInternal = fixture.ToList();
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Fixture"/> to a new list</em></p>
    ///   <p>The full name (with namespace) of a test fixture to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly.(OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetFixture<T>(this T toolSettings, IEnumerable<string> fixture) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.FixtureInternal = fixture.ToList();
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Adds values to <see cref="RevitTestSettings.Fixture"/></em></p>
    ///   <p>The full name (with namespace) of a test fixture to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly.(OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T AddFixture<T>(this T toolSettings, params string[] fixture) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.FixtureInternal.AddRange(fixture);
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Adds values to <see cref="RevitTestSettings.Fixture"/></em></p>
    ///   <p>The full name (with namespace) of a test fixture to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly.(OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T AddFixture<T>(this T toolSettings, IEnumerable<string> fixture) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.FixtureInternal.AddRange(fixture);
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Clears <see cref="RevitTestSettings.Fixture"/></em></p>
    ///   <p>The full name (with namespace) of a test fixture to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly.(OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ClearFixture<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.FixtureInternal.Clear();
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Removes values from <see cref="RevitTestSettings.Fixture"/></em></p>
    ///   <p>The full name (with namespace) of a test fixture to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly.(OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T RemoveFixture<T>(this T toolSettings, params string[] fixture) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        var hashSet = new HashSet<string>(fixture);
        toolSettings.FixtureInternal.RemoveAll(x => hashSet.Contains(x));
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Removes values from <see cref="RevitTestSettings.Fixture"/></em></p>
    ///   <p>The full name (with namespace) of a test fixture to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly.(OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T RemoveFixture<T>(this T toolSettings, IEnumerable<string> fixture) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        var hashSet = new HashSet<string>(fixture);
        toolSettings.FixtureInternal.RemoveAll(x => hashSet.Contains(x));
        return toolSettings;
    }
    #endregion
    #region TestName
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.TestName"/> to a new list</em></p>
    ///   <p>The name of a test to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetTestName<T>(this T toolSettings, params string[] testName) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TestNameInternal = testName.ToList();
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.TestName"/> to a new list</em></p>
    ///   <p>The name of a test to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetTestName<T>(this T toolSettings, IEnumerable<string> testName) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TestNameInternal = testName.ToList();
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Adds values to <see cref="RevitTestSettings.TestName"/></em></p>
    ///   <p>The name of a test to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T AddTestName<T>(this T toolSettings, params string[] testName) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TestNameInternal.AddRange(testName);
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Adds values to <see cref="RevitTestSettings.TestName"/></em></p>
    ///   <p>The name of a test to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T AddTestName<T>(this T toolSettings, IEnumerable<string> testName) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TestNameInternal.AddRange(testName);
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Clears <see cref="RevitTestSettings.TestName"/></em></p>
    ///   <p>The name of a test to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ClearTestName<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TestNameInternal.Clear();
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Removes values from <see cref="RevitTestSettings.TestName"/></em></p>
    ///   <p>The name of a test to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T RemoveTestName<T>(this T toolSettings, params string[] testName) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        var hashSet = new HashSet<string>(testName);
        toolSettings.TestNameInternal.RemoveAll(x => hashSet.Contains(x));
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Removes values from <see cref="RevitTestSettings.TestName"/></em></p>
    ///   <p>The name of a test to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T RemoveTestName<T>(this T toolSettings, IEnumerable<string> testName) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        var hashSet = new HashSet<string>(testName);
        toolSettings.TestNameInternal.RemoveAll(x => hashSet.Contains(x));
        return toolSettings;
    }
    #endregion
    #region Category
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Category"/> to a new list</em></p>
    ///   <p>The name of a test category to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetCategory<T>(this T toolSettings, params string[] category) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CategoryInternal = category.ToList();
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Category"/> to a new list</em></p>
    ///   <p>The name of a test category to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetCategory<T>(this T toolSettings, IEnumerable<string> category) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CategoryInternal = category.ToList();
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Adds values to <see cref="RevitTestSettings.Category"/></em></p>
    ///   <p>The name of a test category to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T AddCategory<T>(this T toolSettings, params string[] category) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CategoryInternal.AddRange(category);
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Adds values to <see cref="RevitTestSettings.Category"/></em></p>
    ///   <p>The name of a test category to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T AddCategory<T>(this T toolSettings, IEnumerable<string> category) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CategoryInternal.AddRange(category);
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Clears <see cref="RevitTestSettings.Category"/></em></p>
    ///   <p>The name of a test category to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ClearCategory<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CategoryInternal.Clear();
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Removes values from <see cref="RevitTestSettings.Category"/></em></p>
    ///   <p>The name of a test category to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T RemoveCategory<T>(this T toolSettings, params string[] category) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        var hashSet = new HashSet<string>(category);
        toolSettings.CategoryInternal.RemoveAll(x => hashSet.Contains(x));
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Removes values from <see cref="RevitTestSettings.Category"/></em></p>
    ///   <p>The name of a test category to run. If no fixture, no category and no test names are specified, RTF will run all tests in the assembly. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T RemoveCategory<T>(this T toolSettings, IEnumerable<string> category) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        var hashSet = new HashSet<string>(category);
        toolSettings.CategoryInternal.RemoveAll(x => hashSet.Contains(x));
        return toolSettings;
    }
    #endregion
    #region Exclude
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Exclude"/> to a new list</em></p>
    ///   <p>The name of a test category to exclude. This has a higher priority than other settings. If a specified category is set here, any test cases that belongs to that category will not be run. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetExclude<T>(this T toolSettings, params string[] exclude) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ExcludeInternal = exclude.ToList();
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Exclude"/> to a new list</em></p>
    ///   <p>The name of a test category to exclude. This has a higher priority than other settings. If a specified category is set here, any test cases that belongs to that category will not be run. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetExclude<T>(this T toolSettings, IEnumerable<string> exclude) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ExcludeInternal = exclude.ToList();
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Adds values to <see cref="RevitTestSettings.Exclude"/></em></p>
    ///   <p>The name of a test category to exclude. This has a higher priority than other settings. If a specified category is set here, any test cases that belongs to that category will not be run. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T AddExclude<T>(this T toolSettings, params string[] exclude) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ExcludeInternal.AddRange(exclude);
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Adds values to <see cref="RevitTestSettings.Exclude"/></em></p>
    ///   <p>The name of a test category to exclude. This has a higher priority than other settings. If a specified category is set here, any test cases that belongs to that category will not be run. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T AddExclude<T>(this T toolSettings, IEnumerable<string> exclude) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ExcludeInternal.AddRange(exclude);
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Clears <see cref="RevitTestSettings.Exclude"/></em></p>
    ///   <p>The name of a test category to exclude. This has a higher priority than other settings. If a specified category is set here, any test cases that belongs to that category will not be run. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ClearExclude<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ExcludeInternal.Clear();
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Removes values from <see cref="RevitTestSettings.Exclude"/></em></p>
    ///   <p>The name of a test category to exclude. This has a higher priority than other settings. If a specified category is set here, any test cases that belongs to that category will not be run. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T RemoveExclude<T>(this T toolSettings, params string[] exclude) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        var hashSet = new HashSet<string>(exclude);
        toolSettings.ExcludeInternal.RemoveAll(x => hashSet.Contains(x));
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Removes values from <see cref="RevitTestSettings.Exclude"/></em></p>
    ///   <p>The name of a test category to exclude. This has a higher priority than other settings. If a specified category is set here, any test cases that belongs to that category will not be run. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T RemoveExclude<T>(this T toolSettings, IEnumerable<string> exclude) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        var hashSet = new HashSet<string>(exclude);
        toolSettings.ExcludeInternal.RemoveAll(x => hashSet.Contains(x));
        return toolSettings;
    }
    #endregion
    #region Concatenate
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Concatenate"/></em></p>
    ///   <p>Concatenate the results from this run of RTF with an existing results file if one exists at the path specified. The default behavior is to replace the existing results file. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetConcatenate<T>(this T toolSettings, bool? concatenate) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Concatenate = concatenate;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="RevitTestSettings.Concatenate"/></em></p>
    ///   <p>Concatenate the results from this run of RTF with an existing results file if one exists at the path specified. The default behavior is to replace the existing results file. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ResetConcatenate<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Concatenate = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="RevitTestSettings.Concatenate"/></em></p>
    ///   <p>Concatenate the results from this run of RTF with an existing results file if one exists at the path specified. The default behavior is to replace the existing results file. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T EnableConcatenate<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Concatenate = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="RevitTestSettings.Concatenate"/></em></p>
    ///   <p>Concatenate the results from this run of RTF with an existing results file if one exists at the path specified. The default behavior is to replace the existing results file. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T DisableConcatenate<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Concatenate = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="RevitTestSettings.Concatenate"/></em></p>
    ///   <p>Concatenate the results from this run of RTF with an existing results file if one exists at the path specified. The default behavior is to replace the existing results file. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ToggleConcatenate<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Concatenate = !toolSettings.Concatenate;
        return toolSettings;
    }
    #endregion
    #region CopyAddins
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.CopyAddins"/></em></p>
    ///   <p>Specify whether to copy the addins from the Revit folder to the current working directory. Copying the addins from the Revit folder will cause the test process to simulate the typical setup on your machine. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetCopyAddins<T>(this T toolSettings, bool? copyAddins) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CopyAddins = copyAddins;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="RevitTestSettings.CopyAddins"/></em></p>
    ///   <p>Specify whether to copy the addins from the Revit folder to the current working directory. Copying the addins from the Revit folder will cause the test process to simulate the typical setup on your machine. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ResetCopyAddins<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CopyAddins = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="RevitTestSettings.CopyAddins"/></em></p>
    ///   <p>Specify whether to copy the addins from the Revit folder to the current working directory. Copying the addins from the Revit folder will cause the test process to simulate the typical setup on your machine. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T EnableCopyAddins<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CopyAddins = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="RevitTestSettings.CopyAddins"/></em></p>
    ///   <p>Specify whether to copy the addins from the Revit folder to the current working directory. Copying the addins from the Revit folder will cause the test process to simulate the typical setup on your machine. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T DisableCopyAddins<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CopyAddins = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="RevitTestSettings.CopyAddins"/></em></p>
    ///   <p>Specify whether to copy the addins from the Revit folder to the current working directory. Copying the addins from the Revit folder will cause the test process to simulate the typical setup on your machine. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ToggleCopyAddins<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CopyAddins = !toolSettings.CopyAddins;
        return toolSettings;
    }
    #endregion
    #region Continuous
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Continuous"/></em></p>
    ///   <p>Run all selected tests in one Revit session. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetContinuous<T>(this T toolSettings, bool? continuous) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Continuous = continuous;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="RevitTestSettings.Continuous"/></em></p>
    ///   <p>Run all selected tests in one Revit session. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ResetContinuous<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Continuous = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="RevitTestSettings.Continuous"/></em></p>
    ///   <p>Run all selected tests in one Revit session. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T EnableContinuous<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Continuous = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="RevitTestSettings.Continuous"/></em></p>
    ///   <p>Run all selected tests in one Revit session. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T DisableContinuous<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Continuous = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="RevitTestSettings.Continuous"/></em></p>
    ///   <p>Run all selected tests in one Revit session. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ToggleContinuous<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Continuous = !toolSettings.Continuous;
        return toolSettings;
    }
    #endregion
    #region Time
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Time"/></em></p>
    ///   <p>The time, in milliseconds, after which RTF will close the testing process automatically. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetTime<T>(this T toolSettings, int? time) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Time = time;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="RevitTestSettings.Time"/></em></p>
    ///   <p>The time, in milliseconds, after which RTF will close the testing process automatically. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ResetTime<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Time = null;
        return toolSettings;
    }
    #endregion
    #region GroupByModel
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.GroupByModel"/></em></p>
    ///   <p>Run tests with same model without reopening the model for faster execution, requires continuous. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetGroupByModel<T>(this T toolSettings, bool? groupByModel) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.GroupByModel = groupByModel;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="RevitTestSettings.GroupByModel"/></em></p>
    ///   <p>Run tests with same model without reopening the model for faster execution, requires continuous. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ResetGroupByModel<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.GroupByModel = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="RevitTestSettings.GroupByModel"/></em></p>
    ///   <p>Run tests with same model without reopening the model for faster execution, requires continuous. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T EnableGroupByModel<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.GroupByModel = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="RevitTestSettings.GroupByModel"/></em></p>
    ///   <p>Run tests with same model without reopening the model for faster execution, requires continuous. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T DisableGroupByModel<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.GroupByModel = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="RevitTestSettings.GroupByModel"/></em></p>
    ///   <p>Run tests with same model without reopening the model for faster execution, requires continuous. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ToggleGroupByModel<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.GroupByModel = !toolSettings.GroupByModel;
        return toolSettings;
    }
    #endregion
    #region Dry
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Dry"/></em></p>
    ///   <p>Conduct a dry run. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetDry<T>(this T toolSettings, bool? dry) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Dry = dry;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="RevitTestSettings.Dry"/></em></p>
    ///   <p>Conduct a dry run. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ResetDry<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Dry = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="RevitTestSettings.Dry"/></em></p>
    ///   <p>Conduct a dry run. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T EnableDry<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Dry = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="RevitTestSettings.Dry"/></em></p>
    ///   <p>Conduct a dry run. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T DisableDry<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Dry = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="RevitTestSettings.Dry"/></em></p>
    ///   <p>Conduct a dry run. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ToggleDry<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Dry = !toolSettings.Dry;
        return toolSettings;
    }
    #endregion
    #region Clean
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Clean"/></em></p>
    ///   <p>Cleanup journal files after test completion. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetClean<T>(this T toolSettings, bool? clean) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Clean = clean;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="RevitTestSettings.Clean"/></em></p>
    ///   <p>Cleanup journal files after test completion. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ResetClean<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Clean = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="RevitTestSettings.Clean"/></em></p>
    ///   <p>Cleanup journal files after test completion. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T EnableClean<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Clean = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="RevitTestSettings.Clean"/></em></p>
    ///   <p>Cleanup journal files after test completion. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T DisableClean<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Clean = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="RevitTestSettings.Clean"/></em></p>
    ///   <p>Cleanup journal files after test completion. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ToggleClean<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Clean = !toolSettings.Clean;
        return toolSettings;
    }
    #endregion
    #region Debug
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Debug"/></em></p>
    ///   <p>Should RTF attempt to attach to a debugger?. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetDebug<T>(this T toolSettings, bool? debug) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Debug = debug;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="RevitTestSettings.Debug"/></em></p>
    ///   <p>Should RTF attempt to attach to a debugger?. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ResetDebug<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Debug = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="RevitTestSettings.Debug"/></em></p>
    ///   <p>Should RTF attempt to attach to a debugger?. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T EnableDebug<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Debug = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="RevitTestSettings.Debug"/></em></p>
    ///   <p>Should RTF attempt to attach to a debugger?. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T DisableDebug<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Debug = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="RevitTestSettings.Debug"/></em></p>
    ///   <p>Should RTF attempt to attach to a debugger?. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ToggleDebug<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Debug = !toolSettings.Debug;
        return toolSettings;
    }
    #endregion
    #region Revit
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Revit"/></em></p>
    ///   <p>The Revit executable to be used for testing. If no executable is specified, RTF will use the first version of Revit that is found on the machine using the RevitAddinUtility. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetRevit<T>(this T toolSettings, string revit) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Revit = revit;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="RevitTestSettings.Revit"/></em></p>
    ///   <p>The Revit executable to be used for testing. If no executable is specified, RTF will use the first version of Revit that is found on the machine using the RevitAddinUtility. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ResetRevit<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Revit = null;
        return toolSettings;
    }
    #endregion
    #region Help
    /// <summary>
    ///   <p><em>Sets <see cref="RevitTestSettings.Help"/></em></p>
    ///   <p>Show this message and exit. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T SetHelp<T>(this T toolSettings, bool? help) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Help = help;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="RevitTestSettings.Help"/></em></p>
    ///   <p>Show this message and exit. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ResetHelp<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Help = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="RevitTestSettings.Help"/></em></p>
    ///   <p>Show this message and exit. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T EnableHelp<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Help = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="RevitTestSettings.Help"/></em></p>
    ///   <p>Show this message and exit. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T DisableHelp<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Help = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="RevitTestSettings.Help"/></em></p>
    ///   <p>Show this message and exit. (OPTIONAL)</p>
    /// </summary>
    [Pure]
    public static T ToggleHelp<T>(this T toolSettings) where T : RevitTestSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Help = !toolSettings.Help;
        return toolSettings;
    }
    #endregion
}
#endregion
