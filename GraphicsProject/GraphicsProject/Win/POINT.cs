using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Win
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {

        public int X, Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        public POINT(System.Drawing.Point pt) : this(pt.X, pt.Y)
        { 
        }

        public static implicit operator System.Drawing.Point(POINT pt) => new System.Drawing.Point(pt.X, pt.Y);

        public static implicit operator POINT(System.Drawing.Point pt) => new POINT(pt.X, pt.Y);
    }
}
