using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace FredrikHr.Tower3dPro.Extensions.Records
{
    public static partial class Tower3dRecordParser
    {
        public static Task<ReadOnlyMemory<Tower3dTerminalRecord>> ParseTerminalRecords(TextReader textReader) =>
            ParseRecordsAsync<Tower3dTerminalRecord>(textReader, TryParseTerminalRecord);

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        private static bool TryParseTerminalRecord(ReadOnlySpan<char> span, out Tower3dTerminalRecord record)
        {
            const int fieldCount = 2;
            Tower3dTerminalRecord TerminalRecordFromFields(string[] fields)
            {
                var airlinesString = fields[1];
                var airlineIataCodes = new List<string>((airlinesString.Length / 4) + 1);
                var remaining = airlinesString.AsSpan();
                int commaIdx;
                for (commaIdx = remaining.IndexOf(','); commaIdx >= 0;
                    remaining = remaining.Slice(commaIdx + 1),
                    commaIdx = remaining.IndexOf(','))
                {
                    var codeSpan = remaining.Slice(start: 0, length: commaIdx)
                        .Trim();
                    if (!codeSpan.IsEmpty)
                        airlineIataCodes.Add(codeSpan.ToString());
                }
                remaining = remaining.Trim();
                if (!remaining.IsEmpty)
                    airlineIataCodes.Add(remaining.ToString());

                return new Tower3dTerminalRecord
                {
                    Name = fields[0],
                    AirlineIcaoIdentifiers = airlineIataCodes.ToArray().AsMemory()
                };
            }
            bool ValidateTerminalRecord(in Tower3dTerminalRecord testRecord)
            {
                if (string.IsNullOrWhiteSpace(testRecord.Name))
                    return false;
                return true;
            }

            return TryParseDelimitedRecord(span, fieldCount, ParseColonDelimitedFields, TerminalRecordFromFields, ValidateTerminalRecord, out record);
        }
    }
}
