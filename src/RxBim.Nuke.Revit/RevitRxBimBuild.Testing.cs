namespace RxBim.Nuke.Revit
{
    using System;
    using System.Linq;
    using Extensions;
    using global::Nuke.Common;
    using static RevitTestTasks;

    public partial class RevitRxBimBuild
    {
        /// <summary>
        /// Starts integration tests in selected project
        /// </summary>
        public Target IntegrationTests => _ => _
            .Requires(() => Project)
            .Executes(() =>
            {
                var project = Solution.AllProjects.FirstOrDefault(x => x.Name == Project) ??
                              throw new ArgumentException($"Project {Project} not found");

                var workingDirectory = project.BuildProject(Configuration);

                RevitTest(settings => settings
                    .SetDir(workingDirectory)
                    .SetResults(workingDirectory / "result.xml")
                    .SetAssembly(workingDirectory / $"{project.Name}.dll"));
            });
    }
}