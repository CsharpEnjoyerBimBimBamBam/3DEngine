using System.Drawing;

public abstract class Renderer : Component
{
    public Renderer(GameObject attachedGameObject) : base(attachedGameObject)
    {

    }

    public Material Material
    {
        get => _Material;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            _Material = value;
        }
    }
    private Material _Material = new Material();

    public abstract bool CheckForIntersection(ref Ray ray, out Color color, out Vector3 IntersectionPoint);
}
