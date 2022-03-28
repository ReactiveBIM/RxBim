namespace RxBim.Transactions.IntegrationsTests.Setup
{
    using Attributes;
    using Autodesk.Revit.DB;

    /// <inheritdoc />
    public class TestService : ITestService
    {
        /// <inheritdoc/>
        [Transactional(TransactionName = "Test")]
        public void SetComment(Element element)
        {
            element.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)?.Set("trulala");
        }
    }
}