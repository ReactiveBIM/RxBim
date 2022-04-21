namespace RxBim.Transactions.Revit.IntegrationsTests
{
    using System.Linq;
    using System.Reflection;
    using Autodesk.Revit.DB;
    using Di;
    using Di.Testing.Revit;
    using NUnit.Framework;
    using RTF.Applications;
    using RTF.Framework;
    using Setup;

    [TestFixture]
    public class Tests
    {
        private IContainer _container;

        [SetUp]
        public void Setup()
        {
            var testingDiConfigurator = new TestingDiConfigurator(RevitTestExecutive.CommandData);
            testingDiConfigurator.Configure(Assembly.GetExecutingAssembly());
            _container = testingDiConfigurator.Container;
        }

        [Test]
        [TestModel("./model.rvt")]
        public void CommentShouldBeSetAndNotThrowsException()
        {
            var testService = _container.GetService<ITestService>();
            var element = new FilteredElementCollector(_container.GetService<Document>())
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