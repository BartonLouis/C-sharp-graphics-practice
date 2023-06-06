using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Mathematics.Extensions
{
    public static class Point2DEx
    {
        public static Point2D ToPoint2D(this System.Drawing.Point point) => new Point2D(point.X, point.Y);

        public static Point2D ToPoint2D(this System.Windows.Point point) => new Point2D(point.X, point.Y);
    }
}
