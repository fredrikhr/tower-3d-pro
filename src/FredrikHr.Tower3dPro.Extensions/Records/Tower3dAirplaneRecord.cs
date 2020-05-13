using System.Diagnostics.CodeAnalysis;

namespace FredrikHr.Tower3dPro.Extensions.Records
{
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types")]
    public struct Tower3dAirplaneRecord
    {
        public string AssetFilePrefix { get; set; }
        public string IataIdentifier { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
