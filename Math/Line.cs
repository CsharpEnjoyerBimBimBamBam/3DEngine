

public struct Line
{
    public Line(Vector3 firstPoint, Vector3 secondPoint)
    {
        FirstPoint = firstPoint;
        SecondPoint = secondPoint;
    }

    public Vector3 FirstPoint { get; set; }
    public Vector3 SecondPoint { get; set; }
}