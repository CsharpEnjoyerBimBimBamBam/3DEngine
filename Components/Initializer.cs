using System.Drawing;

public class Initializer : Behaviour
{
    public Initializer(GameObject attachedGameObject) : base(attachedGameObject)
    {

    }

    public override void Start()
    {
        Scene scene = Scene.Current;

        //GameObject Cube2 = GameObject.Create(PrimitiveType.Cube);
        GameObject Sphere1 = GameObject.Create(PrimitiveType.Sphere);
        Sphere1.Transform.WorldPosition = new Vector3(-4, 0, 2);
        Sphere1.GetComponent<Renderer>().Material.Roughness = 1;
        Sphere1.GetComponent<Renderer>().Material.Color = Color.FromArgb(255, 255, 255, 255);
        scene.Instantiate(Sphere1);

        GameObject Sphere2 = GameObject.Create(PrimitiveType.Sphere);
        Sphere2.Transform.WorldPosition = new Vector3(-1, 0.5, 2);
        Sphere2.GetComponent<SphereRenderer>().Radius = 1.5;
        Sphere2.GetComponent<Renderer>().Material.Color = Color.FromArgb(255, 255, 255, 255);
        Sphere2.GetComponent<Renderer>().Material.Roughness = 0.5;
        scene.Instantiate(Sphere2);

        GameObject Sphere3 = GameObject.Create(PrimitiveType.Sphere);
        Sphere3.Transform.WorldPosition = new Vector3(2.5, 1, 2);
        Sphere3.GetComponent<SphereRenderer>().Radius = 2;
        Sphere3.GetComponent<Renderer>().Material.Color = Color.FromArgb(255, 255, 255, 255);
        Sphere3.GetComponent<Renderer>().Material.Roughness = 0;
        scene.Instantiate(Sphere3);

        Light light1 = new Light();

        DirectionalLight light = new DirectionalLight();
        light.Transform.WorldRotation = new Vector3(-30, 0, 0);
        scene.Instantiate(light);

        light1.Transform.WorldPosition = new Vector3(3, 1, 2);

        //GameObject Cube1 = GameObject.Create(PrimitiveType.Cube);
        //Material CubeMaterial = Cube1.GetComponent<Renderer>().Material;
        //CubeMaterial.Color = Color.Red;
        //CubeMaterial.Reflectivity = 1;
        //Cube1.Transform.WorldPosition = new Vector3(2, 0, 2);
        //Cube1.Transform.WorldRotation = new Vector3(45, 45, 0);
        //scene.Instantiate(Cube1);

        GameObject Plane = GameObject.Create(PrimitiveType.Plane);
        Material PlaneMaterial = Plane.GetComponent<Renderer>().Material;
        PlaneMaterial.Reflectivity = 1;
        PlaneMaterial.Color = Color.Green;
        PlaneMaterial.Roughness = 1;
        Plane.Transform.Size = Vector3.One * 100;
        Plane.Transform.WorldPosition = new Vector3(0, -1, 0);
        Plane.Transform.WorldRotation = new Vector3(0, 90, 0);
        scene.Instantiate(Plane);

        //Sphere2.Transform.WorldPosition = new Vector3(-2, 0, 2);

        //Cube2.Transform.WorldPosition = new Vector3(-2, 0, 2);
        //Cube2.Transform.WorldRotation = new Vector3(20, 0, 0);
        //Cube2.Transform.Size = Vector3.One / 3;

        scene.MainCamera.Transform.WorldPosition = new Vector3(0, 0, -3);

        //Cube1.AddComponent<CubeMovement>();
        scene.MainCamera.AddComponent<CameraMovement>();
        //scene.MainCamera.Transform.WorldPosition = new Vector3(2, 0, 0);
    }
}