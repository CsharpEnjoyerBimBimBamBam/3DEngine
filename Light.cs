using System.Drawing;

public class Light : GameObject
{
    public Light()
    {
        
    }

    public Color Color { get; set; }

    public double Intensity
    {
        get => _Intensity;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            _Intensity = value;
        }
    }
    public bool CastSwadows { get; set; } = true;
    private double _Intensity;
}
