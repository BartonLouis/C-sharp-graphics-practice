﻿using MathNet.Spatial.Euclidean;

namespace GraphicsProject.Mathematics.Extensions
{
    public static class Vector4DEx
    {
        #region // from

        public static Vector4D ToVector4D(this in Vector2D value, double z, double w)
        {
            return new Vector4D(value.X, value.Y, z, w);
        }

        public static Vector4D ToVector4D(this in Vector3D value, double w)
        {
            return new Vector4D(value.X, value.Y, value.Z, w);
        }

        public static Vector4D ToVector4D(this in UnitVector3D value, double w)
        {
            return new Vector4D(value.X, value.Y, value.Z, w);
        }

        public static Vector4D ToVector4D(this in Point2D value, double z, double w)
        {
            return new Vector4D(value.X, value.Y, z, w);
        }

        public static Vector4D ToVector4D(this in Point3D value, double w)
        {
            return new Vector4D(value.X, value.Y, value.Z, w);
        }

        public static Vector4D ToVector4D(this in Vector2F value, double z, double w)
        {
            return new Vector4D(value.X, value.Y, z, w);
        }

        public static Vector4D ToVector4D(this in Vector3F value, double w)
        {
            return new Vector4D(value.X, value.Y, value.Z, w);
        }

        #endregion

        #region // to

        public static Vector3D ToVector3DNormalized(this in Vector4D value)
        {
            return new Vector3D(value.X / value.W, value.Y / value.W, value.Z / value.W);
        }

        public static Point3D ToPoint3DNormalized(this in Vector4D value)
        {
            return new Point3D(value.X / value.W, value.Y / value.W, value.Z / value.W);
        }

        public static Vector3D ToVector3D(this in Vector4D value)
        {
            return new Vector3D(value.X, value.Y, value.Z);
        }

        public static Vector2D ToVector2D(this in Vector4D value)
        {
            return new Vector2D(value.X, value.Y);
        }

        public static Point3D ToPoint3D(this in Vector4D value)
        {
            return new Point3D(value.X, value.Y, value.Z);
        }

        public static Point2D ToPoint2D(this in Vector4D value)
        {
            return new Point2D(value.X, value.Y);
        }

        #endregion
    }
}
