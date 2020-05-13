using System.Diagnostics.CodeAnalysis;

namespace FredrikHr.Tower3dPro.Extensions.Records
{
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types")]
    public struct Tower3dAirportRecord
    {
        public string IataIdentifier { get; set; }
        public string Text { get; set; }
        public LatitudeCoordinates Latitude { get; set; }
        public LongitudeCoordinates Longitude { get; set; }
        public string IcaoIdentifier { get; set; }
    }
}
