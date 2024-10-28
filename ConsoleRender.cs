using System.Text;

public class ConsoleRender
{
    public ConsoleRender(int Width, int Height)
    {
        _Frame = new StringBuilder(Width * Height);
    }

    public ConsoleRender(int PixelsCount)
    {
        _Frame = new StringBuilder(PixelsCount);
    }

    public ConsoleRender()
    {

    }

    public int PixelsCount => _Frame.Length;
    private StringBuilder _Frame = new StringBuilder();
    private char[] _Gradient = new char[]
    {
        ' ',
        '.',
        '\'',
        ',',
        '"',
        ':',
        ';',
        'o',
        '0',
        'A',
        '#',
        'Q',
        '@',
    };

    public void Add(ConsoleRender other) => _Frame.Append(other._Frame);

    public void AddPixel() => AddPixel(SortedColor.White, 1);

    public void AddPixel(double Brightness) => AddPixel(SortedColor.White, Brightness);

    public void AddPixel(SortedColor color) => AddPixel(color, 1);

    public void AddPixel(SortedColor color, double Brightness)
    {
        if (Brightness < 0)
            Brightness = 0;

        if (Brightness > 1)
            Brightness = 1;

        int SymbolIndex = (int)Math.Ceiling((_Gradient.Length - 1) * Brightness);
        char Symbol = _Gradient[SymbolIndex];
        _Frame.Append($"\x1b[{(byte)color}m{Symbol}");
    }

    public void AddEmptyPixel() => _Frame.Append(' ');

    public void WrapLine() => _Frame.AppendLine();

    public void DrawInConsole() => DrawInConsole(true);

    public void DrawInConsole(bool ResetColor)
    {
        Console.CursorVisible = false;

        Console.SetCursorPosition(0, 0);
        Console.Write(_Frame.ToString());

        if (ResetColor)
            Console.ResetColor();
    }
}