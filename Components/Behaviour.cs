public abstract class Behaviour : Component
{
    protected Behaviour(GameObject attachedGameObject) : base(attachedGameObject)
    {

    }

    public virtual void Start() { }

    public virtual void Update(double DeltaTime) { }
}
