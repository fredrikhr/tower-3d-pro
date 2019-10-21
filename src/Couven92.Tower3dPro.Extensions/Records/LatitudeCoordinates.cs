using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using THNETII.TypeConverter.Serialization;

namespace Couven92.Tower3dPro.Extensions.Records
{
    public struct LatitudeCoordinates : IEquatable<LatitudeCoordinates>, IEquatable<double>
    {
        public enum Modifier
        {
            [EnumMember(Value = "")]
            Unknown = 0,
            [EnumMember(Value = "N")]
            North = 1,
            [EnumMember(Value = "S")]
            South = -1
        }

        public double Coordinates { get; set; }

        public Modifier NorthSouth { get; set; }

        public double AsDouble() => (int)NorthSouth * Coordinates;

        [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates", Justification = nameof(AsDouble))]
        public static implicit operator double(LatitudeCoordinates coordinates) =>
            coordinates.AsDouble();

        public static LatitudeCoordinates FromDouble(double coordinates)
        {
            int sign = Math.Sign(coordinates);
            var modifier = sign < 0 ? Modifier.South : Modifier.North;
            return new LatitudeCoordinates
            {
                Coordinates = Math.Abs(coordinates),
                NorthSouth = modifier
            };
        }

        public static explicit operator LatitudeCoordinates(double coordinates) =>
            FromDouble(coordinates);

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case null:
                    return false;
                case double d:
                    return Equals(d);
                case LatitudeCoordinates coordinates:
                    return Equals(coordinates);
                default: return false;
            }
        }

        public bool Equals(LatitudeCoordinates coordinates) =>
            AsDouble() == coordinates.AsDouble();

        public bool Equals(double coordinates) =>
            AsDouble() == coordinates;

        public override int GetHashCode() => Coordinates.GetHashCode();

        public static bool operator ==(LatitudeCoordinates left, LatitudeCoordinates right) =>
            left.Equals(right);
        public static bool operator ==(LatitudeCoordinates left, double right) =>
            left.Equals(right);
        public static bool operator ==(double left, LatitudeCoordinates right) =>
            right.Equals(left);

        public static bool operator !=(LatitudeCoordinates left, LatitudeCoordinates right) =>
            !left.Equals(right);
        public static bool operator !=(LatitudeCoordinates left, double right) =>
            !left.Equals(right);
        public static bool operator !=(double left, LatitudeCoordinates right) =>
            !right.Equals(left);

        public override string ToString()
        {
            var modifier = EnumMemberStringConverter.ToString(NorthSouth);
            return FormattableString.Invariant($"{Coordinates}° {modifier}");
        }
    }
}
