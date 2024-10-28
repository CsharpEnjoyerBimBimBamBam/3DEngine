public interface IPolygon
{
    public Vector3[] Vertices { get; }
    public Vector3 RandomVertex { get; }
    public Vector3 Normal { get; }
    public Vector3 GeometryCenter { get; }

    public bool CheckForIntersection(ref Line line);

    public bool CheckForIntersection(ref Line line, out Vector3 IntersectionPoint);

    public bool CheckForIntersection(Line line, out Vector3 IntersectionPoint) => CheckForIntersection(ref line, out IntersectionPoint);

    public bool CheckForIntersection(ref Ray ray, out Vector3 IntersectionPoint)
    {
        Line line = ray.ToLine();
        return CheckForIntersection(ref line, out IntersectionPoint) && Vector3.DotProduct(IntersectionPoint - ray.StartPoint, ray.Direction) >= 0;
    }

    public bool CheckForIntersection(Ray ray, out Vector3 IntersectionPoint) => CheckForIntersection(ref ray, out IntersectionPoint);
}
