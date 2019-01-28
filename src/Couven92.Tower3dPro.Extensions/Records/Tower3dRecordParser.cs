using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Couven92.Tower3dPro.Extensions.Records
{
    public static partial class Tower3dRecordParser
    {
        private delegate bool TryParseRecord<T>(ReadOnlySpan<char> span, out T record);
        private delegate T RecordFieldParser<T>(ReadOnlySpan<char> span, int fieldCount, Func<string[], T> fieldsToRecord);
        private delegate bool RecordValidator<T>(in T record);

        private const System.Globalization.NumberStyles integerNumberStyle = System.Globalization.NumberStyles.Integer;
        private const System.Globalization.NumberStyles floatNumberStyle = System.Globalization.NumberStyles.Float;
        private static readonly System.Globalization.CultureInfo invariant = System.Globalization.CultureInfo.InvariantCulture;

        private static readonly ArrayPool<string> stringPool = ArrayPool<string>.Shared;

        private static async Task<ReadOnlyMemory<T>> ParseRecordsAsync<T>(TextReader textReader, TryParseRecord<T> tryParseRecord)
        {
            var recordList = new List<T>();
            Task<string> nextRead;
            for (var currentRead = textReader.ReadLineAsync(); true; currentRead = nextRead)
            {
                var line = await currentRead.ConfigureAwait(false);
                if (line is null)
                    break;
                nextRead = textReader.ReadLineAsync();

                if (tryParseRecord(line.AsSpan(), out T record))
                    recordList.Add(record);
            }

            return recordList.ToArray().AsMemory();
        }

        private static bool TryParseDelimitedRecord<T>(ReadOnlySpan<char> span, int fieldCount, RecordFieldParser<T> fieldParser,
            Func<string[], T> fieldsToRecord, RecordValidator<T> validator, out T record)
        {
            span = span.Trim();
            if (span.IsEmpty || span.StartsWith("//".AsSpan()))
                goto noAirlineRecord;
            record = fieldParser(span, fieldCount, fieldsToRecord);
            return validator?.Invoke(record) ?? true;

            noAirlineRecord:
            record = default;
            return false;
        }

        private static T ParseCommaDelimitedFields<T>(ReadOnlySpan<char> span, int fieldCount, Func<string[], T> fieldsToRecord) =>
            ParseDelimitedFields(span, ',', fieldCount, fieldsToRecord);

        private static T ParseColonDelimitedFields<T>(ReadOnlySpan<char> span, int fieldCount, Func<string[], T> fieldsToRecord) =>
            ParseDelimitedFields(span, ':', fieldCount, fieldsToRecord);

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        private static T ParseDelimitedFields<T>(ReadOnlySpan<char> span, char delimiter, int fieldCount, Func<string[], T> fieldsToRecord)
        {
            var fields = stringPool.Rent(fieldCount);
            try
            {
                var remaining = span;
                int delimIdx, fieldIdx;
                for (fieldIdx = 0; fieldIdx < fieldCount; fieldIdx++)
                {
                    delimIdx = remaining.IndexOf(delimiter);
                    ReadOnlySpan<char> fieldSpan;
                    if (delimIdx < 0)
                        fieldSpan = remaining;
                    else
                        fieldSpan = remaining.Slice(start: 0, length: delimIdx);
                    fields[fieldIdx] = fieldSpan.Trim().ToString();
                    remaining = remaining.Slice(delimIdx + 1);
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
    }
}
