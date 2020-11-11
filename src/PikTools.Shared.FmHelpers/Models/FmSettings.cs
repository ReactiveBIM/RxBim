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
        /// Версия клиента FM
        /// </summary>
        public string ClientVersion { get; set; }
            = "2.3";

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public string ClientId { get; set; }
            = "fm_app";

        /// <summary>
        /// Клиентский ключ
        /// </summary>
        public string ClientSecret { get; set; }
            = "xyQBKp7YEkZ2tmu7";

        /// <summary>
        /// Адрес авторизации
        /// </summary>
        public string AuthorityUri { get; set; }
            = "https://auth.pik.ru";

        /// <summary>
        /// Область видимости
        /// </summary>
        public string Scope { get; set; }
            = "openid email offline_access fm_api profile";
    }
}
