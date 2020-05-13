using System;
using System.Diagnostics.CodeAnalysis;

namespace FredrikHr.Tower3dPro.Extensions.Records
{
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types")]
    public struct Tower3dGaLocalTrafficRecord
    {
        public string OriginAirportIcao { get; set; }
        public string DestinationAirportIcao { get; set; }
        public TimeSpan Time { get; set; }
        public string AirplaneTypeIata { get; set; }
        public string IcaoIdentifier { get; set; }
        public int StopAndGo { get; set; }
        public int TouchAndGo { get; set; }
        public bool LowApproach { get; set; }
        public string ShortIdentifier { get; set; }
        public string Callsign { get; set; }
    }
}
