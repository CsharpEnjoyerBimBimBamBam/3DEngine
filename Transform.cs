
public class Transform
{
    public Vector3 WorldPosition
    {
        get => _WorldPosition;
        set => _WorldPosition = value;
    }
    public Vector3 WorldRotation { get; set; }
    public Vector3 LocalPosition { get; set; }
    public Vector3 Size
    {
        get => _Size;
        set
        {
            if (value.X < 0 || value.Y < 0 || value.Z < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            _Size = value;
        }
    }
    public Vector3 Forward => Vector3.Forward * RotatonMatrix;
    public Vector3 Right => Vector3.Right * RotatonMatrix;
    public Vector3 Up => Vector3.Up * RotatonMatrix;
    public Transform? Parent
    {
        get => _Parent;
        set
        {
            if (_Parent == value)
                return;

            if (value == null)
            {
                _Parent = null;
                return;
            }

            _Parent._Childrens.Remove(this);
            _Parent = value;
            _Parent._Childrens.Add(this);
        }
    }
    public IReadOnlyList<Transform> Childrens => new List<Transform>(_Childrens);
    public Matrix3X3 LocalToWorldMatrix
    {
        get
        {
            Matrix3X3 rotationMatrix = RotatonMatrix;
            return Matrix3X3.GetTransformMatrix(GetRightAxis(rotationMatrix), GetForwardAxis(rotationMatrix), GetUpAxis(rotationMatrix));
        }
    }
    public Matrix3X3 WorldToLocalMatrix
    {
        get
        {
            Vector3 MinusRotation = -_WorldRotationInRadians;
            Matrix3X3 rotationMatrix = Matrix3X3.GetRotationMatrix(MinusRotation);
            return Matrix3X3.GetTransformMatrix(GetRightAxis(rotationMatrix), GetForwardAxis(rotationMatrix), GetUpAxis(rotationMatrix));
        }
    }
    public Matrix3X3 RotatonMatrix => Matrix3X3.GetRotationMatrix(_WorldRotationInRadians);
    private Vector3 _WorldPosition;
    private Vector3 _WorldRotationInRadians => WorldRotation * MathConstant.DegreesToRadians;
    private Transform? _Parent;
    private List<Transform> _Childrens = new List<Transform>();
    private Vector3 _Size = new Vector3(1, 1, 1);

    public Vector3 ToWorldPosition(Vector3 Position) => Position * LocalToWorldMatrix + WorldPosition;

    public Vector3 ToLocalPosition(Vector3 Position) => Position * WorldToLocalMatrix - WorldPosition;

    private Vector3 GetForwardAxis(Matrix3X3 RotatonMatrix) => Vector3.Forward * _Size.Z * RotatonMatrix;

    private Vector3 GetRightAxis(Matrix3X3 RotatonMatrix) => Vector3.Right * _Size.X * RotatonMatrix;

    private Vector3 GetUpAxis(Matrix3X3 RotatonMatrix) => Vector3.Up * _Size.Y * RotatonMatrix;
}