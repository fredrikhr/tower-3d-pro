using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Couven92.Tower3dPro.Extensions.Records
{
    public static partial class Tower3dRecordParser
    {
        public static Task<ReadOnlyMemory<Tower3dAirplaneRecord>> ParseAirplaneRecords(TextReader textReader) =>
            ParseRecordsAsync<Tower3dAirplaneRecord>(textReader, TryParseAirplaneRecord);

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        private static bool TryParseAirplaneRecord(ReadOnlySpan<char> span, out Tower3dAirplaneRecord record)
        {
            const int fieldCount = 4;
            Tower3dAirplaneRecord AirplaneRecordFromFields(string[] fields) => new Tower3dAirplaneRecord
            {
                AssetFilePrefix = fields[0],
                IataIdentifier = fields[1],
                Name = fields[2],
                Type = fields[3]
            };
            bool ValidateAirplaneRecord(in Tower3dAirplaneRecord testRecord)
            {
                if (string.IsNullOrWhiteSpace(testRecord.IataIdentifier))
                    return false;
                return true;
            }
            Tower3dAirplaneRecord ParseAirplaneRecord(ReadOnlySpan<char> parseSpan, int fCnt, Func<string[], Tower3dAirplaneRecord> fieldsToRecord)
            {
                var fields = stringPool.Rent(fCnt);
                try
                {
                    var remaining = parseSpan;
                    ReadOnlySpan<char> fieldSpan;
                    int fieldIdx = 0, delimIdx;

                    // File name prefix
                    delimIdx = remaining.IndexOf('-');
                    if (delimIdx < 0)
                        goto delimNotFound;
                    fieldSpan = remaining.Slice(start: 0, length: delimIdx).Trim();
                    remaining = remaining.Slice(delimIdx + 1);
                    fields[fieldIdx] = fieldSpan.ToString();
                    fieldIdx++;

                    // IATA
                    delimIdx = remaining.IndexOf('-');
                    if (delimIdx < 0)
                        goto delimNotFound;
                    fieldSpan = remaining.Slice(start: 0, length: delimIdx).Trim();
                    remaining = remaining.Slice(delimIdx + 1);
                    fields[fieldIdx] = fieldSpan.ToString();
                    fieldIdx++;

                    // Name
                    delimIdx = remaining.LastIndexOf('-');
                    if (delimIdx < 0)
                        goto delimNotFound;
                    fieldSpan = remaining.Slice(start: 0, length: delimIdx).Trim();
                    remaining = remaining.Slice(delimIdx + 1);
                    fields[fieldIdx] = fieldSpan.ToString();
                    fieldIdx++;

                    // Type
                    fieldSpan = remaining.Trim();
                    fields[fieldIdx] = fieldSpan.ToString();
                    fieldIdx++;

                    delimNotFound:
                    for (; fieldIdx < fCnt; fieldIdx++)
                        fields[fieldIdx] = default;
                    return fieldsToRecord(fields);
                }
                finally
                {
                    stringPool.Return(fields);
                }
            }

            return TryParseDelimitedRecord(span, fieldCount, ParseAirplaneRecord, AirplaneRecordFromFields, ValidateAirplaneRecord, out record);
        }
    }
}
