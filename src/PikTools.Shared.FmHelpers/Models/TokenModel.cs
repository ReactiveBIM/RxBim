namespace PikTools.Shared.FmHelpers.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Модель токена безопасности.
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// Тип токена
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Токен доступа.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
