using System.Runtime.CompilerServices;
using static System.Drawing.Color;

public struct Plane
{
    public Plane(double a, double b, double c, double d)
    {
        A = a;
        B = b;
        C = c;
        D = d;
    }

    public Plane(ref Vector3 normal, ref double d)
    {
        A = normal.X;
        B = normal.Y;
        C = normal.Z;
        D = d;
    }

    public double A { get; set; }
    public double B { get; set; }
    public double C { get; set; }
    public double D { get; set; }
    public Vector3 Normal => new Vector3(A, B, C);

    public bool CheckForIntersection(Ray ray, out Vector3 IntersectionPoint) => 
        CheckForIntersection(ray.ToLine(), out IntersectionPoint) && Vector3.DotProduct(IntersectionPoint - ray.StartPoint, ray.Direction) > 0;

    public bool CheckForIntersection(Line line, out Vector3 IntersectionPoint)
    {
        IntersectionPoint = Vector3.Zero;
        Vector3 FirstPoint = line.FirstPoint;
        Vector3 SecondPoint = line.SecondPoint;
        Vector3 FirstToSecond = FirstPoint - SecondPoint;
        double Numerator = D + (A * FirstPoint.X) + (B * FirstPoint.Y) + (C * FirstPoint.Z);
        double Denominator = A * FirstToSecond.X + B * FirstToSecond.Y + C * FirstToSecond.Z;

        if (Denominator == 0)
            return false;

        double Mu = Numerator / Denominator;

        IntersectionPoint = FirstPoint + ((SecondPoint - FirstPoint) * Mu);
        return true;
    }

    public static Plane FromPolygon(ref Vector3 normal, ref Vector3 vertex)
    {
        double d = -Vector3.DotProduct(ref normal, ref vertex);
        
        return new Plane(ref normal, ref d);
    }

    public static Plane FromPolygon(IPolygon polygon)
    {
        Vector3 normal = polygon.Normal;
        Vector3 vertex = polygon.RandomVertex;
        return FromPolygon(ref normal, ref vertex);
    }

    public static Plane FromTriangle(ref Triangle triangle)
    {
        Vector3 normal = triangle.Normal;
        Vector3 vertex = triangle.RandomVertex;
        return FromPolygon(ref normal, ref vertex);
    }

    public static Plane FromQuadrilateral(ref Quadrilateral quadrilateral)
    {
        Vector3 normal = quadrilateral.Normal;
        Vector3 vertex = quadrilateral.RandomVertex;
        return FromPolygon(ref normal, ref vertex);
    }
}
