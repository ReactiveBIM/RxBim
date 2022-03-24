namespace RxBim.Transactions.Attributes
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Indicates that a method marked by this attribute should be wrapped by transaction 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [UsedImplicitly]
    public class TransactionalAttribute : Attribute
    {
        /// <summary>
        /// Name of transaction
        /// </summary>
        [UsedImplicitly]
        public string TransactionName { get; set; }
    }
}