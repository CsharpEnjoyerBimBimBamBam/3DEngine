public struct LineSegment
{
    public LineSegment(Vector3 startPoint, Vector3 endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }

    public Vector3 StartPoint;
    public Vector3 EndPoint;

    public Line ToLine() => new Line(StartPoint, EndPoint);

    public bool CheckForIntersection(IPolygon polygon, out Vector3 IntersectionPoint)
    {
        if (!polygon.CheckForIntersection(ToLine(), out IntersectionPoint))
            return false;

        Vector3 IntersectionToStart = IntersectionPoint - StartPoint;
        Vector3 IntersectionToEnd = IntersectionPoint - StartPoint;

        Vector3 StartToEnd = StartPoint - EndPoint;
        Vector3 EndToStart = EndPoint - StartPoint;

        return Vector3.DotProduct(StartToEnd, IntersectionToEnd) > 0 && 
            Vector3.DotProduct(EndToStart, IntersectionToStart) > 0;
    }
}
