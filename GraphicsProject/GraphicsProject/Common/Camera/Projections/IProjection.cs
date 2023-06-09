using MathNet.Numerics.LinearAlgebra;
using System;
namespace GraphicsProject.Common.Camera.Projections
{
    public interface IProjection : ICloneable
    {
        double NearPlane { get; }
        double FarPlane { get; }
        Matrix<double> GetMatrixProjection();
        IProjection GetAdjustedProjection(double aspectRatio);
    }
}
