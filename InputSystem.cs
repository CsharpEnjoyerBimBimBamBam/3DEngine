using OpenTK.Windowing.GraphicsLibraryFramework;

public class InputSystem
{
    public static bool IsKeyPressed(Keys key) => Render.Current.KeyboardState.IsKeyPressed(key);

    public static bool IsKeyDown(Keys key) => Render.Current.KeyboardState.IsKeyDown(key);

    public static bool IsKeyUp(Keys key) => Render.Current.KeyboardState.IsKeyReleased(key);
}
