namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    using Newtonsoft.Json;
    using Services;

    /// <summary>
    /// Element of ribbon panel
    /// </summary>
    [JsonConverter(typeof(JsonPanelElementConverter))]
    public interface IRibbonPanelElement
    {
    }
}