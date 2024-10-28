
public struct Quadrilateral : IPolygon
{
    public Quadrilateral(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 vertex4)
    {
        Vertex1 = vertex1;
        Vertex2 = vertex2;
        Vertex3 = vertex3;
        Vertex4 = vertex4;
    }

    public Vector3[] Vertices => new Vector3[] { Vertex1, Vertex2, Vertex3, Vertex4 };
    public Vector3 RandomVertex => Vertex1;
    public Vector3 Normal => Vector3.CrossProduct(Vertex2 - Vertex1, Vertex4 - Vertex1);
    public Vector3 GeometryCenter => (Vertex1 + Vertex2 + Vertex3 + Vertex4) / 4;
    public Vector3 Vertex1 { get; set; }
    public Vector3 Vertex2 { get; set; }
    public Vector3 Vertex3 { get; set; }
    public Vector3 Vertex4 { get; set; }

    public bool CheckForIntersection(ref Line line) => CheckForIntersection(ref line, out _);

    public bool CheckForIntersection(ref Line line, out Vector3 IntersectionPoint)
    {
        IntersectionPoint = Vector3.Zero;

        if (!Plane.FromQuadrilateral(ref this).CheckForIntersection(line, out Vector3 PlaneIntersectionPoint))
            return false;

        Vector3 Vertex1Local = Vertex1 - PlaneIntersectionPoint;
        Vector3 Vertex2Local = Vertex2 - PlaneIntersectionPoint;
        Vector3 Vertex3Local = Vertex3 - PlaneIntersectionPoint;
        Vector3 Vertex4Local = Vertex4 - PlaneIntersectionPoint;

        Vector3 Cross1 = Vector3.CrossProduct(ref Vertex1Local, ref Vertex2Local);
        Vector3 Cross2 = Vector3.CrossProduct(ref Vertex2Local, ref Vertex3Local);
        Vector3 Cross3 = Vector3.CrossProduct(ref Vertex3Local, ref Vertex4Local);
        Vector3 Cross4 = Vector3.CrossProduct(ref Vertex4Local, ref Vertex1Local);

        double Dot1 = Vector3.DotProduct(ref Cross1, ref Cross2);
        double Dot2 = Vector3.DotProduct(ref Cross2, ref Cross3);
        double Dot3 = Vector3.DotProduct(ref Cross3, ref Cross4);
        double Dot4 = Vector3.DotProduct(ref Cross4, ref Cross1);

        if ((Dot1 >= 0 && Dot2 >= 0 && Dot3 >= 0 && Dot4 >= 0) || (Dot1 <= 0 && Dot2 <= 0 && Dot3 <= 0 && Dot4 <= 0))
        {
            IntersectionPoint = PlaneIntersectionPoint;
            return true;
        }

        return false;
    }
}