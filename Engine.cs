using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using System.Drawing;

public class Engine
{
    public static void Main()
    {
        new Render(new GameWindowSettings { UpdateFrequency = 75 }, new NativeWindowSettings 
        { 
            APIVersion = new Version(3, 3), 
            API = ContextAPI.OpenGL,
            Profile = ContextProfile.Compatability,
            ClientSize = new Vector2i(1980, 1020)
        }).Run();
    }
}
