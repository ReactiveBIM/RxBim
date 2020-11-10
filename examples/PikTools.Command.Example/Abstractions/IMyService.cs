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

        /// <summary>
        /// Загрузка семейства
        /// </summary>
        /// <param name="familyName">Название семейства</param>
        Result LoadFamily(string familyName);
    }
}