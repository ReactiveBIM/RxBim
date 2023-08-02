namespace RxBim.Transactions.Revit.IntegrationsTests
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
    public class Tests
    {
        private IServiceCollection _services = null!;

        [SetUp]
        public void Setup()
        {
            var testingDiConfigurator = new TestingDiConfigurator(RevitTestExecutive.CommandData);
            testingDiConfigurator.Configure(Assembly.GetExecutingAssembly());
            _services = testingDiConfigurator.Services;
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

            var comment = element.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
                .AsString();
            Assert.NotNull(comment, "comment != null");
            Assert.IsNotEmpty(comment);
        }
    }
}