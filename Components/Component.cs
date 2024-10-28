public abstract class Component
{
    public Component(GameObject attachedGameObject)
    {
        if (attachedGameObject == null)
            throw new ArgumentNullException(nameof(attachedGameObject));

        _AttachedGameObject = attachedGameObject;
    }

    public Transform AttachedTransform => AttachedGameObject.Transform;
    public GameObject AttachedGameObject => _AttachedGameObject;
    private GameObject _AttachedGameObject;
}
