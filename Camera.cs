
public class Camera : GameObject
{
    public static Camera Main => Scene.Current.MainCamera;
    public SortedColor SkyColor { get; set; } = SortedColor.Blue;
    public double SkyBrightness
    {
        get => _SkyBrightness;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            _SkyBrightness = value;
        }
    }
    public double FieldOfView
    {
        get => _FieldOfView;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            _FieldOfView = value;
        }
    }
    public double MaxRenderDistance
    {
        get => _MaxRenderDistance;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            _MaxRenderDistance = value;
        }
    }
    private double _SkyBrightness = 0;
    private double _FieldOfView = 60;
    private double _MaxRenderDistance = 20;
}
