public class SphereRenderer : Renderer3D
{
    public SphereRenderer(GameObject attachedGameObject) : base(attachedGameObject)
    {
        Scene.Current.FrameRenderStart += () => _WorldPosition = AttachedTransform.WorldPosition;
    }

    public double Radius
    {
        get => _Radius;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            _Radius = value;
        }
    }
    private double _Radius = 1;
    private Vector3 _WorldPosition;

    protected override bool CheckForIntersection(ref Ray Ray, out Vector3 IntersectionPoint, out Vector3 Normal)
    {
        Vector3 RayToSphere = Ray.StartPoint - _WorldPosition;

        double A = Vector3.DotProduct(ref Ray.Direction, ref Ray.Direction);
        double B = 2 * Vector3.DotProduct(ref Ray.Direction, ref RayToSphere);
        double C = Vector3.DotProduct(ref RayToSphere, ref RayToSphere) - (_Radius * _Radius);
        double Discriminant = (B * B) - (4 * A * C);

        IntersectionPoint = Vector3.Zero;
        Normal = Vector3.Zero;

        if (Discriminant < 0 || A == 0)
            return false;

        double DoubleA = 2 * A;

        if (Discriminant == 0)
        {
            double X = -B / DoubleA;
            IntersectionPoint = (Ray.Direction * X) + Ray.StartPoint;
            return IsIntersectionPointOnRay(ref Ray, ref IntersectionPoint);
        }

        double X1 = (-B + Math.Sqrt(Discriminant)) / DoubleA;
        double X2 = (-B - Math.Sqrt(Discriminant)) / DoubleA;

        IntersectionPoint = (Ray.Direction * X1) + Ray.StartPoint;

        if (!IsIntersectionPointOnRay(ref Ray, ref IntersectionPoint))
            return false;

        Vector3 SecondsIntersectionPoint = (Ray.Direction * X2) + Ray.StartPoint;

        if (Vector3.Distance(ref SecondsIntersectionPoint, ref Ray.StartPoint) < Vector3.Distance(ref IntersectionPoint, ref Ray.StartPoint))
            IntersectionPoint = SecondsIntersectionPoint;

        Normal = (IntersectionPoint - _WorldPosition).Normalized;

        return true;
    }

    private bool IsIntersectionPointOnRay(ref Ray ray, ref Vector3 Point)
    {
        Vector3 PointLocal = Point - ray.StartPoint;
        return Vector3.DotProduct(ref ray.Direction, ref PointLocal) > 0;
    }
}
