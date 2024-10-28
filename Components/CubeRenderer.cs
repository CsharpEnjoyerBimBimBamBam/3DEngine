using System.Drawing;

public class CubeRenderer : Renderer3D
{
    public CubeRenderer(GameObject attachedGameObject) : base(attachedGameObject)
    {
        _Cube = new Parallelepiped(Vector3.One / 2);
        Scene.Current.FrameRenderStart += () =>
        {
            _LocalToWorldMatrix = AttachedTransform.LocalToWorldMatrix;
            Vector3 WorldPosition = AttachedTransform.WorldPosition;
            _Cube.TransformPosition(ref _LocalToWorldMatrix, ref WorldPosition);
        };
    }

    public double SideSize
    {
        get => _SideSize;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            _SideSize = value;
            double HalfSideSize = _SideSize / 2;
            _Cube = new Parallelepiped(new Vector3(HalfSideSize, HalfSideSize, HalfSideSize));
        }
    }
    private double _SideSize = 1;
    private Matrix3X3 _LocalToWorldMatrix;
    private Parallelepiped _Cube;

    protected override bool CheckForIntersection(ref Ray ray, out Vector3 IntersectionPoint, out Vector3 Normal)
    {
        Normal = Vector3.Zero;

        if (!_Cube.CheckForIntersection(ref ray, out IntersectionPoint, out Quadrilateral Face))
            return false;

        Normal = Face.Normal;

        return true;
    }
}
