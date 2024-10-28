
public class Mesh
{
    public Vector3[] Vertices
    {
        get => _Vertices;
        set
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _Vertices = value;
        }
    }
    public int[] Triangles
    {
        get => _Triangles;
        set
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (value.Length % 3 != 0)
                throw new Exception("Triangles count must be devided by 3 without remainder");
            _Triangles = value;
        }
    }
    public Vector3[] Normals
    {
        get => _Normals;
        set
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (value.Length != _Vertices.Length) throw new ArgumentOutOfRangeException(nameof(value));

            _Normals = value;
        }
    }
    public Bounds Bounds
    {
        get => _Bounds;
        set
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _Bounds = value;
        }
    }
    public int TrianglesCount => Triangles.Length / 3;
    private Vector3[] _Vertices = new Vector3[0];
    private int[] _Triangles = new int[0];
    private Vector3[] _Normals = new Vector3[0];
    private Bounds _Bounds = new Bounds();
}
