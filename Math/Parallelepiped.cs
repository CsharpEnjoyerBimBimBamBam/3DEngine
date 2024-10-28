public struct Parallelepiped
{
    public Parallelepiped(Vector3 size)
    {
        if (size.X <= 0 || size.Y <= 0 || size.Z <= 0) throw new ArgumentOutOfRangeException(nameof(size));
        _Size = size;
    }

    public Vector3 Size
    {
        get => _Size;
        set
        {
            if (value.X <= 0 || value.Y <=0 || value.Z <=0 ) throw new ArgumentOutOfRangeException(nameof(value));
            _Size = value;
        }
    }
    public Quadrilateral Front => _Front;
    public Quadrilateral Bottom => _Bottom;
    public Quadrilateral Left => _Left;
    public Quadrilateral Right => _Right;
    public Quadrilateral Down => _Down;
    public Quadrilateral Up => _Up;
    private Quadrilateral _Front = new Quadrilateral();
    private Quadrilateral _Bottom = new Quadrilateral();
    private Quadrilateral _Left = new Quadrilateral();
    private Quadrilateral _Right = new Quadrilateral();
    private Quadrilateral _Down = new Quadrilateral();
    private Quadrilateral _Up = new Quadrilateral();
    private Vector3 _Size;

    public void TransformPosition(ref Matrix3X3 LocalToWorldMatrix, ref Vector3 WorldPosition)
    {
        Vector3 FrontLeftDown = new Vector3(_Size.X, -_Size.Y, _Size.Z);
        Vector3 FrontRightDown = new Vector3(-_Size.X, -_Size.Y, _Size.Z);
        Vector3 FrontLeftUp = new Vector3(_Size.X, _Size.Y, _Size.Z);
        Vector3 FrontRightUp = new Vector3(-_Size.X, _Size.Y, _Size.Z);

        Vector3 BottomLeftDown = new Vector3(_Size.X, -_Size.Y, -_Size.Z);
        Vector3 BottomRightDown = new Vector3(-_Size.X, -_Size.Y, -_Size.Z);
        Vector3 BottomLeftUp = new Vector3(_Size.X, _Size.Y, -_Size.Z);
        Vector3 BottomRightUp = new Vector3(-_Size.X, _Size.Y, -_Size.Z);

        ToWorldPosition(ref FrontLeftDown, ref WorldPosition, ref LocalToWorldMatrix, out FrontLeftDown);
        ToWorldPosition(ref FrontRightDown, ref WorldPosition, ref LocalToWorldMatrix, out FrontRightDown);
        ToWorldPosition(ref FrontLeftUp, ref WorldPosition, ref LocalToWorldMatrix, out FrontLeftUp);
        ToWorldPosition(ref FrontRightUp, ref WorldPosition, ref LocalToWorldMatrix, out FrontRightUp);

        ToWorldPosition(ref BottomLeftDown, ref WorldPosition, ref LocalToWorldMatrix, out BottomLeftDown);
        ToWorldPosition(ref BottomRightDown, ref WorldPosition, ref LocalToWorldMatrix, out BottomRightDown);
        ToWorldPosition(ref BottomLeftUp, ref WorldPosition, ref LocalToWorldMatrix, out BottomLeftUp);
        ToWorldPosition(ref BottomRightUp, ref WorldPosition, ref LocalToWorldMatrix, out BottomRightUp);

        _Front = new Quadrilateral(FrontRightDown, FrontLeftDown, FrontLeftUp, FrontRightUp);
        _Bottom = new Quadrilateral(BottomRightDown, BottomRightUp, BottomLeftUp, BottomLeftDown);
        _Left = new Quadrilateral(FrontLeftDown, BottomLeftDown, BottomLeftUp, FrontLeftUp);
        _Right = new Quadrilateral(BottomRightDown, FrontRightDown, FrontRightUp, BottomRightUp);
        _Down = new Quadrilateral(BottomRightDown, BottomLeftDown, FrontLeftDown, FrontRightDown);
        _Up = new Quadrilateral(FrontRightUp, FrontLeftUp, BottomLeftUp, BottomRightUp);
    }

    public bool CheckForIntersection(ref Ray ray) =>
        ray.CheckForIntersection(_Front, out _) || ray.CheckForIntersection(_Bottom, out _) ||
        ray.CheckForIntersection(_Left, out _) || ray.CheckForIntersection(_Right, out _) ||
        ray.CheckForIntersection(_Down, out _) || ray.CheckForIntersection(_Up, out _);

    public bool CheckForIntersection(ref Ray ray, out Vector3 IntersectionPoint, out Quadrilateral IntersectionFace)
    {
        Span<IntersectionData> IntersectionDatas = stackalloc IntersectionData[6];

        IntersectionDatas[0] = 
            new IntersectionData(ray.CheckForIntersection(_Front, out Vector3 FrontIntersectionPoint), ref FrontIntersectionPoint, ref _Front);
        IntersectionDatas[1] = 
            new IntersectionData(ray.CheckForIntersection(_Bottom, out Vector3 BottomIntersectionPoint), ref BottomIntersectionPoint, ref _Bottom);
        IntersectionDatas[2] = 
            new IntersectionData(ray.CheckForIntersection(_Left, out Vector3 LeftIntersectionPoint), ref LeftIntersectionPoint, ref _Left);
        IntersectionDatas[3] = 
            new IntersectionData(ray.CheckForIntersection(_Right, out Vector3 RightIntersectionPoint), ref RightIntersectionPoint, ref _Right);
        IntersectionDatas[4] = 
            new IntersectionData(ray.CheckForIntersection(_Down, out Vector3 DownIntersectionPoint), ref DownIntersectionPoint, ref _Down);
        IntersectionDatas[5] = 
            new IntersectionData(ray.CheckForIntersection(_Up, out Vector3 UpIntersectionPoint), ref UpIntersectionPoint, ref _Up);

        bool IsIntersecting = false;
        bool IsAssigned = false;
        double MinDistance = 0;
        Vector3 NearestIntersectionPoint = Vector3.Zero;
        Quadrilateral Face = new Quadrilateral();

        for (int i = 0; i < IntersectionDatas.Length; i++)
        {
            IntersectionData Data = IntersectionDatas[i];

            if (!Data.IsIntersecting)
                continue;

            IsIntersecting = true;

            double Distance = Vector3.Distance(ref Data.IntersectionPoint, ref ray.StartPoint);

            if (!IsAssigned)
            {
                NearestIntersectionPoint = Data.IntersectionPoint;
                Face = Data.Face;
                MinDistance = Distance;
                IsAssigned = true;
                continue;
            }

            if (Distance < MinDistance)
            {
                NearestIntersectionPoint = Data.IntersectionPoint;
                Face = Data.Face;
                MinDistance = Distance;
            }
        }

        IntersectionPoint = NearestIntersectionPoint;
        IntersectionFace = Face;

        return IsIntersecting;
    }

    private void ToWorldPosition(ref Vector3 Position, ref Vector3 ObjectWorldPosition, ref Matrix3X3 LocalToWorldMatrix, out Vector3 WorldPosition)
    {
        WorldPosition = (Position * LocalToWorldMatrix) + ObjectWorldPosition;
    }

    private struct IntersectionData
    {
        public IntersectionData(bool isIntersecting, ref Vector3 intersectionPoint, ref Quadrilateral face)
        {
            IsIntersecting = isIntersecting;
            IntersectionPoint = intersectionPoint;
            Face = face;
        }

        public bool IsIntersecting;
        public Vector3 IntersectionPoint;
        public Quadrilateral Face;
    }
}
