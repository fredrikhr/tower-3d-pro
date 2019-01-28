using System;
using System.Diagnostics.CodeAnalysis;

namespace Couven92.Tower3dPro.Extensions.Records
{
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types")]
    public struct Tower3dScheduleRecord
    {
        public string OriginAirportIata { get; set; }
        public string DestinationAirportIata { get; set; }
        public string AirplaneTypeIata { get; set; }
        public string AirlineIata { get; set; }
        public string FlightNumber { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string CodeshareIata { get; set; }
    }
}
