using System;
using System.IO;
using System.Threading.Tasks;

namespace FredrikHr.Tower3dPro.Extensions.Records
{
    public static partial class Tower3dRecordParser
    {
        public static Task<ReadOnlyMemory<Tower3dScheduleRecord>> ParseScheduleRecords(TextReader textReader) =>
            ParseRecordsAsync<Tower3dScheduleRecord>(textReader, TryParseScheduleRecord);

        private static bool TryParseScheduleRecord(ReadOnlySpan<char> span, out Tower3dScheduleRecord record)
        {
            const int fieldCount = 9;
            Tower3dScheduleRecord ScheduleRecordFromFields(string[] fields)
            {
                _ = TimeSpan.TryParse(fields[5], invariant, out TimeSpan timeArrival);
                _ = TimeSpan.TryParse(fields[6], invariant, out TimeSpan timeDeparture);
                var durationTime = int.TryParse(fields[7], integerNumberStyle, invariant, out int durationHours)
                    ? TimeSpan.FromHours(durationHours) : default;
                return new Tower3dScheduleRecord
                {
                    OriginAirportIata = fields[0],
                    DestinationAirportIata = fields[1],
                    AirplaneTypeIata = fields[2],
                    AirlineIata = fields[3],
                    FlightNumber = fields[4],
                    ArrivalTime = timeArrival,
                    DepartureTime = timeDeparture,
                    Duration = durationTime,
                    CodeshareIata = fields[8],
                };
            }
            bool ValidateScheduleRecord(in Tower3dScheduleRecord testRecord)
            {
                if (string.IsNullOrWhiteSpace(testRecord.AirlineIata) ||
                    string.IsNullOrWhiteSpace(testRecord.FlightNumber))
                    return false;
                return true;
            }

            return TryParseDelimitedRecord(span, fieldCount, ParseCommaDelimitedFields, ScheduleRecordFromFields, ValidateScheduleRecord, out record);
        }
    }
}
