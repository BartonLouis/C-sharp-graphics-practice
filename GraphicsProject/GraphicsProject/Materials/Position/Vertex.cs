using MathNet.Spatial.Euclidean;
using System.Runtime.InteropServices;

namespace GraphicsProject.Materials.Position
{
    /// <inheritdoc cref="IVertex"/>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex :
        IVertex
    {
        #region // storage

        /// <inheritdoc />
        public Point3D Position { get; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Vertex(Point3D position)
        {
            Position = position;
        }

        #endregion
    }
}
