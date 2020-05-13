using System;
using System.IO;
using System.Threading.Tasks;

namespace FredrikHr.Tower3dPro.Extensions.Records
{
    public static partial class Tower3dRecordParser
    {
        public static Task<ReadOnlyMemory<Tower3dAirlineRecord>> ParseAirlineRecords(TextReader textReader) =>
            ParseRecordsAsync<Tower3dAirlineRecord>(textReader, TryParseAirlineRecord);

        private static bool TryParseAirlineRecord(ReadOnlySpan<char> span, out Tower3dAirlineRecord record)
        {
            const int fieldCount = 5;
            Tower3dAirlineRecord AirlineRecordFromFields(string[] fields) => new Tower3dAirlineRecord
            {
                IcaoIdentifier = fields[0],
                IataIdentifier = fields[1],
                Callsign = fields[2],
                Name = fields[3],
                Country = fields[4]
            };
            bool ValidateAirlineRecord(in Tower3dAirlineRecord testRecord)
            {
                if (string.IsNullOrWhiteSpace(testRecord.IcaoIdentifier) ||
                    string.IsNullOrWhiteSpace(testRecord.IataIdentifier))
                    return false;
                return true;
            }

            return TryParseDelimitedRecord(span, fieldCount, ParseCommaDelimitedFields, AirlineRecordFromFields, ValidateAirlineRecord, out record);
        }
    }
}
