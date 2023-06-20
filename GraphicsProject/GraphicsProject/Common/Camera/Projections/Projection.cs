using GraphicsProject.Mathematics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Common.Camera.Projections
{
    /// <inheritdoc cref="IProjection"/>
    public abstract class Projection :
        IProjection
    {
        #region // storage

        /// <inheritdoc />
        public double NearPlane { get; }

        /// <inheritdoc />
        public double FarPlane { get; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        protected Projection(double nearPlane, double farPlane)
        {
            NearPlane = nearPlane;
            FarPlane = farPlane;
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        public abstract object Clone();

        /// <inheritdoc />
        public abstract Matrix4D GetMatrixProjection();

        /// <inheritdoc />
        public abstract IProjection GetAdjustedProjection(double aspectRatio);

        /// <inheritdoc />
        public abstract Ray3D GetMouseRay(ICameraInfo cameraInfo, Point3D mouseWorld);

        #endregion
    }
}
