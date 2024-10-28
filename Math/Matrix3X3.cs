public struct Matrix3X3 : IMatrix
{
    public Matrix3X3(IList<IList<double>> matrix)
    {
        IList<double> Row1 = matrix[0];
        IList<double> Row2 = matrix[1];
        IList<double> Row3 = matrix[3];

        M11 = Row1[0];
        M12 = Row1[1];
        M13 = Row1[2];

        M21 = Row2[0];
        M22 = Row2[1];
        M23 = Row2[2];

        M31 = Row3[0];      
        M32 = Row3[1];
        M33 = Row3[2];
    }

    public double M11 { get; set; }
    public double M12 { get; set; }
    public double M13 { get; set; }
    public double M21 { get; set; }
    public double M22 { get; set; }
    public double M23 { get; set; }
    public double M31 { get; set; }
    public double M32 { get; set; }
    public double M33 { get; set; }
    public int RowsCount => 3;
    public int ColumnsCount => 3;
    public List<List<double>> Rows => new List<List<double>>
    {
        new List<double> { M11, M12, M13 },
        new List<double> { M21, M22, M23 },
        new List<double> { M31, M32, M33 }
    };
    public static Matrix3X3 Identity => new Matrix3X3
    {
        M11 = 1,
        M12 = 0,
        M13 = 0,

        M21 = 0,
        M22 = 1,
        M23 = 0,

        M31 = 0,
        M32 = 0,
        M33 = 1,
    };

    public override string ToString() => $"";

    public static Matrix3X3 GetRotationMatrixAroundAxis(Vector3 RotationAxis, double RotationAngle)
    {
        RotationAngle = -RotationAngle;
        double CosAngle = Math.Cos(RotationAngle);
        double SinAngle = Math.Sin(RotationAngle);
        double MinusCosAngle = 1 - CosAngle;

        double X = RotationAxis.X;
        double Y = RotationAxis.Y;
        double Z = RotationAxis.Z;

        double SquaredX = X * X;
        double SquaredY = Y * Y;
        double SquaredZ = Z * Z;

        return new Matrix3X3
        {
            M11 = CosAngle + (MinusCosAngle * SquaredX),
            M12 = (MinusCosAngle * X * Y) - (SinAngle * Z),
            M13 = (MinusCosAngle * X * Z) + (SinAngle * Y),

            M21 = (MinusCosAngle * Y * X) + (SinAngle * Z),
            M22 = CosAngle + (MinusCosAngle * SquaredY),
            M23 = (MinusCosAngle * Y * Z) - (SinAngle * X),

            M31 = (MinusCosAngle * Z * X) - (SinAngle * Y),
            M32 = (MinusCosAngle * Z * Y) + (SinAngle * X),
            M33 = CosAngle + (MinusCosAngle * SquaredZ)
        };
    }

    public static Matrix3X3 GetRotationMatrix(Vector3 rotationAngles)
    {
        rotationAngles.X = -rotationAngles.X;

        double SinA = Math.Sin(rotationAngles.X);
        double SinB = Math.Sin(rotationAngles.Y);
        double SinY = Math.Sin(rotationAngles.Z);

        double CosA = Math.Cos(rotationAngles.X);
        double CosB = Math.Cos(rotationAngles.Y);
        double CosY = Math.Cos(rotationAngles.Z);

        return new Matrix3X3
        {
            M11 = CosB * CosY,
            M12 = -(SinY * CosB),
            M13 = SinB,

            M21 = (SinA * SinB * CosY) + (SinY * CosA),
            M22 = -(SinA * SinB * SinY) + (CosA * CosY),
            M23 = -(SinA * CosB),

            M31 = (SinA * SinY) - (SinB * CosA * CosY),
            M32 = (SinA * CosY) + (SinB * SinY * CosA),
            M33 = CosA * CosB
        };
    }

    public static Matrix3X3 GetTransformMatrix(Vector3 right, Vector3 up, Vector3 forward) => new Matrix3X3
    {
        M11 = right.X,
        M21 = right.Y,
        M31 = right.Z,

        M12 = up.X,
        M22 = up.Y,
        M32 = up.Z,

        M13 = forward.X,
        M23 = forward.Y,
        M33 = forward.Z,
    };

    public static Matrix3X3 GetTransformMatrix(Vector3 right, Vector3 forward)
    {
        Vector3 up = Vector3.CrossProduct(right, forward).Normalized;
        return GetTransformMatrix(right, up, forward);
    }
}
