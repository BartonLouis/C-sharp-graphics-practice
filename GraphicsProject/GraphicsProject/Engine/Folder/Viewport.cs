using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Engine.Folder
{
    public class Viewport
    {

        #region // storage
        public int X { get; }       // X coordinate of the upper-left corner of the viewport on the render target surface
        public int Y { get; }       // Y coordinate of the upper-left corner of the viewport on the render target surface
        public int Width { get; }   // The Width of the viewport in pixels
        public int Height { get; }  // The Height of the viewport in pixels

        public double MinZ { get; }    // Minimum value of the clip volume
        public double MaxZ { get; }    // Maximum value of the clip volume

        #endregion

        #region // queries

        public System.Drawing.Point Location => new System.Drawing.Point(X, Y);
        public System.Drawing.Size Size => new System.Drawing.Size(Width, Height);
        public double AspectRation => (double)Width / Height;

        #endregion

        #region //ctor

        public Viewport(int x, int y, int width, int height, double minZ, double maxZ)
        {
            X= x;
            Y= y;
            Width= width;
            Height= height;
            MinZ= minZ;
            MaxZ= maxZ;
        }

        public Viewport(System.Drawing.Point location, System.Drawing.Size size, double minZ, double maxZ) :
           this(location.X, location.Y, size.Width, size.Height, minZ, maxZ)
        {
        }

        #endregion
    }
}
