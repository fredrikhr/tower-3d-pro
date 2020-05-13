using System;
using System.Diagnostics.CodeAnalysis;

namespace FredrikHr.Tower3dPro.Extensions.Records
{
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types")]
    public struct Tower3dTerminalRecord
    {
        public string Name { get; set; }
        public ReadOnlyMemory<string> AirlineIcaoIdentifiers { get; set; }
    }
}
