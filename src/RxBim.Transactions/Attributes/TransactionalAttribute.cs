namespace RxBim.Transactions.Attributes
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Indicates that a method is transactional.
    /// <para>
    /// Methods decorated with this attribute will be executed in a transaction.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [UsedImplicitly]
    public class TransactionalAttribute : Attribute
    {
        /// <summary>
        /// The name of the transaction.
        /// </summary>
        [UsedImplicitly]
        public string TransactionName { get; set; }
    }
}