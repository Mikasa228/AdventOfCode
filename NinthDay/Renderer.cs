namespace NinthDay;

internal class RopeRenderer
{
    private readonly List<PointRenderer> renderers = new();
    readonly object _locker;

    internal RopeRenderer(List<Point> points, object locker)
    {
        _locker = locker;

        Console.CursorVisible = false;
        for (int i = 0; i < points.Count; i++)
        {
            var identifier = i switch
            {
                9 => "8",
                0 => "э",
                _ => "о"
            };
            renderers.Add(new PointRenderer(points[i], identifier));
        }
    }

    internal void RenderRope()
    {
        lock (_locker)
        {
            for (int i = renderers.Count - 2; i > 0; i--) renderers[i].CleanUp();
            renderers[9].CleanUp();
            renderers[0].CleanUp();

            for (int i = renderers.Count - 2; i > 0; i--) renderers[i].Render();
            renderers[9].Render();
            renderers[0].Render();
        }
    }
}

internal class PointRenderer
{
    private readonly Point _coordinate;

    private string _identifier;

    private int _previousX = 0;
    private int _previousY = 0;

    private const int _offsetX = 103;
    private const int _offsetY = 274;

    private static readonly double _coeffX = 0;
    private static readonly double _coeffY = 0;

    static PointRenderer()
    {
        _coeffX = (Console.WindowHeight - 1) / 158d;
        _coeffY = (Console.WindowWidth - 1) / 280d;
    }

    internal PointRenderer(Point coordinate, string identifier)
    {
        _coordinate = coordinate;
        _identifier = identifier;
    }

    internal void CleanUp()
    {
        Console.SetCursorPosition(ModifyY(_previousY), ModifyX(_previousX));
        var placeholder = _identifier == "8" ? "." : " ";
        Console.Write(placeholder);
    }

    internal void Render()
    {
        if (_identifier == "э" && _coordinate.Y < _previousY) _identifier = "c";
        if (_identifier == "c" && _coordinate.Y > _previousY) _identifier = "э";

        Console.SetCursorPosition(ModifyY(_coordinate.Y), ModifyX(_coordinate.X));
        Console.Write(_identifier);
        _previousX = _coordinate.X;
        _previousY = _coordinate.Y;
    }

    private static int ModifyX(int x) => (int)((x + _offsetX) * _coeffX);
    private static int ModifyY(int y) => (int)((y + _offsetY) * _coeffY);
}
