
public class EngineSettings
{
    private EngineSettings()
    {

    }

    public int RenderWidth { get; set; } = 200;
    public int RenderHeight { get; set; } = 190;
    public bool UseRayTracing { get; set; } = true;
    public int MaxReflectionsCount
    {
        get => _MaxReflectionsCount;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            _MaxReflectionsCount = value;
        }
    }
    public int PixelRayCastingCount
    {
        get => _PixelRayCastingCount;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            _PixelRayCastingCount = value;
        }
    }
    private int _MaxReflectionsCount = 10;
    private int _PixelRayCastingCount = 50;
    private static EngineSettings? _Instance;

    public static EngineSettings GetInstance() => _Instance ?? (_Instance = new EngineSettings());
}