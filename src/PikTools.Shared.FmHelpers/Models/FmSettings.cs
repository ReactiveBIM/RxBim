namespace PikTools.Shared.FmHelpers.Models
{
    /// <summary>
    /// Парметры работы с FM
    /// </summary>
    public class FmSettings
    {
        /// <summary>
        /// Family Manager end point
        /// </summary>
        public string FmEndPoint { get; set; }
            = "https://fm-api.bimteam.ru/";

        /// <summary>
        /// Путь к кэшу авторизации
        /// </summary>
        public string AuthCachePath { get; set; }
            = @"\PIK\Auth\cache.json";

        /// <summary>
        /// Версия клиента FM
        /// </summary>
        public string ClientVersion { get; set; }
            = "2.3";
    }
}
