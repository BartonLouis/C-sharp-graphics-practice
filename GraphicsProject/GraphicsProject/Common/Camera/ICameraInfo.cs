using GraphicsProject.Common.Camera.Projections;
using MathNet.Spatial.Euclidean;
using System;

namespace GraphicsProject.Common.Camera
{
    public interface ICameraInfo : ICloneable
    {
        Point3D Position { get; }
        Point3D Target { get; }
        UnitVector3D UpVector { get; }
        IProjection Projection { get; }
        Viewport Viewport { get; }
        ICameraInfoCache Cache { get; }

    }
}
