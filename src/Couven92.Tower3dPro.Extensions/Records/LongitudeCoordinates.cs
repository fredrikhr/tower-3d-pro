using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using THNETII.TypeConverter;

namespace Couven92.Tower3dPro.Extensions.Records
{
    public class LongitudeCoordinates : IEquatable<LongitudeCoordinates>, IEquatable<double>
    {
        public enum Modifier
        {
            [EnumMember(Value = "")]
            Unknown = 0,
            [EnumMember(Value = "E")]
            East = 1,
            [EnumMember(Value = "W")]
            West = -1,
        }

        public double Coordinates { get; set; }

        public Modifier EastWest { get; set; }

        public double AsDouble() => (int)EastWest * Coordinates;

        [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates", Justification = nameof(AsDouble))]
        public static implicit operator double(LongitudeCoordinates coordinates) =>
            coordinates.AsDouble();

        public static LongitudeCoordinates FromDouble(double coordinates)
        {
            int sign = Math.Sign(coordinates);
            var modifier = sign < 0 ? Modifier.West : Modifier.East;
            return new LongitudeCoordinates
            {
                Coordinates = Math.Abs(coordinates),
                EastWest = modifier
            };
        }

        public static explicit operator LongitudeCoordinates(double coordinates) =>
            FromDouble(coordinates);

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case null:
                    return false;
                case double d:
                    return Equals(d);
                case LongitudeCoordinates coordinates:
                    return Equals(coordinates);
                default: return false;
            }
        }

        public bool Equals(LongitudeCoordinates coordinates) =>
            AsDouble() == coordinates.AsDouble();

        public bool Equals(double coordinates) =>
            AsDouble() == coordinates;

        public override int GetHashCode() => Coordinates.GetHashCode();

        public static bool operator ==(LongitudeCoordinates left, LongitudeCoordinates right) =>
            left.Equals(right);
        public static bool operator ==(LongitudeCoordinates left, double right) =>
            left.Equals(right);
        public static bool operator ==(double left, LongitudeCoordinates right) =>
            right.Equals(left);

        public static bool operator !=(LongitudeCoordinates left, LongitudeCoordinates right) =>
            !left.Equals(right);
        public static bool operator !=(LongitudeCoordinates left, double right) =>
            !left.Equals(right);
        public static bool operator !=(double left, LongitudeCoordinates right) =>
            !right.Equals(left);

        public override string ToString()
        {
            var modifier = EnumStringConverter.ToString(EastWest);
            return FormattableString.Invariant($"{Coordinates}° {modifier}");
        }
    }
}
