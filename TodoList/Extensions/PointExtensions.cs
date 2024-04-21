using Microsoft.Xna.Framework;

namespace TodoList.Extensions;

public static class PointExtensions {
    public static Point Multiply(this Point p, double factor) {
        return new Point(p.X * (int)factor, p.Y * (int)factor);
    }
}