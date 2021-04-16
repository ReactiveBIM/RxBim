namespace PikTools.Shared.RevitExtensions.Abstractions
{
    using System;
    using CSharpFunctionalExtensions;

    /// <summary>
    /// Сервис по работе с транзакцией
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Оборачивает действие в транзакцию
        /// </summary>
        /// <param name="action">Действие</param>
        /// <param name="transactionName">Название транзакции</param>
        Result RunInTransaction(Action action, string transactionName);

        /// <summary>
        /// Оборачивает действие в группу транзакций
        /// </summary>
        /// <param name="action">Действие</param>
        /// <param name="transactionGroupName">Название группы транзакций</param>
        Result RunInTransactionGroup(Action action, string transactionGroupName);
    }
}
