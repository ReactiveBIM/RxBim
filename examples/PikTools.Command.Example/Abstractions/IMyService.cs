namespace PikTools.CommandExample.Abstractions
{
    using System.Threading.Tasks;
    using CSharpFunctionalExtensions;

    /// <summary>
    /// my service
    /// </summary>
    public interface IMyService
    {
        /// <summary>
        /// go
        /// </summary>
        Task<Result> Go();
    }
}