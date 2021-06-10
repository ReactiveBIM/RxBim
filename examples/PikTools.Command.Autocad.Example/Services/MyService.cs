namespace PikTools.Command.Autocad.Example.Services
{
    using Abstractions;
    using CSharpFunctionalExtensions;
    using Shared.Ui.Abstractions;

    /// <inheritdoc/>
    public class MyService : IMyService
    {
        private readonly INotificationService _notificationService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="notificationService">notification</param>
        public MyService(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <inheritdoc/>
        public Result Go()
        {
            return Result.Success();
        }
    }
}