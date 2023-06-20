using GraphicsProject.Mathematics;
using GraphicsProject.Mathematics.Extensions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Common.Camera.Projections
{
    /// <inheritdoc cref="IProjectionPerspective"/>
    public class ProjectionPerspective :
        Projection,
        IProjectionPerspective
    {
        #region // routines

        /// <inheritdoc />
        public double FieldOfViewY { get; }

        /// <inheritdoc />
        public double AspectRatio { get; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        public ProjectionPerspective(double nearPlane, double farPlane, double fieldOfViewY, double aspectRatio) :
            base(nearPlane, farPlane)
        {
            FieldOfViewY = fieldOfViewY;
            AspectRatio = aspectRatio;
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        public override object Clone()
        {
            return new ProjectionPerspective(NearPlane, FarPlane, FieldOfViewY, AspectRatio);
        }

        /// <inheritdoc />
        public override Matrix4D GetMatrixProjection()
        {
            return Matrix4DEx.PerspectiveFovRH(FieldOfViewY, AspectRatio, NearPlane, FarPlane);
        }

        /// <inheritdoc />
        public override IProjection GetAdjustedProjection(double aspectRatio)
        {
            return new ProjectionPerspective(NearPlane, FarPlane, FieldOfViewY, aspectRatio);
        }

        /// <inheritdoc />
        public override Ray3D GetMouseRay(ICameraInfo cameraInfo, Point3D mouseWorld)
        {
            return new Ray3D(mouseWorld, (mouseWorld - cameraInfo.Position).Normalize());
        }

        #endregion
    }
}
