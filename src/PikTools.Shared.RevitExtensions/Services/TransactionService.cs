namespace PikTools.Shared.RevitExtensions.Services
{
    using System;
    using Abstractions;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
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
            Func<Result> funcWithResult = () =>
            {
                action.Invoke();
                return Result.Success();
            };

            return RunInTransaction(funcWithResult, transactionName);
        }

        /// <inheritdoc/>
        public Result RunInTransactionGroup(Action action, string transactionGroupName)
        {
            Func<Result> funcWithResult = () =>
            {
                action.Invoke();
                return Result.Success();
            };

            return RunInTransactionGroup(funcWithResult, transactionGroupName);
        }

        /// <inheritdoc />
        public Result RunInTransaction(Func<Result> action, string transactionName)
        {
            if (action == null)
                return Result.Failure("Не задано действие для транзакции");

            Result result;

            using var tr = new Transaction(CurrentDocument, transactionName);
            try
            {
                tr.Start();
                result = action.Invoke();
                tr.Commit();
            }
            catch (Exception exception)
            {
                if (tr.GetStatus() != TransactionStatus.RolledBack)
                    tr.RollBack();
                return Result.Failure(exception.ToString());
            }

            return result;
        }

        /// <inheritdoc />
        public Result RunInTransactionGroup(Func<Result> action, string transactionGroupName)
        {
            if (action == null)
                return Result.Failure("Не задано действие для транзакции");

            Result result;
            using var tr = new TransactionGroup(CurrentDocument, transactionGroupName);
            try
            {
                tr.Start();
                result = action.Invoke();
                tr.Assimilate();
            }
            catch (Exception exception)
            {
                if (tr.GetStatus() != TransactionStatus.RolledBack)
                    tr.RollBack();
                return Result.Failure(exception.ToString());
            }

            return result;
        }
    }
}
