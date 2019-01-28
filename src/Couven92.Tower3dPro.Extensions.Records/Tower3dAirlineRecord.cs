using System.Diagnostics.CodeAnalysis;

namespace Couven92.Tower3dPro.Extensions.Records
{
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types")]
    public struct Tower3dAirlineRecord
    {
        public string IcaoIdentifier { get; set; }
        public string IataIdentifier { get; set; }
        public string Callsign { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
