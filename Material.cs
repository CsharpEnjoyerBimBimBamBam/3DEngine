using SharpDX.D3DCompiler;
using SharpDX.Direct2D1;
using System.Diagnostics;
using System.Drawing;

public class Material
{
    public Color Color { get; set; } = Color.White;
    public double Reflectivity 
    {
        get => _Reflectivity;
        set
        {
            if (value < 0 || value > 1) throw new ArgumentOutOfRangeException(nameof(value));
            _Reflectivity = value;
        }
    }
    public double Roughness
    {
        get => _Roughness;
        set
        {
            if (value < 0 || value > 1) throw new ArgumentOutOfRangeException(nameof(value));
            _Roughness = value;
        }
    }
    private double _Reflectivity = 1;
    private double _Roughness = 1;
}

public class Shader
{
    public Shader(string vertrexFunctionName, string pixelFunctionName, InputElement[] vertexFunctionInput)
    {
        if (string.IsNullOrEmpty(vertrexFunctionName))
            throw new ArgumentException(nameof(vertrexFunctionName));

        if (string.IsNullOrEmpty(pixelFunctionName))
            throw new ArgumentException(nameof(pixelFunctionName));

        if (vertexFunctionInput == null)
            throw new ArgumentNullException(nameof(vertexFunctionInput));

        _VertrexFunctionName = vertrexFunctionName;
        _PixelFunctionName = pixelFunctionName;
        _VertexFunctionInput = vertexFunctionInput;
    }

    public string VertrexFunctionName => _VertrexFunctionName;
    public string PixelFunctionName => _PixelFunctionName;
    public InputElement[] VertexFunctionInput => _VertexFunctionInput;
    private string _VertrexFunctionName = "";
    private string _PixelFunctionName = "";
    private InputElement[] _VertexFunctionInput = new InputElement[0];
}
