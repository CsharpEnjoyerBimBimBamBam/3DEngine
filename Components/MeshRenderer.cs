using System.Drawing;
using System.Dynamic;

public class MeshRenderer : Renderer3D
{
    public MeshRenderer(GameObject attachedGameObject) : base(attachedGameObject)
    {
        Scene.Current.FrameRenderStart += () =>
        {
            _LocalToWorldMatrix = AttachedTransform.LocalToWorldMatrix;
            _Polygons = new List<GameObjectPolygon>(_Mesh.TrianglesCount);
            _Bounds = _Mesh.Bounds.ToCube();

            Vector3 WorldPosition = AttachedTransform.WorldPosition;

            _Bounds.TransformPosition(ref _LocalToWorldMatrix, ref WorldPosition);

            for (int i = 0; i < _Mesh.Triangles.Length; i += 3)
                _Polygons.Add(TrianglesToPolygon(i));
        };
    }

    public Mesh Mesh
    {
        get => _Mesh;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _Mesh = value;
        }
    }
    private Mesh _Mesh = new Mesh();
    private Matrix3X3 _LocalToWorldMatrix;
    private List<GameObjectPolygon> _Polygons;
    private Parallelepiped _Bounds;

    protected override bool CheckForIntersection(ref Ray ray, out Vector3 IntersectionPoint, out Vector3 Normal)
    {
        IntersectionPoint = Vector3.Zero;
        Normal = Vector3.Zero;

        if (_Polygons.Count == 0)
            return false;

        //if (!_Bounds.CheckForIntersection(ref ray))
        //    return false;

        bool IsIntersecting = false;

        Vector3 CameraPosition = Camera.Main.Transform.WorldPosition;

        double MinDistance = 0;
        Vector3 CurrentNormal = Vector3.Zero;
        Vector3 CurrentIntersectionPoint = Vector3.Zero;

        for (int i = 0; i < _Polygons.Count; i++)
        {
            GameObjectPolygon Current = _Polygons[i];
            if (!ray.CheckForIntersection(ref Current.Triangle, out CurrentIntersectionPoint))
                continue;

            double Distance = Vector3.Distance(ref CameraPosition, ref CurrentIntersectionPoint);

            if (Distance > MinDistance && IsIntersecting)
                continue;

            CurrentNormal = Current.Normal;

            if (Vector3.DotProduct(ref Normal, ref ray.Direction) > 0)
                continue;

            IsIntersecting = true;
            MinDistance = Distance;
            IntersectionPoint = CurrentIntersectionPoint;
        }

        if (!IsIntersecting)
            return false;

        Normal = CurrentNormal;

        return true;
    }

    private GameObjectPolygon TrianglesToPolygon(int startTriangleIndex)
    {
        Vector3[] Vertices = Mesh.Vertices;
        int[] Triangles = Mesh.Triangles;

        Vector3 WorldPosition = AttachedTransform.WorldPosition;

        Vector3 Vertex1 = Vertices[Triangles[startTriangleIndex]] * _LocalToWorldMatrix + WorldPosition;
        Vector3 Vertex2 = Vertices[Triangles[startTriangleIndex + 1]] * _LocalToWorldMatrix + WorldPosition;
        Vector3 Vertex3 = Vertices[Triangles[startTriangleIndex + 2]] * _LocalToWorldMatrix + WorldPosition;

        Triangle triangle = new Triangle(Vertex1, Vertex2, Vertex3);
        Vector3 Normal = triangle.Normal;

        return new GameObjectPolygon(ref triangle, ref Normal);
    }
}
