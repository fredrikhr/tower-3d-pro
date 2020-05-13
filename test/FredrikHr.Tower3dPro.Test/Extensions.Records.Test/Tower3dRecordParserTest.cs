using System;
using System.IO;
using Xunit;

namespace FredrikHr.Tower3dPro.Extensions.Records.Test
{
    public static class Tower3dRecordParserTest
    {
        [Fact]
        public static void CanParseAirlines()
        {
            const string sampleString = @"//ICAO, IATA, call sign, airline, country

ACA, AC, AIR CANADA, Air Canada, Canada
AMX, AM, AEROMEXICO, AeroMéxico, Mexico
VOZ, VA, VELOCITY, Virgin Australia, Australia
CCA, CA, AIR CHINA, Air China, China
AFR, AF, AIR FRANCE, Air France, France
ANZ, NZ, NEW ZEALAND, Air New Zealand, New Zealand
AZA, AZ, ALITALIA, Alitalia, Italy
CHP, 6A, AVIACSA, Consorcio Aviaxsa, Mexico
";
            using (var reader = new StringReader(sampleString))
            {
                var parsedMemory = Tower3dRecordParser.ParseAirlineRecords(reader)
                    .GetAwaiter().GetResult();
                var parsedSpan = parsedMemory.Span;
                Span<Tower3dAirlineRecord> expectedRecords = new[]
                {
                    new Tower3dAirlineRecord{IataIdentifier="AC", IcaoIdentifier="ACA"},
                    new Tower3dAirlineRecord{IataIdentifier="AM", IcaoIdentifier="AMX"},
                    new Tower3dAirlineRecord{IataIdentifier="VA", IcaoIdentifier="VOZ"},
                    new Tower3dAirlineRecord{IataIdentifier="CA", IcaoIdentifier="CCA"},
                    new Tower3dAirlineRecord{IataIdentifier="AF", IcaoIdentifier="AFR"},
                    new Tower3dAirlineRecord{IataIdentifier="NZ", IcaoIdentifier="ANZ"},
                    new Tower3dAirlineRecord{IataIdentifier="AZ", IcaoIdentifier="AZA"},
                    new Tower3dAirlineRecord{IataIdentifier="6A", IcaoIdentifier="CHP"},
                };
                Assert.Equal(expectedRecords.Length, parsedSpan.Length);
                for (int i = 0; i < parsedSpan.Length; i++)
                {
                    ref readonly Tower3dAirlineRecord expected = ref expectedRecords[i];
                    ref readonly Tower3dAirlineRecord actual = ref parsedSpan[i];
                    Assert.Equal(expected.IcaoIdentifier, actual.IcaoIdentifier, ignoreCase: true);
                    Assert.Equal(expected.IataIdentifier, actual.IataIdentifier, ignoreCase: true);
                }
            }
        }

        [Fact]
        public static void CanParseAirplanes()
        {
            const string sampleString = @"A30B-AB3-AIRBUS INDUSTRIE A300                    -WIDE BODY JET
A30B-AB4-AIRBUS INDUSTRIE A300B2/B4               -WIDE BODY JET
A306-AB6-AIRBUS INDUSTRIE A300-600                -WIDE BODY JET
AN24-AN4-ANTONOV AN-24                            -TURBOPROP
AN6_-AN6-ANTONOV AN-26/30/32                      -TURBOPROP
ARJ_-ARJ-AVRO RJ70/RJ85/RJ100                     -REGIONAL JET
RJ1H-AR1-AVRO RJ100                               -REGIONAL JET
RJ85-AR8-AVRO RJ85                                -REGIONAL JET
ATP_-ATP-BRITISH AEROSPACE ATP                    -TURBOPROP
ATR_-ATR-ATR 42 / ATR 72                          -TURBOPROP
";
            using (var reader = new StringReader(sampleString))
            {
                var parsedMemory = Tower3dRecordParser.ParseAirplaneRecords(reader)
                    .GetAwaiter().GetResult();
                var parsedSpan = parsedMemory.Span;
                Span<Tower3dAirplaneRecord> expectedRecords = new[]
                {
                    new Tower3dAirplaneRecord{IataIdentifier="AB3", AssetFilePrefix="A30B", Type="WIDE BODY JET"},
                    new Tower3dAirplaneRecord{IataIdentifier="AB4", AssetFilePrefix="A30B", Type="WIDE BODY JET"},
                    new Tower3dAirplaneRecord{IataIdentifier="AB6", AssetFilePrefix="A306", Type="WIDE BODY JET"},
                    new Tower3dAirplaneRecord{IataIdentifier="AN4", AssetFilePrefix="AN24", Type="TURBOPROP"},
                    new Tower3dAirplaneRecord{IataIdentifier="AN6", AssetFilePrefix="AN6_", Type="TURBOPROP"},
                    new Tower3dAirplaneRecord{IataIdentifier="ARJ", AssetFilePrefix="ARJ_", Type="REGIONAL JET"},
                    new Tower3dAirplaneRecord{IataIdentifier="AR1", AssetFilePrefix="RJ1H", Type="REGIONAL JET"},
                    new Tower3dAirplaneRecord{IataIdentifier="AR8", AssetFilePrefix="RJ85", Type="REGIONAL JET"},
                    new Tower3dAirplaneRecord{IataIdentifier="ATP", AssetFilePrefix="ATP_", Type="TURBOPROP"},
                    new Tower3dAirplaneRecord{IataIdentifier="ATR", AssetFilePrefix="ATR_", Type="TURBOPROP"},
                };
                Assert.Equal(expectedRecords.Length, parsedSpan.Length);
                for (int i = 0; i < parsedSpan.Length; i++)
                {
                    ref readonly Tower3dAirplaneRecord expected = ref expectedRecords[i];
                    ref readonly Tower3dAirplaneRecord actual = ref parsedSpan[i];
                    Assert.Equal(expected.IataIdentifier, actual.IataIdentifier, ignoreCase: true);
                    Assert.Equal(expected.AssetFilePrefix, actual.AssetFilePrefix, ignoreCase: true);
                    Assert.Equal(expected.Type, actual.Type, ignoreCase: true);
                }
            }
        }

        [Fact]
        public static void CanParseAirports()
        {
            const string sampleString = @"  AAA ANAA_FRENCH_POLYNESIA                     17.25 S 145.30 W NTGA
  AAE ANNABA_RABAH_BITAT                        36.50 N   8.00 E DABB
  AAL AALBORG_DENMARK                           57.06 N   9.51 E EKYT
  AAR AARHUS_DENMARK                            56.09 N  10.13 E EKAH
  ABA ABAKAN_RUSSIAN_FED                        53.43 N  91.26 E UNAA
  ABD ABADAN_IRAN                               30.22 N  48.14 E OIAA
  ABI ABILENE_TX_USA                            32.25 N  99.41 W KABI
  ABJ ABIDJAN_COTE_D_IVOIRE                      5.15 N   3.56 W DIAP
  ABK KABRI_DAR_ETHIOPIA                         6.44 N  44.16 E HADK
  ABM BAMAGA_QL_AUSTRALIA                       10.52 S 142.24 E YBAM
";
            using (var reader = new StringReader(sampleString))
            {
                var parsedMemory = Tower3dRecordParser.ParseAirportRecords(reader)
                    .GetAwaiter().GetResult();
                var parsedSpan = parsedMemory.Span;
                Span<Tower3dAirportRecord> expectedRecords = new[]
                {
                    new Tower3dAirportRecord{IataIdentifier="AAA", IcaoIdentifier="NTGA", Latitude=LatitudeCoordinates.FromDouble(-17.25), Longitude=LongitudeCoordinates.FromDouble(-145.30)},
                    new Tower3dAirportRecord{IataIdentifier="AAE", IcaoIdentifier="DABB", Latitude=LatitudeCoordinates.FromDouble(+36.50), Longitude=LongitudeCoordinates.FromDouble(+008.00)},
                    new Tower3dAirportRecord{IataIdentifier="AAL", IcaoIdentifier="EKYT", Latitude=LatitudeCoordinates.FromDouble(+57.06), Longitude=LongitudeCoordinates.FromDouble(+009.51)},
                    new Tower3dAirportRecord{IataIdentifier="AAR", IcaoIdentifier="EKAH", Latitude=LatitudeCoordinates.FromDouble(+56.09), Longitude=LongitudeCoordinates.FromDouble(+010.13)},
                    new Tower3dAirportRecord{IataIdentifier="ABA", IcaoIdentifier="UNAA", Latitude=LatitudeCoordinates.FromDouble(+53.43), Longitude=LongitudeCoordinates.FromDouble(+091.26)},
                    new Tower3dAirportRecord{IataIdentifier="ABD", IcaoIdentifier="OIAA", Latitude=LatitudeCoordinates.FromDouble(+30.22), Longitude=LongitudeCoordinates.FromDouble(+048.14)},
                    new Tower3dAirportRecord{IataIdentifier="ABI", IcaoIdentifier="KABI", Latitude=LatitudeCoordinates.FromDouble(+32.25), Longitude=LongitudeCoordinates.FromDouble(-099.41)},
                    new Tower3dAirportRecord{IataIdentifier="ABJ", IcaoIdentifier="DIAP", Latitude=LatitudeCoordinates.FromDouble(+05.15), Longitude=LongitudeCoordinates.FromDouble(-003.56)},
                    new Tower3dAirportRecord{IataIdentifier="ABK", IcaoIdentifier="HADK", Latitude=LatitudeCoordinates.FromDouble(+06.44), Longitude=LongitudeCoordinates.FromDouble(+044.16)},
                    new Tower3dAirportRecord{IataIdentifier="ABM", IcaoIdentifier="YBAM", Latitude=LatitudeCoordinates.FromDouble(-10.52), Longitude=LongitudeCoordinates.FromDouble(+142.24)},
                };
                Assert.Equal(expectedRecords.Length, parsedSpan.Length);
                for (int i = 0; i < parsedSpan.Length; i++)
                {
                    ref readonly Tower3dAirportRecord expected = ref expectedRecords[i];
                    ref readonly Tower3dAirportRecord actual = ref parsedSpan[i];
                    Assert.Equal(expected.IataIdentifier, actual.IataIdentifier, ignoreCase: true);
                    Assert.Equal(expected.IcaoIdentifier, actual.IcaoIdentifier, ignoreCase: true);
                    Assert.Equal(expected.Latitude, actual.Latitude);
                    Assert.Equal(expected.Longitude, actual.Longitude);
                }
            }
        }

        [Fact]
        public static void CanParseGaAndLocalTraffic()
        {
            const string sampleString = @"// airport_from, airport_to, time, planetype, linecode, stop n go, touch n go, low approach,
TIST, KLAX, 10:00, CNF, 1N4634, 0, 0, 0, Nero 4634, NERO FOUR SIX THREE FOUR
KLAX, TIST, 09:20, CNF, 2N7544, 0, 0, 0, Nero 7544, NERO SEVEN FIVE FOUR FOUR
TNCM, TIST, 10:15, CNT, N324FT, 0, 0, 0, 324FT, THREE TWO FOUR FOX TANGO
TIST, TNCM, 10:45, CNT, N324FT, 0, 0, 0, 324FT, THREE TWO FOUR FOX TANGO
KMIA, TIST, 10:25, CNT, N765FT, 0, 0, 0, 765FT, SEVEN SIX FIVE FOX TANGO
TIST, TNCM, 10:52, CNT, N765FT, 0, 0, 0, 765FT, SEVEN SIX FIVE FOX TANGO
KSNA, TIST, 10:42, CNF, N002FT, 0, 0, 0, 002FT, ZERO TWO FOX TANGO
TIST, KFLL, 11:28, CNF, N002FT, 0, 0, 0, 002FT, ZERO TWO FOX TANGO
LHBP, TIST, 11:01, CNF, N001FT, 0, 0, 0, 001FT, ZERO ONE FOX TANGO
TIST, MDPC, 11:40, CNF, N001FT, 0, 0, 0, 001FT, ZERO ONE FOX TANGO
";
            using (var reader = new StringReader(sampleString))
            {
                var parsedMemory = Tower3dRecordParser.ParseGaLocalTrafficRecords(reader)
                    .GetAwaiter().GetResult();
                var parsedSpan = parsedMemory.Span;
                Span<Tower3dGaLocalTrafficRecord> expectedRecords = new[]
                {
                    new Tower3dGaLocalTrafficRecord{OriginAirportIcao="TIST", DestinationAirportIcao="KLAX",Time=new TimeSpan(10,00,00),AirplaneTypeIata="CNF",IcaoIdentifier="1N4634"},
                    new Tower3dGaLocalTrafficRecord{OriginAirportIcao="KLAX", DestinationAirportIcao="TIST",Time=new TimeSpan(09,20,00),AirplaneTypeIata="CNF",IcaoIdentifier="2N7544"},
                    new Tower3dGaLocalTrafficRecord{OriginAirportIcao="TNCM", DestinationAirportIcao="TIST",Time=new TimeSpan(10,15,00),AirplaneTypeIata="CNT",IcaoIdentifier="N324FT"},
                    new Tower3dGaLocalTrafficRecord{OriginAirportIcao="TIST", DestinationAirportIcao="TNCM",Time=new TimeSpan(10,45,00),AirplaneTypeIata="CNT",IcaoIdentifier="N324FT"},
                    new Tower3dGaLocalTrafficRecord{OriginAirportIcao="KMIA", DestinationAirportIcao="TIST",Time=new TimeSpan(10,25,00),AirplaneTypeIata="CNT",IcaoIdentifier="N765FT"},
                    new Tower3dGaLocalTrafficRecord{OriginAirportIcao="TIST", DestinationAirportIcao="TNCM",Time=new TimeSpan(10,52,00),AirplaneTypeIata="CNT",IcaoIdentifier="N765FT"},
                    new Tower3dGaLocalTrafficRecord{OriginAirportIcao="KSNA", DestinationAirportIcao="TIST",Time=new TimeSpan(10,42,00),AirplaneTypeIata="CNF",IcaoIdentifier="N002FT"},
                    new Tower3dGaLocalTrafficRecord{OriginAirportIcao="TIST", DestinationAirportIcao="KFLL",Time=new TimeSpan(11,28,00),AirplaneTypeIata="CNF",IcaoIdentifier="N002FT"},
                    new Tower3dGaLocalTrafficRecord{OriginAirportIcao="LHBP", DestinationAirportIcao="TIST",Time=new TimeSpan(11,01,00),AirplaneTypeIata="CNF",IcaoIdentifier="N001FT"},
                    new Tower3dGaLocalTrafficRecord{OriginAirportIcao="TIST", DestinationAirportIcao="MDPC",Time=new TimeSpan(11,40,00),AirplaneTypeIata="CNF",IcaoIdentifier="N001FT"},
                };
                Assert.Equal(expectedRecords.Length, parsedSpan.Length);
                for (int i = 0; i < parsedSpan.Length; i++)
                {
                    ref readonly Tower3dGaLocalTrafficRecord expected = ref expectedRecords[i];
                    ref readonly Tower3dGaLocalTrafficRecord actual = ref parsedSpan[i];
                    Assert.Equal(expected.OriginAirportIcao, actual.OriginAirportIcao, ignoreCase: true);
                    Assert.Equal(expected.DestinationAirportIcao, actual.DestinationAirportIcao, ignoreCase: true);
                    Assert.Equal(expected.Time, actual.Time);
                    Assert.Equal(expected.AirplaneTypeIata, actual.AirplaneTypeIata, ignoreCase: true);
                    Assert.Equal(expected.IcaoIdentifier, actual.IcaoIdentifier, ignoreCase: true);
                }
            }
        }

        [Fact]
        public static void CanParseSchedule()
        {
            const string sampleString = @"STX, STT, CNA, 9K, 8591, 16:12, 12:00, 1, 9K
SJU, STT, 320, B6, 1036, 16:07, 12:00, 1, B6
NCM, STT, DH3, LI,  550, 15:55, 12:00, 1, LI
SJU, STT, SF3, BB, 3593, 15:47, 12:00, 1, BB
FLL, STT, 319, NK,  201, 15:18, 12:00, 1, NK
MIA, STT, 73H, AA, 2421, 15:15, 12:00, 1, AA
CLT, STT, 752, AA,  833, 15:11, 12:00, 1, AA
STX, STT, CNA, 9K, 8531, 14:52, 12:00, 1, 9K
MIA, STT, 752, AA, 1391, 14:50, 12:00, 1, AA
BOS, STT, 320, B6,  807, 14:39, 12:00, 1, B6
";
            using (var reader = new StringReader(sampleString))
            {
                var parsedMemory = Tower3dRecordParser.ParseScheduleRecords(reader)
                    .GetAwaiter().GetResult();
                var parsedSpan = parsedMemory.Span;
                Span<Tower3dScheduleRecord> expectedRecords = new[]
                {
                    new Tower3dScheduleRecord{OriginAirportIata="STX", DestinationAirportIata="STT", AirplaneTypeIata="CNA", AirlineIata="9K", FlightNumber="8591", ArrivalTime=new TimeSpan(16,12,0)},
                    new Tower3dScheduleRecord{OriginAirportIata="SJU", DestinationAirportIata="STT", AirplaneTypeIata="320", AirlineIata="B6", FlightNumber="1036", ArrivalTime=new TimeSpan(16,07,0)},
                    new Tower3dScheduleRecord{OriginAirportIata="NCM", DestinationAirportIata="STT", AirplaneTypeIata="DH3", AirlineIata="LI", FlightNumber="550", ArrivalTime=new TimeSpan(15,55,0)},
                    new Tower3dScheduleRecord{OriginAirportIata="SJU", DestinationAirportIata="STT", AirplaneTypeIata="SF3", AirlineIata="BB", FlightNumber="3593", ArrivalTime=new TimeSpan(15,47,0)},
                    new Tower3dScheduleRecord{OriginAirportIata="FLL", DestinationAirportIata="STT", AirplaneTypeIata="319", AirlineIata="NK", FlightNumber="201", ArrivalTime=new TimeSpan(15,18,0)},
                    new Tower3dScheduleRecord{OriginAirportIata="MIA", DestinationAirportIata="STT", AirplaneTypeIata="73H", AirlineIata="AA", FlightNumber="2421", ArrivalTime=new TimeSpan(15,15,0)},
                    new Tower3dScheduleRecord{OriginAirportIata="CLT", DestinationAirportIata="STT", AirplaneTypeIata="752", AirlineIata="AA", FlightNumber="833", ArrivalTime=new TimeSpan(15,11,0)},
                    new Tower3dScheduleRecord{OriginAirportIata="STX", DestinationAirportIata="STT", AirplaneTypeIata="CNA", AirlineIata="9K", FlightNumber="8531", ArrivalTime=new TimeSpan(14,52,0)},
                    new Tower3dScheduleRecord{OriginAirportIata="MIA", DestinationAirportIata="STT", AirplaneTypeIata="752", AirlineIata="AA", FlightNumber="1391", ArrivalTime=new TimeSpan(14,50,0)},
                    new Tower3dScheduleRecord{OriginAirportIata="BOS", DestinationAirportIata="STT", AirplaneTypeIata="320", AirlineIata="B6", FlightNumber="807", ArrivalTime=new TimeSpan(14,39,0)},
                };
                Assert.Equal(expectedRecords.Length, parsedSpan.Length);
                for (int i = 0; i < parsedSpan.Length; i++)
                {
                    ref readonly Tower3dScheduleRecord expected = ref expectedRecords[i];
                    ref readonly Tower3dScheduleRecord actual = ref parsedSpan[i];
                    Assert.Equal(expected.OriginAirportIata, actual.OriginAirportIata, ignoreCase: true);
                    Assert.Equal(expected.DestinationAirportIata, actual.DestinationAirportIata, ignoreCase: true);
                    Assert.Equal(expected.AirplaneTypeIata, actual.AirplaneTypeIata, ignoreCase: true);
                    Assert.Equal(expected.AirlineIata, actual.AirlineIata, ignoreCase: true);
                    Assert.Equal(expected.FlightNumber, actual.FlightNumber, ignoreCase: true);
                    Assert.Equal(expected.ArrivalTime, actual.ArrivalTime);
                }
            }
        }

        [Fact]
        public static void CanParseTerminal()
        {
            const string sampleString = @"Terminal_1: AAL,DAL,JBU,KAP,LIA,NKS,SBS,SCX,UAL
Terminal_GA: GA
";
            using (var reader = new StringReader(sampleString))
            {
                var parsedMemory = Tower3dRecordParser.ParseTerminalRecords(reader)
                    .GetAwaiter().GetResult();
                var parsedSpan = parsedMemory.Span;
                Span<Tower3dTerminalRecord> expectedRecords = new[]
                {
                    new Tower3dTerminalRecord { Name = "Terminal_1", AirlineIcaoIdentifiers = new[] { "AAL", "DAL", "JBU", "KAP", "LIA", "NKS", "SBS", "SCX", "UAL" }.AsMemory() },
                    new Tower3dTerminalRecord { Name = "Terminal_GA", AirlineIcaoIdentifiers = new[] { "GA" }.AsMemory() },
                };
                Assert.Equal(expectedRecords.Length, parsedSpan.Length);
                for (int i = 0; i < parsedSpan.Length; i++)
                {
                    ref readonly Tower3dTerminalRecord expected = ref expectedRecords[i];
                    ref readonly Tower3dTerminalRecord actual = ref parsedSpan[i];
                    Assert.Equal(expected.Name, actual.Name, ignoreCase: true);
                    Assert.Equal(expected.AirlineIcaoIdentifiers.Length, actual.AirlineIcaoIdentifiers.Length);
                    var expectedAirlinesSpan = expected.AirlineIcaoIdentifiers.Span;
                    var actualAirlinesSpan = actual.AirlineIcaoIdentifiers.Span;
                    for (int j = 0; j < actualAirlinesSpan.Length; j++)
                        Assert.Equal(expectedAirlinesSpan[j], actualAirlinesSpan[j], ignoreCase: true);
                }
            }
        }
    }
}
