namespace RxBim.Example.IntegrationTests.Setup
{
    using Autodesk.Revit.DB;

    /// <inheritdoc />
    public class TestService : ITestService
    {
        private readonly Document _document;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="document">Revit document</param>
        public TestService(Document document)
        {
            _document = document;
        }

        /// <inheritdoc/>
        public void SetComment(Element element)
        {
            using var t = new Transaction(_document);
            t.Start("Test");
            element.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS)?.Set("trulala");
            t.Commit();
        }

        public void Throw()
        {
            throw new System.Exception();
        }
    }
}