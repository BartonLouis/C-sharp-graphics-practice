using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Common.Camera.Projections
{
    public abstract class Projection : IProjection
    {
        #region //storage
        public double NearPlane { get; }

        public double FarPlane { get; }

        #endregion

        #region //ctor

        protected Projection(double nearPlane, double farPlane)
        {
            NearPlane = nearPlane;
            FarPlane = farPlane;
        }

        #endregion

        #region //routines

        public abstract Matrix<double> GetMatrixProjection();

        public abstract IProjection GetAdjustedProjection(double aspectRatio);

        public abstract object Clone();

        #endregion
    }
}
