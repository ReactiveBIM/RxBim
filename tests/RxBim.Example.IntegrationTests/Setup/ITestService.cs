namespace RxBim.Example.IntegrationTests.Setup
{
    /// <summary>
    /// Test service
    /// </summary>
    public interface ITestService
    {
        /// <summary>
        /// Sets comment to element
        /// </summary>
        /// <param name="element">Revit element</param>
        void SetComment(Autodesk.Revit.DB.Element element);

        void Throw();
    }
}