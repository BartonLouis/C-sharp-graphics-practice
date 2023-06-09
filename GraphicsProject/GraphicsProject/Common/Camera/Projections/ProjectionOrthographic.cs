using GraphicsProject.Mathematics.Extensions;
using MathNet.Numerics.LinearAlgebra;

namespace GraphicsProject.Common.Camera.Projections
{
    public class ProjectionOrthographic : Projection, IProjectionOrthographic
    {
        #region //storage
        public double FieldWidth { get; }

        public double FieldHeight { get; }

        #endregion

        #region //ctor

        public ProjectionOrthographic(double nearPlane, double farPlane, double fieldWidth, double fieldHeight) : base(nearPlane, farPlane)
        {
            FieldWidth = fieldWidth;
            FieldHeight = fieldHeight;
        }

        #endregion

        #region // routines

        public static IProjectionOrthographic FromDistance(double nearPlane, double farPlane, double cameraPositionToTargetDistance, double aspectRatio)
        {
            return new ProjectionOrthographic(nearPlane, farPlane, cameraPositionToTargetDistance * aspectRatio, cameraPositionToTargetDistance);
        }

        public override object Clone()
        {
            return new ProjectionOrthographic(NearPlane, FarPlane, FieldWidth, FieldHeight);
        }

        public override IProjection GetAdjustedProjection(double aspectRatio)
        {
            return new ProjectionOrthographic(NearPlane, FarPlane, FieldWidth * aspectRatio, FieldHeight);
        }

        public override Matrix<double> GetMatrixProjection()
        {
            return MatrixEx.OrthoRH(FieldWidth, FieldHeight, NearPlane, FarPlane);
        }

        #endregion
    }
}
