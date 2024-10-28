using OpenTK.Windowing.GraphicsLibraryFramework;

public class CubeMovement : Behaviour
{
    public CubeMovement(GameObject attachedGameObject) : base(attachedGameObject)
    {

    }

    private double _DeltaRotaion = 30;
    private bool _IsPaused = false;

    public override void Update(double DeltaTime)
    {
        if (InputSystem.IsKeyDown(Keys.Escape))
            _IsPaused = !_IsPaused;

        if (_IsPaused)
            return;

        AttachedTransform.WorldRotation += new Vector3(_DeltaRotaion, _DeltaRotaion, _DeltaRotaion) * DeltaTime;
    }
}
