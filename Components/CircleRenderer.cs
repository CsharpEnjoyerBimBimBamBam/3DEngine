using System.Drawing;

public class CircleRenderer : Renderer
{
    public CircleRenderer(GameObject attachedGameObject) : base(attachedGameObject)
    {
        Scene.Current.FrameRenderStart += () =>
        {
            Camera MainCamera = Scene.Current.MainCamera;

            _CameraPositon = MainCamera.Transform.WorldPosition;
            _CameraForward = MainCamera.Transform.Forward;
            _CircleCenter = -AttachedTransform.Forward + _CameraPositon;
        };
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
    private double _Radius = 0.1;
    private Vector3 _CameraPositon;
    private Vector3 _CameraForward;
    private Vector3 _CircleCenter;

    public override bool CheckForIntersection(ref Ray ray, out Color color, out Vector3 IntersectionPoint)
    {
        color = Material.Color;
        IntersectionPoint = new Vector3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);

        Vector3 CameraBackward = -_CameraForward;
        Plane CirclePlane = Plane.FromPolygon(ref CameraBackward, ref _CircleCenter);

        return CirclePlane.CheckForIntersection(ray, out Vector3 CircleIntersectionPoint) && 
            Vector3.Distance(ref CircleIntersectionPoint, ref _CircleCenter) <= _Radius;
    }
}