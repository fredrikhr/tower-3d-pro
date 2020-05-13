using System;
using System.IO;
using System.Threading.Tasks;

using THNETII.TypeConverter;

namespace FredrikHr.Tower3dPro.Extensions.Records
{
    public static partial class Tower3dRecordParser
    {
        public static Task<ReadOnlyMemory<Tower3dGaLocalTrafficRecord>> ParseGaLocalTrafficRecords(TextReader textReader) =>
            ParseRecordsAsync<Tower3dGaLocalTrafficRecord>(textReader, TryParseGaLocalTrafficRecord);

        private static bool TryParseGaLocalTrafficRecord(ReadOnlySpan<char> span, out Tower3dGaLocalTrafficRecord record)
        {
            const int fieldCount = 10;
            Tower3dGaLocalTrafficRecord GaLocalTrafficRecordFromFields(string[] fields)
            {
                _ = TimeSpan.TryParse(fields[2], invariant, out TimeSpan time);
                _ = int.TryParse(fields[5], integerNumberStyle, invariant, out int stopngo);
                _ = int.TryParse(fields[6], integerNumberStyle, invariant, out int touchngo);
                bool lowApproach = BooleanStringConverter.ParseOrDefault(fields[7]);
                return new Tower3dGaLocalTrafficRecord
                {
                    OriginAirportIcao = fields[0],
                    DestinationAirportIcao = fields[1],
                    Time = time,
                    AirplaneTypeIata = fields[3],
                    IcaoIdentifier = fields[4],
                    StopAndGo = stopngo,
                    TouchAndGo = touchngo,
                    LowApproach = lowApproach,
                    ShortIdentifier = fields[8],
                    Callsign = fields[9],
                };
            }
            bool ValidateGaLocalTrafficRecord(in Tower3dGaLocalTrafficRecord testRecord)
            {
                if (string.IsNullOrWhiteSpace(testRecord.IcaoIdentifier))
                    return false;
                return true;
            }

            return TryParseDelimitedRecord(span, fieldCount, ParseCommaDelimitedFields, GaLocalTrafficRecordFromFields, ValidateGaLocalTrafficRecord, out record);
        }
    }
}
