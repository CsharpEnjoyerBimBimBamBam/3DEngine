public class GameObjectPolygon
{
    public GameObjectPolygon(ref Triangle triangle, ref Vector3 normal)
    {
        Triangle = triangle;
        Normal = normal;
    }

    public Triangle Triangle;
    public Vector3 Normal;
}