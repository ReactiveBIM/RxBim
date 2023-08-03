namespace RxBim.Example.Revit.IntegrationTests
{
    using System.Linq;
    using System.Reflection;
    using Autodesk.Revit.DB;
    using Di.Testing.Revit;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using RTF.Applications;
    using RTF.Framework;
    using Setup;

    [TestFixture]
    public class Tests1
    {
        private IServiceCollection _services = null!;

        [SetUp]
        public void Setup()
        {
            var testingDiConfigurator = new TestingDiConfigurator(RevitTestExecutive.CommandData);
            testingDiConfigurator.Configure(
                Assembly
                    .GetExecutingAssembly()); // if you are using mocked configuration from current tests assembly.
            // You should understand that you must add all dependencies
            // in the test configuration

            /*
             or use:
             testingDiConfigurator.Configure(typeof(ITestService).Assembly);
             if are using real configuration from the application.
             */

            _services = testingDiConfigurator.Container.Services;
        }

        [Test]
        [TestModel("./model.rvt")]
        public void CommentShouldBeSetAndNotThrowsException()
        {
            using var provider = _services.BuildServiceProvider(false);
            var testService = provider.GetRequiredService<ITestService>();
            var element = new FilteredElementCollector(provider.GetRequiredService<Document>())
                .WhereElementIsNotElementType()
                .OfClass(typeof(Wall))
                .FirstOrDefault();
            Assert.NotNull(element, "element != null");
            Assert.DoesNotThrow(() => testService.SetComment(element));

            var comment = element.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS)
                .AsValueString();
            Assert.NotNull(comment, "comment != null");
            Assert.IsNotEmpty(comment);
        }

        [Test]
        [TestModel("./model.rvt")]
        public void AlwaysSuccess()
        {
            Assert.Pass();
        }
    }
}