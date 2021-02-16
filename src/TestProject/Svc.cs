namespace TestProject
{
    using Autodesk.Revit.DB;
    using CSharpFunctionalExtensions;
    using PikTools.Shared.FmHelpers.Abstractions;

    /// <inheritdoc />
    public class Svc : ISvc
    {
        private readonly Document _document;
        private readonly IFamilyManagerService _familyManagerService;

        /// <inheritdoc />
        public Svc(Document document, IFamilyManagerService familyManagerService)
        {
            _document = document;
            _familyManagerService = familyManagerService;
        }

        /// <inheritdoc />
        public Result Run()
        {
            throw new System.NotImplementedException();
        }
    }
}