using OpenTK.Windowing.GraphicsLibraryFramework;

public class CameraMovement : Behaviour
{
    public CameraMovement(GameObject attachedGameObject) : base(attachedGameObject)
    {

    }

    private double _MovementSpeed = 3;
    private double _RotationSpeed = 100;
    private Light _Light;

    public override void Start()
    {
        _Light = new Light();
        //Scene.Current.Instantiate(_Light);
    }

    public override void Update(double DeltaTime)
    {
        Vector3 Movement = Vector3.Zero;
        Vector3 Rotaton = Vector3.Zero;
    
        Vector3 Forward = AttachedTransform.Forward;
        Vector3 Right = AttachedTransform.Right;
        Vector3 Up = AttachedTransform.Up;

        if (InputSystem.IsKeyDown(Keys.W))
            Movement = Forward * _MovementSpeed;
        if (InputSystem.IsKeyDown(Keys.A))
            Movement = Right * -_MovementSpeed;
        if (InputSystem.IsKeyDown(Keys.S))
            Movement = Forward * -_MovementSpeed;
        if (InputSystem.IsKeyDown(Keys.D))
            Movement = Right * _MovementSpeed;
    
        if (InputSystem.IsKeyDown(Keys.Up))
            Movement = Up * _MovementSpeed;
        if (InputSystem.IsKeyDown(Keys.Down))
            Movement = Up * -_MovementSpeed;
    
        if (InputSystem.IsKeyDown(Keys.Left))
            Rotaton = Vector3.Up * -_RotationSpeed;
        if (InputSystem.IsKeyDown(Keys.Right))
            Rotaton = Vector3.Up * _RotationSpeed;
    
    
        AttachedTransform.WorldPosition += Movement * DeltaTime;
        AttachedTransform.WorldRotation += Rotaton * DeltaTime;
        _Light.Transform.WorldPosition = AttachedTransform.WorldPosition;
    }
}
