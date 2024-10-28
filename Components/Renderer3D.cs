using System.Drawing;

public abstract class Renderer3D : Renderer
{
    public Renderer3D(GameObject attachedGameObject) : base(attachedGameObject)
    {
        _Renderers3D.Add(this);
        Scene.Current.FrameRenderStart += () =>
        {
            _ClearedRenderers3D = _Renderers3D.Where(X => X != this).ToList();

            List<Light> AllLights = Scene.Current.Lights;

            _Lights = AllLights.Where(X => X.GetType().Name == nameof(Light)).ToList();
            _DirectionalLights = AllLights.Where(X => X.GetType().Name == nameof(DirectionalLight)).Cast<DirectionalLight>().ToList();
            _LightDirections = _DirectionalLights.Select(X => X.Transform.Forward).ToList();
            _MaxReflectionsCount = _Settings.MaxReflectionsCount;
        };
    }

    private double _MinBrightness = 0.1;
    private int _MaxReflectionsCount;
    private List<Light> _Lights;
    private List<DirectionalLight> _DirectionalLights;
    private List<Vector3> _LightDirections;
    private static List<Renderer3D> _Renderers3D = new List<Renderer3D>();
    private List<Renderer3D> _ClearedRenderers3D = new List<Renderer3D>();
    private EngineSettings _Settings = EngineSettings.GetInstance();
    private Random _Random = new Random();

    public override bool CheckForIntersection(ref Ray ray, out Color color, out Vector3 IntersectionPoint)
    {
        color = Material.Color;

        if (!CheckForIntersection(ref ray, out IntersectionPoint, out Vector3 Normal))
            return false;

        if (!_Settings.UseRayTracing)
        {
            color = GetPixelColor(ref Normal, ref IntersectionPoint);
            return true;
        }

        Vector3 CurrentColor = Material.Color.ToNormalizedVector3();

        for (int i = 0; i < _Settings.PixelRayCastingCount; i++)
            CurrentColor += TraceRay(ray, ref Normal, ref IntersectionPoint);

        CurrentColor /= _Settings.PixelRayCastingCount;

        int Red = (int)Math.Round(CurrentColor.X);
        int Green = (int)Math.Round(CurrentColor.Y);
        int Blue = (int)Math.Round(CurrentColor.Z);

        color = Color.FromArgb(Red, Green, Blue);

        return true;
    }

    protected abstract bool CheckForIntersection(ref Ray ray, out Vector3 IntersectionPoint, out Vector3 Normal);

    private bool CheckForIntersection(ref LineSegment lineSegment, out Vector3 IntersectionPoint, out Vector3 Normal)
    {
        Vector3 Direction = lineSegment.EndPoint - lineSegment.StartPoint;
        Ray ray = new Ray(lineSegment.StartPoint, Direction);
    
        if (!CheckForIntersection(ref ray, out IntersectionPoint, out Normal))
            return false;
    
        Vector3 StartToEnd = lineSegment.StartPoint - lineSegment.EndPoint;
        Vector3 IntersectionToEnd = IntersectionPoint - lineSegment.EndPoint;

        return Vector3.DotProduct(StartToEnd, IntersectionToEnd) > 0;
    }

    private Color GetPixelColor(ref Vector3 Normal, ref Vector3 IntersectionPoint)
    {
        if (_Lights.Count == 0 && _DirectionalLights.Count == 0)
            return Color.Black;

        double Brightness = 0;

        foreach (Light light in _Lights)
        {
            LineSegment lineSegment = new LineSegment(light.Transform.WorldPosition, IntersectionPoint);

            if (CheckLightForIntersection(ref lineSegment))
                continue;

            Vector3 LightLocal = light.Transform.WorldPosition - IntersectionPoint;
            double CurrentBrightness = CalculateBrightness(ref Normal, ref LightLocal);

            if (CurrentBrightness <= 0)
                continue;

            if (CurrentBrightness > Brightness)
                Brightness = CurrentBrightness;
        }

        foreach (Vector3 LightDirection in _LightDirections)
        {
            Ray LightRay = new Ray(IntersectionPoint, -LightDirection);

            if (CheckDirectionalLightForIntersection(ref LightRay))
                continue;

            double CurrentBrightness = CalculateBrightness(ref Normal, ref LightRay.Direction);

            if (CurrentBrightness <= 0)
                continue;

            if (CurrentBrightness > Brightness)
                Brightness = CurrentBrightness;
        }

        if (Brightness < _MinBrightness)
            Brightness = _MinBrightness;

        return Material.Color.Multiply(Brightness);
    }

    private Vector3 TraceRay(Ray ray, ref Vector3 Normal, ref Vector3 IntersectionPoint)
    {
        ray.StartPoint = IntersectionPoint;
        ray.Direction = CalculateRayDirection(ref ray.Direction, ref Normal, Material);

        Vector3 AverageColor = Material.Color.ToNormalizedVector3();

        int RefletionsCount = 0;
        Renderer3D PreviousRenderer = this;

        while (true)
        {
            if (!TryFindNearestIntersection(ref ray, out Vector3 CurrentIntersection, PreviousRenderer, out Vector3 CurrentNormal, out Renderer3D Renderer))
                break;

            PreviousRenderer = Renderer;

            Material CurrentMaterial = Renderer.Material;;

            Vector3 CurrentColor = CurrentMaterial.Color.ToNormalizedVector3();

            double ColorMultiplier = Renderer.Material.Reflectivity * Math.Abs(-Vector3.CosAngleBetween(ref ray.Direction, ref CurrentNormal));

            CurrentColor *= ColorMultiplier;

            MixColors(ref AverageColor, ref CurrentColor);

            ray.StartPoint = CurrentIntersection;
            ray.Direction = CalculateRayDirection(ref ray.Direction, ref CurrentNormal, Renderer.Material);

            RefletionsCount++;

            if (RefletionsCount > _MaxReflectionsCount)
                return Vector3.Zero;
        }

        double Brightness = 0;
        int Index = 0;

        for (int i = 0; i < _LightDirections.Count; i++)
        {
            Vector3 LightDirection = _LightDirections[i];
            double NormalizedAngle = -Vector3.CosAngleBetween(ref ray.Direction, ref LightDirection);

            if (NormalizedAngle > Brightness)
            {
                Index = i;
                Brightness = NormalizedAngle;
            }
        }

        if (Brightness != 0)
        {
            Vector3 LightColor = _DirectionalLights[Index].Color.ToNormalizedVector3() * Brightness;
            MixColors(ref AverageColor, ref LightColor);
        }

        AverageColor *= Brightness;
        AverageColor.Multiply(byte.MaxValue);

        return AverageColor;
    }

    private Vector3 CalculateRayDirection(ref Vector3 Direction, ref Vector3 Normal, Material material)
    {
        Vector3 NormalizedNormal = Normal.Normalized;
        Vector3 NormalizedDirection = Direction.Normalized;

        Vector3 Project = NormalizedNormal * (Vector3.DotProduct(NormalizedNormal, NormalizedDirection) * 2);

        Vector3 ReflectedDirection = Direction - Project;
        ReflectedDirection.Normalize();

        Vector3 RandomDirection = RandomVector();
        RandomDirection.Normalize();
        //RandomDirection += NormalizedNormal;

        if (Vector3.DotProduct(Normal, RandomDirection) < 0)
            RandomDirection = -RandomDirection;

        Vector3 NewDirection = Vector3.Lerp(ref ReflectedDirection, ref RandomDirection, material.Roughness);

        //double Alpha = material.Color.A / (double)byte.MaxValue;
        //
        //if (!_Random.NextBoolean(Alpha))
        //    NewDirection = -NewDirection;

        return NewDirection;
    }

    private void MixColors(ref Vector3 FirstColor, ref Vector3 SecondColor)
    {
        FirstColor.X *= SecondColor.X;
        FirstColor.Y *= SecondColor.Y;
        FirstColor.Z *= SecondColor.Z;
    }

    private Vector3 RandomVector()
    {
        int MinValue = -1;
        int MaxValue = 1;

        double X = _Random.NextDouble(MinValue, MaxValue);
        double Y = _Random.NextDouble(MinValue, MaxValue);
        double Z = _Random.NextDouble(MinValue, MaxValue);

        return new Vector3(X, Y, Z);
    }

    private double CalculateBrightness(ref Vector3 Normal, ref Vector3 LightLocal) => 
        Vector3.CosAngleBetween(ref Normal, ref LightLocal);

    private bool CheckLightForIntersection(ref LineSegment LightToIntersection)
    {
        foreach (Renderer3D Component in _ClearedRenderers3D)
            if (Component.CheckForIntersection(ref LightToIntersection, out _, out _))
                return true;
        return false;
    }

    private bool CheckDirectionalLightForIntersection(ref Ray LightRay)
    {
        foreach (Renderer3D Component in _ClearedRenderers3D)
            if (Component.CheckForIntersection(ref LightRay, out _, out _))
                return true;
        return false;
    }

    private bool TryFindNearestIntersection(ref Ray ray, out Vector3 IntersectionPoint, Renderer3D? PreviousRenderer, out Vector3 Normal, out Renderer3D Renderer)
    {
        double MinDistance = 0;
        Vector3 NearestIntersection = Vector3.Zero;
        Vector3 NearestNormal = Vector3.Zero;
        Renderer3D NearestRenderer = null;

        foreach (Renderer3D renderer in _Renderers3D)
        {
            if (renderer == PreviousRenderer)
                continue;

            if (!renderer.CheckForIntersection(ref ray, out Vector3 CurrentIntersectionPoint, out Vector3 CurrentNormal))
                continue;
        
            double Distance = Vector3.Distance(ref CurrentIntersectionPoint, ref ray.StartPoint);
        
            if (NearestRenderer == null)
            {
                NearestIntersection = CurrentIntersectionPoint;
                NearestNormal = CurrentNormal;
                NearestRenderer = renderer;
                MinDistance = Distance;
                continue;
            }
        
            if (Distance < MinDistance)
            {
                NearestIntersection = CurrentIntersectionPoint;
                NearestNormal = CurrentNormal;
                NearestRenderer = renderer;
                MinDistance = Distance;
            }
        }

        IntersectionPoint = Vector3.Zero;
        Normal = Vector3.Zero;
        Renderer = null;

        if (NearestRenderer == null)
            return false;

        IntersectionPoint = NearestIntersection;
        Normal = NearestNormal;
        Renderer = NearestRenderer;

        return true;
    }
}
