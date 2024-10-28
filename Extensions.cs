using System.Drawing;

public static class Extensions
{
    private const double _ByteMaxValue = byte.MaxValue;

    public static Color Multiply(this Color color, double Multiplier)
    {
        int R = (int)Math.Round(color.R * Multiplier);
        int G = (int)Math.Round(color.G * Multiplier);
        int B = (int)Math.Round(color.B * Multiplier);
        return Color.FromArgb(1, R, G, B);
    }

    public static Vector3 ToVector3(this Color color) => 
        new Vector3(color.R, color.G, color.B);

    public static Vector3 ToNormalizedVector3(this Color color) =>
        ToVector3(color) / _ByteMaxValue;

    public static double NextDouble(this Random random, double MinValue, double MaxValue) => 
        (random.NextDouble() * (MaxValue - MinValue)) + MinValue;

    public static bool NextBoolean(this Random random) => 
        random.Next(0, 2) == 1;

    public static bool NextBoolean(this Random random, double Chance) =>
        Chance == 0 ? false : random.Next(0, (int)Math.Round(1 / Chance)) == 0;
}