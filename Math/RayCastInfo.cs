
public class RayCastInfo
{
    public RayCastInfo(GameObject gameObject, ref Vector3 intersectionPoint, GameObjectPolygon polygon)
    {
        _GameObject = gameObject;
        _IntersectionPoint = intersectionPoint;
        _IsIntersecting = true;
        _Polygon = polygon;
    }

    public RayCastInfo() { }

    public GameObject? GameObject => _GameObject;
    public Vector3 IntersectionPoint => _IntersectionPoint;
    public bool IsIntersecting => _IsIntersecting;
    public GameObjectPolygon? Polygon => _Polygon;
    private bool _IsIntersecting;
    private GameObject? _GameObject;
    private Vector3 _IntersectionPoint;
    private GameObjectPolygon? _Polygon;
}