public class RayCast
{
    public RayCast(Ray ray)
    {
        _Ray = ray;
    }

    public RayCast()
    {

    }

    public Ray Ray { get => _Ray; set => _Ray = value; }
    public Dictionary<GameObject, Matrix3X3> LocalToWorldMatrixes
    {
        get => _LocalToWorldMatrixes;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            _LocalToWorldMatrixes = value;
        }
    }
    public double MaxDistance
    {
        get => _MaxDistance;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            _MaxDistance = value;
        }
    }
    public bool CheckForMatrixExistence = true;
    private Ray _Ray = new Ray();
    private double _MaxDistance = double.PositiveInfinity;
    private Dictionary<GameObject, Matrix3X3> _LocalToWorldMatrixes = new Dictionary<GameObject, Matrix3X3>();

    //public List<RayCastInfo> Cast(IList<RendererComponent> renderers)
    //{
    //    List<RayCastInfo> castInfo = new List<RayCastInfo>();
    //
    //    foreach (RendererComponent gameObject in renderers)
    //    {
    //        Matrix3X3 LocalToWorldMatrix;
    //        if (CheckForMatrixExistence)
    //        {
    //            LocalToWorldMatrix =
    //            _LocalToWorldMatrixes.ContainsKey(gameObject) ?
    //            _LocalToWorldMatrixes[gameObject] :
    //            gameObject.Transform.LocalToWorldMatrix;
    //        }
    //        else
    //        {
    //            LocalToWorldMatrix = _LocalToWorldMatrixes[gameObject];
    //        }
    //        Cast(gameObject, castInfo, ref LocalToWorldMatrix);
    //    }
    //    return castInfo;
    //}
    //
    //public void Cast(RendererComponent gameObject, List<RayCastInfo> CastInfo)
    //{
    //    Matrix3X3 WorldToLocalMatrix = gameObject.Transform.WorldToLocalMatrix;
    //    Cast(gameObject, CastInfo, ref WorldToLocalMatrix);
    //}

    //public void Cast(RendererComponent gameObject, List<RayCastInfo> CastInfo, ref Matrix3X3 LocalToWorldMatrix)
    //{
    //    
    //    gameObject.CheckForIntersection(ref _Ray, out _, out Vector3 intersectionPoint);
    //}

    //private bool IsInBounds(GameObject gameObject, ref Matrix3X3 LocalToWorldMatrix)
    //{
    //    Mesh mesh = gameObject.Mesh;
    //    Bounds bounds = mesh.Bounds;
    //
    //    Vector3 Size = bounds.Size;
    //    Vector3 Center = bounds.Center;
    //
    //    Vector3 FrontLeftDown = new Vector3(Center.X + Size.X, Center.Y - Size.Y, Center.Z + Size.Z);
    //    Vector3 FrontRightDown = new Vector3(Center.X - Size.X, Center.Y - Size.Y, Center.Z + Size.Z);
    //    Vector3 FrontLeftUp = new Vector3(Center.X + Size.X, Center.Y + Size.Y, Center.Z + Size.Z);
    //    Vector3 FrontRightUp = new Vector3(Center.X - Size.X, Center.Y + Size.Y, Center.Z + Size.Z);
    //
    //    Vector3 BottomLeftDown = new Vector3(Center.X + Size.X, Center.Y - Size.Y, Center.Z - Size.Z);
    //    Vector3 BottomRightDown = new Vector3(Center.X - Size.X, Center.Y - Size.Y, Center.Z - Size.Z);
    //    Vector3 BottomLeftUp = new Vector3(Center.X + Size.X, Center.Y + Size.Y, Center.Z - Size.Z);
    //    Vector3 BottomRightUp = new Vector3(Center.X - Size.X, Center.Y + Size.Y, Center.Z - Size.Z);
    //
    //    Vector3 WorldPosition = gameObject.Transform.WorldPosition;
    //
    //    ToWorldPosition(ref FrontLeftDown, ref WorldPosition, ref LocalToWorldMatrix, out FrontLeftDown);
    //    ToWorldPosition(ref FrontRightDown, ref WorldPosition, ref LocalToWorldMatrix, out FrontRightDown);
    //    ToWorldPosition(ref FrontLeftUp, ref WorldPosition, ref LocalToWorldMatrix, out FrontLeftUp);
    //    ToWorldPosition(ref FrontRightUp, ref WorldPosition, ref LocalToWorldMatrix, out FrontRightUp);
    //
    //    ToWorldPosition(ref BottomLeftDown, ref WorldPosition, ref LocalToWorldMatrix, out BottomLeftDown);
    //    ToWorldPosition(ref BottomRightDown, ref WorldPosition, ref LocalToWorldMatrix, out BottomRightDown);
    //    ToWorldPosition(ref BottomLeftUp, ref WorldPosition, ref LocalToWorldMatrix, out BottomLeftUp);
    //    ToWorldPosition(ref BottomRightUp, ref WorldPosition, ref LocalToWorldMatrix, out BottomRightUp);
    //
    //    IPolygon Front = new Quadrilateral(FrontRightDown, FrontLeftDown, FrontLeftUp, FrontRightUp);
    //    IPolygon Bottom = new Quadrilateral(BottomRightDown, BottomLeftDown, BottomLeftUp, BottomRightUp);
    //    IPolygon Left = new Quadrilateral(FrontLeftDown, BottomLeftDown, BottomLeftUp, FrontLeftUp);
    //    IPolygon Right = new Quadrilateral(BottomRightDown, FrontRightDown, FrontRightUp, BottomRightUp);
    //    IPolygon Down = new Quadrilateral(BottomRightDown, BottomLeftDown, FrontLeftDown, FrontRightDown);
    //    IPolygon Up = new Quadrilateral(FrontRightUp, FrontLeftUp, BottomLeftUp, BottomRightUp);
    //
    //    return Front.CheckForIntersection(ref _Ray, out _) || Bottom.CheckForIntersection(ref _Ray, out _) ||
    //        Left.CheckForIntersection(ref _Ray, out _) || Right.CheckForIntersection(ref _Ray, out _) ||
    //        Down.CheckForIntersection(ref _Ray, out _) || Up.CheckForIntersection(ref _Ray, out _);
    //}
    //
    //private GameObjectPolygon MeshPolygonToTriangle(GameObject gameObject, ref Matrix3X3 LocalToWorldMatrix, int startTriangleIndex)
    //{
    //    Vector3[] Vertices = gameObject.Mesh.Vertices;
    //    int[] Triangles = gameObject.Mesh.Triangles;
    //    Transform transform = gameObject.Transform;
    //
    //    Vector3 WorldPosition = transform.WorldPosition;
    //
    //    Vector3 Vertex1 = Vertices[Triangles[startTriangleIndex]] * LocalToWorldMatrix + WorldPosition;
    //    Vector3 Vertex2 = Vertices[Triangles[startTriangleIndex + 1]] * LocalToWorldMatrix + WorldPosition;
    //    Vector3 Vertex3 = Vertices[Triangles[startTriangleIndex + 2]] * LocalToWorldMatrix + WorldPosition;
    //
    //    Triangle triangle = new Triangle(Vertex1, Vertex2, Vertex3);
    //    Vector3 Zero = Vector3.Zero;
    //
    //    return new GameObjectPolygon(ref triangle, ref Zero);
    //}
    //
    //private void ToWorldPosition(ref Vector3 Position, ref Vector3 ObjectWorldPosition, ref Matrix3X3 LocalToWorldMatrix, out Vector3 WorldPosition)
    //{
    //    WorldPosition = (Position * LocalToWorldMatrix) + ObjectWorldPosition;
    //}
}
