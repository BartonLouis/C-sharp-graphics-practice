using GraphicsProject.Mathematics.Extensions;
using MathNet.Numerics.LinearAlgebra;

namespace GraphicsProject.Common.Camera
{
    public class CameraInfoCache : ICameraInfoCache
    {
        public Matrix<double> MatrixView { get; } 

        public Matrix<double> MatrixViewInverse { get; }

        public Matrix<double> MatrixProjection { get; }

        public Matrix<double> MatrixProjectionInverse { get; }

        public Matrix<double> MatrixViewport { get; }

        public Matrix<double> MatrixViewportInverse { get; }

        public Matrix<double> MatrixViewProjection { get; }

        public Matrix<double> MatrixViewProjectionInverse { get; }

        public Matrix<double> MatrixViewProjectionViewport { get; }

        public Matrix<double> MatrixViewProjectionViewportInverse { get; }

        public CameraInfoCache(ICameraInfo cameraInfo)
        {
            // raw

            // world space -> camera space
            MatrixView = MatrixEx.LookAtRH(cameraInfo.Position.ToVector3D(), cameraInfo.Target.ToVector3D(), cameraInfo.UpVector);
            MatrixViewInverse = MatrixView.Inverse();

            // camera space -> clip space
            MatrixProjection = cameraInfo.Projection.GetMatrixProjection();
            MatrixProjectionInverse = MatrixProjection.Inverse();

            // clip space -> screen space
            MatrixViewport = MatrixEx.Viewport(cameraInfo.Viewport);
            MatrixViewportInverse = MatrixViewport.Inverse();

            // multiplicatives

            // world space -> camera space -> clip space
            MatrixViewProjection = MatrixView * MatrixProjection;
            MatrixViewProjectionInverse = MatrixViewProjection.Inverse();

            // world space -> camera space -> clip space -> screen space
            MatrixViewProjectionViewport = MatrixViewProjection * MatrixViewport;
            MatrixViewProjectionViewportInverse = MatrixViewProjectionViewport.Inverse();
        }
    }
}
