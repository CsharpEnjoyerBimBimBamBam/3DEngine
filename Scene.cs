using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

public class Scene
{
    public Scene()
    {
        if (_Current != null)
            throw new Exception();

        _Current = this;
        Instantiate(_MainCamera);
    }

    public event Action FrameRenderStart;
    public static Scene Current => _Current;
    public Camera MainCamera => _MainCamera;
    public List<Light> Lights => _Lights;
    private List<GameObject> _SceneGameObjects = new List<GameObject>();
    private List<Renderer> _Renderers = new List<Renderer>();
    private List<Light> _Lights = new List<Light>();
    private static Scene _Current;
    private Camera _MainCamera = new Camera();
    private EngineSettings _Settings = EngineSettings.GetInstance();
    private const int _RowsCountInThread = 3;
    private const float _ByteMaxValue = byte.MaxValue;

    public float[][] RenderFrame()
    {
        FrameRenderStart?.Invoke();

        int ScreenWidth = _Settings.RenderWidth;
        int ScreenHeight = _Settings.RenderHeight;

        int ThreadsCount = (int)Math.Round(ScreenHeight / (double)_RowsCountInThread);

        List<Task<float[][]>> Tasks = new List<Task<float[][]>>();
        
        for (int i = 0; i < ScreenHeight; i += _RowsCountInThread)
            Tasks.Add(RenderRows(i, _RowsCountInThread, ScreenWidth, ScreenHeight));
        
        int LastRow = ThreadsCount * _RowsCountInThread;
        
        if (LastRow < ScreenHeight)
            Tasks.Add(RenderRows(LastRow, ScreenHeight - LastRow, ScreenWidth, ScreenHeight));
        
        Task.WhenAll(Tasks).Wait();

        int FullHeight = 0;

        Tasks.ForEach(X => FullHeight += X.Result.Length);

        float[][] Pixels = new float[FullHeight][];
        int Row = 0;

        for (int i = 0; i < Tasks.Count; i++)
        {
            float[][] Current = Tasks[i].Result;
            
            for (int j = 0; j < Current.Length; j++)
            {
                Pixels[Row] = Current[j];
                Row++;
            }
        }

        return Pixels;
    }

    public void Instantiate(GameObject gameObject)
    {
        _SceneGameObjects.Add(gameObject);
        UpdateGameObjects();
    }

    public void Instantiate(List<GameObject> gameObjects)
    {
        _SceneGameObjects.AddRange(gameObjects);
        UpdateGameObjects();
    }

    public void InvokeUpdates(double DeltaTime) => _SceneGameObjects.ForEach(X => X.InvokeUpdates(DeltaTime));

    private void UpdateGameObjects()
    {
        _Lights = _SceneGameObjects.FindAll(X => X is Light).Cast<Light>().ToList();
        _Renderers = new List<Renderer>();
        _SceneGameObjects.ForEach(gameObject => _Renderers.AddRange(gameObject.GetComponents<Renderer>()));
    }

    private async Task<float[][]> RenderRows(int StartRow, int Count, int Width, int Height)
    {
        return await Task.Factory.StartNew(() =>
        {
            float[][] Rows = new float[Count][];

            for (int i = 0; i < Count; i++)
            {
                Rows[i] = RenderRow(StartRow + i, Width, Height);
            }

            return Rows;
        }, TaskCreationOptions.PreferFairness);
    }

    private float[] RenderRow(int Number, int Width, int Height)
    {
        float[] RowPixels = new float[Width * 3];

        double Aspect = Render.Current.WindowWidth / (double)Render.Current.WindowHeight;

        double FieldOfViewRadians = _MainCamera.FieldOfView * MathConstant.DegreesToRadians;

        double NearPlaneHeight = Math.Tan(FieldOfViewRadians / 2);
        double NearPlaneWidth = NearPlaneHeight * Aspect;

        double V = (double)Number / Height;
        V = (V - 0.5) * 2;

        Vector3 CamearaDown = -_MainCamera.Transform.Up;
        Vector3 CameraForward = _MainCamera.Transform.Forward;
        Vector3 CameraRight = _MainCamera.Transform.Right;
        Vector3 CameraPosition = _MainCamera.Transform.WorldPosition;

        Ray ray = new Ray();
        ray.StartPoint = CameraPosition;

        int PixelIndex = 0;

        for (int i = 1; i <= Width; i++)
        {
            double U = i / (double)Width;
            U = (U - 0.5) * 2;

            ray.Direction = CameraForward + (CameraRight * U * NearPlaneWidth) + (CamearaDown * V * NearPlaneHeight);

            if (!TryGetPixelColor(ref ray, out Color Color))
            {
                RowPixels[PixelIndex] = 0;
                RowPixels[PixelIndex + 1] = 0;
                RowPixels[PixelIndex + 2] = 0;
                PixelIndex += 3;
                continue;
            }

            RowPixels[PixelIndex] = Color.R / _ByteMaxValue;
            RowPixels[PixelIndex + 1] = Color.G / _ByteMaxValue;
            RowPixels[PixelIndex + 2] = Color.B / _ByteMaxValue;

            PixelIndex += 3;
        }

        return RowPixels;
    }

    private bool TryGetPixelColor(ref Ray ray, out Color Color)
    {
        Color? NearestPixelColor = null;
        double MinDistance = 0;

        foreach (Renderer rendererComponent in _Renderers)
        {
            if (!rendererComponent.CheckForIntersection(ref ray, out Color color, out Vector3 intersectionPoint))
                continue;

            double Distance = Vector3.Distance(ref intersectionPoint, ref ray.StartPoint);

            if (NearestPixelColor == null)
            {
                NearestPixelColor = color;
                MinDistance = Distance;
                continue;
            }

            if (Distance < MinDistance)
            {
                NearestPixelColor = color;
                MinDistance = Distance;
            }
        }

        Color = default;

        if (NearestPixelColor == null)
            return false;

        Color = (Color)NearestPixelColor;
        
        return true;
    }
}
