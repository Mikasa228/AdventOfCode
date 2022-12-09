namespace NinthDay;

internal class RopeRenderer
{
    private readonly List<PointRenderer> renderers = new();
    internal RopeRenderer(List<Point> points)
    {
        foreach (var point in points)
        {
            renderers.Add(new PointRenderer(point));
        }
    }
    internal void RenderRope()
    {
        for (int i = renderers.Count - 1; i >= 0; i--)
        {
            var identifier = i switch
            {
                9 => "8",
                0 => "э",
                _ => "о"
            };
            renderers[i].Render(identifier);
        }
    }
}

internal class PointRenderer
{
    private readonly Point _coordinate;

    private int _previousX = 0;
    private int _previousY = 0;

    private const int _offsetX = 105;
    private const int _offsetY = 280;

    private const double coeffX = 52d / 156d;
    private const double coeffY = 196d / 280d;

    internal PointRenderer(Point coordinate)
    {
        _coordinate = coordinate;
    }
    internal void Render(string identifier)
    {
        Console.CursorVisible = false;
        Console.SetCursorPosition(ModifyY(_previousY), ModifyX(_previousX));
        var placeholder = identifier == "8" ? "." : " ";
        Console.Write(placeholder);
        Console.SetCursorPosition(ModifyY(_coordinate.Y), ModifyX(_coordinate.X));
        Console.Write(identifier);
        _previousX = _coordinate.X;
        _previousY = _coordinate.Y;
    }

    private static int ModifyX(int x) => (int)((x + _offsetX) * coeffX) > 51 ? 51 : (int)((x + _offsetX) * coeffX);
    private static int ModifyY(int y) => (int)((y + _offsetY) * coeffY) > 195 ? 195 : (int)((y + _offsetY) * coeffY);
}
