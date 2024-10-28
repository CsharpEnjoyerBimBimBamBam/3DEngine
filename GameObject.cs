
using System.ComponentModel;
using System.Reflection;

public class GameObject
{
    public GameObject()
    {

    }

    public Transform Transform => _Transform;
    public string Name { get; set; } = string.Empty;
    private Transform _Transform = new Transform();
    private List<Component> _Components = new List<Component>();
    private List<Behaviour> _Behaviours = new List<Behaviour>();

    public static GameObject Create(PrimitiveType type)
    {
        switch (type) 
        {
            case PrimitiveType.Plane: return CreatePlane();
            case PrimitiveType.Cube: return CreateCube();
            case PrimitiveType.Sphere: return CreateSphere();
            default: return new GameObject();
        }
    }

    public T GetComponent<T>() where T : Component
    {
        Component? Component = _Components.Find(X => X is T);
        if (Component == null)
            throw new Exception();

        return (T)Component;
    }

    public List<T> GetComponents<T>() where T : Component => _Components.FindAll(X => X is T).Cast<T>().ToList();

    public T AddComponent<T>() where T : Component
    {
        if (_Components.Find(X => X is T) != null)
            throw new Exception();
        
        T Component = (T)Activator.CreateInstance(typeof(T), this);
        _Components.Add(Component);

        if (Component is Behaviour behaviour)
            behaviour.Start();

        UpdateBehaviours();
        return Component;
    }

    public void InvokeUpdates(double DeltaTime) => _Behaviours.ForEach(X => X.Update(DeltaTime));

    private void UpdateBehaviours() => _Behaviours = GetComponents<Behaviour>();

    private static GameObject CreatePlane()
    {
        GameObject plane = new GameObject();
        MeshRenderer meshRenderer = plane.AddComponent<MeshRenderer>();
        meshRenderer.Mesh = new Mesh
        {
            Vertices = new Vector3[]
            {
                new Vector3(0.5, -0.5, 0),
                new Vector3(-0.5, -0.5, 0),
                new Vector3(-0.5, 0.5, 0),
                new Vector3(0.5, 0.5, 0),
            },
            Triangles = new int[]
            {
                0, 1, 2, 0, 2, 3
            },
            Bounds = new Bounds
            {
                Size = new Vector3(0.5, 0.5, 0.1)
            }
        };
        return plane;
    }

    private static GameObject CreateCube()
    {
        GameObject Cube = new GameObject();

        Cube.AddComponent<CubeRenderer>();

        return Cube;
    }

    private static GameObject CreateSphere()
    {
        GameObject sphere = new GameObject();
        sphere.AddComponent<SphereRenderer>();
        return sphere;
    }
}
