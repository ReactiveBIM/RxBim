namespace PikTools.Shared.RevitExtensions.Services
{
    using System;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using PikTools.Shared.RevitExtensions.Abstractions;
    using Result = CSharpFunctionalExtensions.Result;

    /// <inheritdoc/>
    public class TransactionService : ITransactionService
    {
        private readonly UIApplication _uiApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionService"/> class.
        /// </summary>
        /// <param name="uiApplication"><see cref="UIApplication"/></param>
        public TransactionService(UIApplication uiApplication)
        {
            _uiApplication = uiApplication;
        }

        private Document CurrentDocument => _uiApplication.ActiveUIDocument.Document;

        /// <inheritdoc/>
        public Result RunInTransaction(Action action, string transactionName)
        {
            if (action == null)
                return Result.Failure("Не задано действие для транзакции");

            using var tr = new Transaction(CurrentDocument, transactionName);
            try
            {
                tr.Start();
                action.Invoke();
                tr.Commit();
            }
            catch (Exception exeption)
            {
                if (tr.GetStatus() != TransactionStatus.RolledBack)
                    tr.RollBack();
                return Result.Failure(exeption.ToString());
            }

            return Result.Success();
        }

        /// <inheritdoc/>
        public Result RunInTransactionGroup(Action action, string transactionGroupName)
        {
            if (action == null)
                return Result.Failure("Не задано действие для транзакции");

            using var tr = new TransactionGroup(CurrentDocument, transactionGroupName);
            try
            {
                tr.Start();
                action.Invoke();
                tr.Assimilate();
            }
            catch (Exception exeption)
            {
                if (tr.GetStatus() != TransactionStatus.RolledBack)
                    tr.RollBack();
                return Result.Failure(exeption.ToString());
            }

            return Result.Success();
        }
    }
}
