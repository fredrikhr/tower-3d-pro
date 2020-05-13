using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

using THNETII.TypeConverter.Serialization;

namespace FredrikHr.Tower3dPro.Extensions.Records
{
    public static partial class Tower3dRecordParser
    {
        public static Task<ReadOnlyMemory<Tower3dAirportRecord>> ParseAirportRecords(TextReader textReader) =>
            ParseRecordsAsync<Tower3dAirportRecord>(textReader, TryParseAirportRecord);

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        private static bool TryParseAirportRecord(ReadOnlySpan<char> span, out Tower3dAirportRecord record)
        {
            const int fieldCount = 7;
            Tower3dAirportRecord AirportRecordFromFields(string[] fields)
            {
                var lat = double.TryParse(fields[2], floatNumberStyle, invariant, out double latCoords)
                    ? new LatitudeCoordinates { Coordinates = latCoords }
                    : default;
                lat.NorthSouth = EnumMemberStringConverter.ParseOrDefault<LatitudeCoordinates.Modifier>(fields[3]);
                var lon = double.TryParse(fields[4], floatNumberStyle, invariant, out double lonCoords)
                    ? new LongitudeCoordinates { Coordinates = lonCoords }
                    : default;
                lon.EastWest = EnumMemberStringConverter.ParseOrDefault<LongitudeCoordinates.Modifier>(fields[5]);
                return new Tower3dAirportRecord
                {
                    IataIdentifier = fields[0],
                    Text = fields[1],
                    Latitude = lat,
                    Longitude = lon,
                    IcaoIdentifier = fields[6],
                };
            }
            bool ValidateAirportRecord(in Tower3dAirportRecord testRecord)
            {
                if (string.IsNullOrWhiteSpace(testRecord.IcaoIdentifier) ||
                    string.IsNullOrWhiteSpace(testRecord.IataIdentifier))
                    return false;
                return true;
            }
            Tower3dAirportRecord ParseAirportRecord(ReadOnlySpan<char> parseSpan, int fCnt, Func<string[], Tower3dAirportRecord> fieldsToRecord)
            {
                var fields = stringPool.Rent(fieldCount);
                try
                {
                    var remaining = parseSpan;
                    ReadOnlySpan<char> fieldSpan;
                    int delimIdx, fieldIdx;
                    for (fieldIdx = 0; fieldIdx < fieldCount; fieldIdx++, remaining = remaining.Slice(delimIdx + 1))
                    {
                        delimIdx = remaining.IndexOf(' ');
                        if (delimIdx < 0)
                            fieldSpan = remaining.Trim();
                        else
                            fieldSpan = remaining.Slice(start: 0, length: delimIdx).Trim();
                        if (fieldSpan.IsEmpty)
                        {
                            fieldIdx--;
                            continue;
                        }
                        fields[fieldIdx] = fieldSpan.ToString();
                    }
                    for (; fieldIdx < fieldCount; fieldIdx++)
                        fields[fieldIdx] = default;
                    return fieldsToRecord(fields);
                }
                finally
                {
                    stringPool.Return(fields);
                }
            }

            return TryParseDelimitedRecord(span, fieldCount, ParseAirportRecord, AirportRecordFromFields, ValidateAirportRecord, out record);
        }
    }
}
