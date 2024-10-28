
public class Bounds
{
    public Vector3 Center { get; set; }
    public Vector3 Size { get; set; }

    public Parallelepiped ToCube() => new Parallelepiped(Size);
}
