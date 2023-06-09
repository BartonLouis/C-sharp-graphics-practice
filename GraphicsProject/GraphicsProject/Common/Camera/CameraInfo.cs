using GraphicsProject.Common.Camera.Projections;
using GraphicsProject.Utilities;
using MathNet.Spatial.Euclidean;

namespace GraphicsProject.Common.Camera
{
    public class CameraInfo : ICameraInfo
    {

        #region //storage
        public Point3D Position { get; }

        public Point3D Target { get; }

        public UnitVector3D UpVector { get; }

        public IProjection Projection { get; }

        public Viewport Viewport { get; }

        private ICameraInfoCache m_Cache;

        public ICameraInfoCache Cache => m_Cache ?? (m_Cache = new CameraInfoCache(this));

        #endregion

        #region //ctor

        public CameraInfo(in Point3D position, in Point3D target, in UnitVector3D upVector, in IProjection projection, in Viewport viewport)
        {
            Position = position;
            Target = target;
            UpVector = upVector;
            Projection = projection;
            Viewport = viewport;
        }
        #endregion

        #region //routines
        public object Clone()
        {
            return new CameraInfo(Position, Target, UpVector, Projection.Cloned(), Viewport);
        }

        #endregion
    }
}
