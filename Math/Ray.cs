
public struct Ray
{
    public Ray(Vector3 startPoint, Vector3 direction)
    {
        StartPoint = startPoint;
        Direction = direction;
    }

    public Vector3 StartPoint;
    public Vector3 Direction;

    public Line ToLine() => new Line(StartPoint, StartPoint + Direction);

    public bool CheckForIntersection(IPolygon polygon, out Vector3 IntersectionPoint)
    {
        IntersectionPoint = Vector3.Zero;
        Vector3 Normal = polygon.Normal;

        if (Vector3.DotProduct(ref Direction, ref Normal) > 0)
            return false;

        return polygon.CheckForIntersection(ToLine(), out IntersectionPoint) && Vector3.DotProduct(IntersectionPoint - StartPoint, Direction) > 0;
    }

    public bool CheckForIntersection(ref Triangle triangle, out Vector3 IntersectionPoint)
    {
        IntersectionPoint = Vector3.Zero;

        Vector3 Normal = triangle.Normal;
        
        if (Vector3.DotProduct(ref Direction, ref Normal) > 0)
            return false;

        Line line = ToLine();

        if (!triangle.CheckForIntersection(ref line, out IntersectionPoint))
            return false;

        Vector3 IntersectionPointLocal = IntersectionPoint - StartPoint;

        return Vector3.DotProduct(ref IntersectionPointLocal, ref Direction) > 0;
    }
}
