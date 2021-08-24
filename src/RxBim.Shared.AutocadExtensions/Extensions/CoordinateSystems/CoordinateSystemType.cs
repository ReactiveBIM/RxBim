namespace RxBim.Shared.AutocadExtensions.Extensions.CoordinateSystems
{
    /// <summary>
    /// AutoCAD coordinate systems enumeration.
    /// </summary>
    public enum CoordinateSystemType
    {
        /// <summary>
        /// World Coordinate System.
        /// </summary>
        WCS = 0,

        /// <summary>
        /// Current User Coordinate System.
        /// </summary>
        UCS,

        /// <summary>
        /// Display Coordinate System of the current viewport.
        /// </summary>
        DCS,

        /// <summary>
        /// Paper Space Display Coordinate System.
        /// </summary>
        PSDCS
    }
}