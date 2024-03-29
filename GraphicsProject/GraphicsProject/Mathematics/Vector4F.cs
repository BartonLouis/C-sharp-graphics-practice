﻿using GraphicsProject.Mathematics.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Mathematics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Vector4F :
       IInterpolate<Vector4F>
    {
        #region // static

        public static readonly Vector4F Zero = new Vector4F(0, 0, 0, 0);

        #endregion

        #region // storage

        public float X { get; }

        public float Y { get; }

        public float Z { get; }

        public float W { get; }

        #endregion

        #region // ctor

        public Vector4F(float x, float y, float z, float w) => (X, Y, Z, W) = (x, y, z, w);

        #endregion

        #region // operators

        public static Vector4F operator +(in Vector4F left, in Vector4F right)
        {
            return new Vector4F
            (
                left.X + right.X,
                left.Y + right.Y,
                left.Z + right.Z,
                left.W + right.W
            );
        }

        public static Vector4F operator -(in Vector4F left, in Vector4F right)
        {
            return new Vector4F
            (
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z,
                left.W - right.W
            );
        }

        public static Vector4F operator *(in Vector4F left, float right)
        {
            return new Vector4F
            (
                left.X * right,
                left.Y * right,
                left.Z * right,
                left.W * right
            );
        }

        public static Vector4F operator /(in Vector4F left, float right)
        {
            return new Vector4F
            (
                left.X / right,
                left.Y / right,
                left.Z / right,
                left.W / right
            );
        }

        #endregion

        #region // interpolation

        public Vector4F InterpolateLinear(in Vector4F other, float alpha)
        {
            return new Vector4F
            (
                X.InterpolateLinear(other.X, alpha),
                Y.InterpolateLinear(other.Y, alpha),
                Z.InterpolateLinear(other.Z, alpha),
                W.InterpolateLinear(other.W, alpha)
            );
        }

        #endregion

        #region // routines

        public override string ToString() => $"{X:0.000000}, {Y:0.000000}, {Z:0.000000}, {W:0.000000}";

        #endregion
    }
}
