using System.Diagnostics.CodeAnalysis;
using System.Numerics;

public struct Vector3
{
    public Vector3(double x, double y, double z)
    {
        X = x; 
        Y = y; 
        Z = z;
    }

    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public double Length => Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
    public Vector3 Normalized => this / Length;
    public static Vector3 Zero => new Vector3();
    public static Vector3 One => new Vector3(1, 1 ,1);
    public static Vector3 Forward => new Vector3(0, 0, 1);
    public static Vector3 Backward => new Vector3(0, 0, -1);
    public static Vector3 Up => new Vector3(0, 1, 0);
    public static Vector3 Down => new Vector3(0, -1, 0);
    public static Vector3 Right => new Vector3(1, 0, 0);
    public static Vector3 Left => new Vector3(-1, 0, 0);

    public static Vector3 operator +(Vector3 L, Vector3 R) => new Vector3(L.X + R.X, L.Y + R.Y, L.Z + R.Z);

    public static Vector3 operator -(Vector3 L, Vector3 R) => new Vector3(L.X - R.X, L.Y - R.Y, L.Z - R.Z);

    public static Vector3 operator -(Vector3 vector) => new Vector3(-vector.X, -vector.Y, -vector.Z);

    public static Vector3 operator *(Vector3 L, double Multiplier) => new Vector3(L.X * Multiplier, L.Y * Multiplier, L.Z * Multiplier);

    public static Vector3 operator *(Vector3 vector, Matrix3X3 matrix) => new Vector3
    {
        X = vector.X * matrix.M11 + vector.Y * matrix.M12 + vector.Z * matrix.M13,
        Y = vector.X * matrix.M21 + vector.Y * matrix.M22 + vector.Z * matrix.M23,
        Z = vector.X * matrix.M31 + vector.Y * matrix.M32 + vector.Z * matrix.M33,
    };

    public static Vector3 operator /(Vector3 L, double Devider) => new Vector3(L.X / Devider, L.Y / Devider, L.Z / Devider);

    public static bool operator ==(Vector3 L, Vector3 R) => L.Equals(R);

    public static bool operator !=(Vector3 L, Vector3 R) => !L.Equals(R);

    public override string ToString() => $"{X}; {Y}; {Z}";

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Vector3 Vector))
            return false;

        return Vector.X == X && Vector.Y == Y && Vector.Z == Z;
    }

    public override int GetHashCode() => base.GetHashCode();

    public void Normalize()
    {
        double VectorLength = Length;
        X /= VectorLength;
        Y /= VectorLength;
        Z /= VectorLength;
    }

    public void Multiply(double Multiplier)
    {
        X *= Multiplier;
        Y *= Multiplier;
        Z *= Multiplier;
    }

    public static Vector3 CrossProduct(ref Vector3 current, ref Vector3 other)
    {
        double X = (current.Z * other.Y) - (current.Y * other.Z);
        double Y = (current.X * other.Z) - (current.Z * other.X);
        double Z = (current.Y * other.X) - (current.X * other.Y);
        return new Vector3(X, Y, Z);
    }

    public static Vector3 CrossProduct(Vector3 current, Vector3 other) => CrossProduct(ref current, ref other);

    public static double DotProduct(Vector3 current, Vector3 other) => DotProduct(ref current, ref other);

    public static double DotProduct(ref Vector3 current, ref Vector3 other) => (current.X * other.X) + (current.Y * other.Y) + (current.Z * other.Z);

    public static double CosAngleBetween(ref Vector3 current, ref Vector3 other) => 
        TryCalculateCosAngle(ref current, ref other, out double CosAngle) ? CosAngle : 1d;

    public static double CosAngleBetween(Vector3 current, Vector3 other) => CosAngleBetween(ref current, ref other);

    public static double AngleBetween(Vector3 current, Vector3 other) => 
        TryCalculateCosAngle(ref current, ref other, out double CosAngle) ? Math.Acos(CosAngle) * MathConstant.RadiansToDegrees : 0;

    public static Vector3 RotateTowards(ref Vector3 from, ref Vector3 to, ref double Angle)
    {
        Vector3 RotationAxis = CrossProduct(from, to).Normalized;
        Matrix3X3 RotationMatrix = Matrix3X3.GetRotationMatrixAroundAxis(RotationAxis, Angle);
        return from * RotationMatrix;
    }

    public static Vector3 RotateTowards(Vector3 from, Vector3 to, double Angle) => RotateTowards(ref from, ref to, ref Angle);

    public static Vector3 RotateAround(ref Vector3 rotationVector, ref Vector3 RotationAxis, ref double Angle) =>
        rotationVector * Matrix3X3.GetRotationMatrixAroundAxis(RotationAxis, Angle);

    public static Vector3 RotateAround(Vector3 rotationVector, Vector3 RotationAxis, double Angle) => 
        RotateAround(ref rotationVector, ref RotationAxis, ref Angle);

    public static Vector3 Rotate(ref Vector3 vector, ref Vector3 rotation) => vector * Matrix3X3.GetRotationMatrix(rotation);

    public static Vector3 Rotate(Vector3 vector, Vector3 rotation) => Rotate(ref vector, ref rotation);

    public static double Distance(ref Vector3 vector1, ref Vector3 vector2) => (vector1 - vector2).Length;

    public static double Distance(Vector3 vector1, Vector3 vector2) => Distance(ref vector1, ref vector2);

    public static Vector3 ProjectOnVector(ref Vector3 vector, ref Vector3 projectVector)
    {
        Vector3 NormalizedProject = projectVector.Normalized;
        return NormalizedProject * DotProduct(ref vector, ref NormalizedProject);
    }

    public static Vector3 ProjectOnVector(Vector3 vector, Vector3 projectVector) => ProjectOnVector(ref vector, ref projectVector);

    public static Vector3 Reflect(ref Vector3 vector, ref Vector3 normal) => vector - (ProjectOnVector(ref vector, ref normal) * 2);

    public static Vector3 Reflect(Vector3 vector, Vector3 normal) => Reflect(ref vector, ref normal);

    public static Vector3 Lerp(ref Vector3 first, ref Vector3 second, double t) => first + ((second - first) * t);

    private static bool TryCalculateCosAngle(ref Vector3 current, ref Vector3 other, out double CosAngle)
    {
        CosAngle = 1;
        double CurrentLength = current.Length;
        double OtherLength = other.Length;

        if (CurrentLength == 0 || OtherLength == 0)
            return false;

        CosAngle = DotProduct(current, other) / (CurrentLength * OtherLength);
        
        return true;
    }
}
