using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using static OpenTK.Graphics.OpenGL.PrimitiveType;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpDX.Windows;

public class Render : GameWindow
{
    public Render(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        if (_Current != null)
            throw new Exception();

        _Scene = new Scene();
        _Scene.MainCamera.AddComponent<Initializer>();
        UpdateScreenSize();

        _Current = this;

        RecalculatePointSize();
    }

    public int WindowWidth => _WindowWidth;
    public int WindowHeight => _WindowHeight;
    public static Render Current => _Current;
    private static Render _Current;
    private int _WindowWidth;
    private int _WindowHeight;
    private Scene _Scene;
    private float _PointSize = 1;
    private EngineSettings _Settings = EngineSettings.GetInstance();

    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        UpdateScreenSize();
        RecalculatePointSize();
        base.OnResize(e);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        double FramesPerSeconds = 1 / args.Time;

        Title = args.Time.ToString();

        _Scene.InvokeUpdates(args.Time);

        GL.ClearColor(0f, 0f, 0f, 1);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.PointSize(_PointSize);

        GL.Begin(Points);

        float[][] Frame = _Scene.RenderFrame();
        DrawFrame(Frame);

        GL.End();

        SwapBuffers();
        base.OnRenderFrame(args);
    }

    private void DrawFrame(float[][] frame)
    {
        float RowsCount = frame.Length;
        float ColumnsCount = frame[0].Length;

        for (int i = 0; i < frame.Length; i++)
        {
            float[] Row = frame[i];

            float V = i / RowsCount;
            V = -(V - 0.5f) * 2;

            for (int j = 0; j < Row.Length; j += 3)
            {
                float U = j / ColumnsCount;
                U = (U - 0.5f) * 2;

                float R = Row[j];
                float G = Row[j + 1];
                float B = Row[j + 2];

                GL.Color3(R, G, B);
                GL.Vertex2(U, V);
            }
        }
    }

    private void UpdateScreenSize()
    {
        _WindowWidth = ClientSize.X;
        _WindowHeight = ClientSize.Y;
    }

    private void RecalculatePointSize() => 
        _PointSize = Math.Max(ClientSize.X, ClientSize.Y) / Math.Min(_Settings.RenderHeight, _Settings.RenderWidth);
}
