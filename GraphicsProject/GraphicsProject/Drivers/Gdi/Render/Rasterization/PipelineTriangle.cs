﻿using GraphicsProject.Common;
using GraphicsProject.Mathematics;
using GraphicsProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Drivers.Gdi.Render.Rasterization
{
    public partial class Pipeline<TVsIn, TPsIn>
    {
        /// <summary>
        /// Triangle primitive.
        /// </summary>
        internal struct PrimitiveTriangle
        {
            public TPsIn PsIn0;
            public TPsIn PsIn1;
            public TPsIn PsIn2;
            public Vector4F PositionScreen0;
            public Vector4F PositionScreen1;
            public Vector4F PositionScreen2;
        }

        #region // routines

        private static int TriangleClampX(int value, in Viewport viewport) => value.Clamp(viewport.X, viewport.X + viewport.Width);

        private static int TriangleClampY(int value, in Viewport viewport) => value.Clamp(viewport.Y, viewport.Y + viewport.Height);

        /// <summary>
        /// https://en.wikipedia.org/wiki/Barycentric_coordinate_system
        /// </summary>
        private static Vector3F TriangleGetBarycentric(Vector3F a, Vector3F b, Vector3F c, Vector3F p)
        {
            // compute barycentric coordinates (u, v, w) for point p with respect to triangle (a, b, c).
            var v0 = b - a;
            var v1 = c - a;
            var v2 = p - a;
            var d00 = v0 * v0;
            var d01 = v0 * v1;
            var d11 = v1 * v1;
            var d20 = v2 * v0;
            var d21 = v2 * v1;
            var denomInv = 1 / (d00 * d11 - d01 * d01);
            var v = (d11 * d20 - d01 * d21) * denomInv;
            var w = (d00 * d21 - d01 * d20) * denomInv;
            var u = 1 - v - w;
            return new Vector3F(u, v, w);
        }

        #endregion

        #region // vertex post-processing

        /// <summary>
        /// <see cref="VertexPostProcessing"/> for triangle.
        /// </summary>
        private void VertexPostProcessingTriangle(ref TPsIn psin0, ref TPsIn psin1, ref TPsIn psin2)
        {
            // vertex post processing + primitive assembly
            PrimitiveTriangle primitive;
            primitive.PsIn0 = psin0;
            primitive.PsIn1 = psin1;
            primitive.PsIn2 = psin2;
            VertexPostProcessing(ref primitive.PsIn0, out primitive.PositionScreen0);
            VertexPostProcessing(ref primitive.PsIn1, out primitive.PositionScreen1);
            VertexPostProcessing(ref primitive.PsIn2, out primitive.PositionScreen2);

            // rasterization stage
            RasterizeTriangle(primitive);
        }

        #endregion

        #region // rasterization

        /// <summary>
        /// Rasterize triangle.
        /// </summary>
        private void RasterizeTriangle(in PrimitiveTriangle primitive)
        {
            var vertex0 = primitive.PositionScreen0;
            var vertex1 = primitive.PositionScreen1;
            var vertex2 = primitive.PositionScreen2;

            // sort vertices by y (so that: vertex0.Y < vertex1.Y < vertex2.Y)
            if (vertex1.Y < vertex0.Y)
            {
                U.Swap(ref vertex0, ref vertex1);
            }
            if (vertex2.Y < vertex1.Y)
            {
                U.Swap(ref vertex1, ref vertex2);
            }
            if (vertex1.Y < vertex0.Y)
            {
                U.Swap(ref vertex0, ref vertex1);
            }

            const float error = 0.0001f;
            if (Math.Abs(vertex0.Y - vertex1.Y) < error)
            {
                // natural flat top

                if (vertex1.X < vertex0.X)
                {
                    U.Swap(ref vertex0, ref vertex1);
                }
                /*
                    (v0)--(v1)
                       \  /
                       (v2)
                */
                RasterizeTriangleFlatTop(primitive, vertex0, vertex1, vertex2);
            }
            else if (Math.Abs(vertex1.Y - vertex2.Y) < error)
            {
                // natural flat bottom

                if (vertex2.X < vertex1.X)
                {
                    U.Swap(ref vertex1, ref vertex2);
                }
                /*
                       (v0)
                       /  \
                    (v1)--(v2)
                */
                RasterizeTriangleFlatBottom(primitive, vertex1, vertex2, vertex0);
            }
            else
            {
                // regular triangle

                // find splitting vertex (and interpolate)
                var alpha = (vertex1.Y - vertex0.Y) / (vertex2.Y - vertex0.Y);
                var interpolant = vertex0.InterpolateLinear(vertex2, alpha);

                if (vertex1.X < interpolant.X)
                {
                    /*
                          (v0)
                          / |
                      (v1)-(i)
                          \ |
                          (v2)
                    */
                    RasterizeTriangleFlatBottom(primitive, vertex1, interpolant, vertex0);
                    RasterizeTriangleFlatTop(primitive, vertex1, interpolant, vertex2);
                }
                else
                {
                    /*
                        (v0)
                         | \
                        (i)-(v1)
                         | /
                        (v2)
                    */
                    RasterizeTriangleFlatBottom(primitive, interpolant, vertex1, vertex0);
                    RasterizeTriangleFlatTop(primitive, interpolant, vertex1, vertex2);
                }
            }
        }

        private void RasterizeTriangleFlatTop(in PrimitiveTriangle primitive, Vector4F vertexLeft, Vector4F vertexRight, Vector4F vertexBottom)
        {
            var height = vertexBottom.Y - vertexLeft.Y;
            var deltaLeft = (vertexBottom - vertexLeft) / height;
            var deltaRight = (vertexBottom - vertexRight) / height;
            RasterizeTriangleFlat(primitive, vertexLeft, vertexRight, deltaLeft, deltaRight, height);
        }

        private void RasterizeTriangleFlatBottom(in PrimitiveTriangle primitive, Vector4F vertexLeft, Vector4F vertexRight, Vector4F vertexTop)
        {
            var height = vertexLeft.Y - vertexTop.Y;
            var deltaLeft = (vertexLeft - vertexTop) / height;
            var deltaRight = (vertexRight - vertexTop) / height;
            RasterizeTriangleFlat(primitive, vertexTop, vertexTop, deltaLeft, deltaRight, height);
        }

        private void RasterizeTriangleFlat(in PrimitiveTriangle primitive, Vector4F edgeLeft, Vector4F edgeRight, Vector4F deltaLeft, Vector4F deltaRight, float height)
        {
            // get where we start and end vertically
            var yStart = TriangleClampY((int)Math.Round(edgeLeft.Y), RenderHost.CameraInfo.Viewport);
            var yEnd = TriangleClampY((int)Math.Round(edgeLeft.Y + height), RenderHost.CameraInfo.Viewport);

            // prestep (compensate for clamping + move to middle of the pixel)
            edgeLeft += deltaLeft * (yStart - edgeLeft.Y + 0.5f);
            edgeRight += deltaRight * (yStart - edgeRight.Y + 0.5f);

            // go vertically down
            for (var y = yStart; y < yEnd; y++)
            {
                // increment (interpolate) edges (going down)
                var eLeft = edgeLeft + deltaLeft * (y - yStart);
                var eRight = edgeRight + deltaRight * (y - yStart);

                // get scanline start and end
                var xStart = TriangleClampX((int)Math.Round(eLeft.X), RenderHost.CameraInfo.Viewport);
                var xEnd = TriangleClampX((int)Math.Round(eRight.X), RenderHost.CameraInfo.Viewport);

                // get precise scanline width
                var width = eRight.X - eLeft.X;
                // get scanline delta on each pixel going towards right
                var deltaScanline = (eRight - eLeft) / width;

                // create scanline (start from left)
                var scanline = eLeft;
                // prestep
                scanline += deltaScanline * (xStart - eLeft.X + 0.5f);

                // go horizontally (execute scanline)
                for (var x = xStart; x < xEnd; x++)
                {
                    var barycentric = TriangleGetBarycentric
                    (
                        primitive.PositionScreen0.ToVector3F(),
                        primitive.PositionScreen1.ToVector3F(),
                        primitive.PositionScreen2.ToVector3F(),
                        scanline.ToVector3F() // cut inverted w
                    );

                    // interpolate attributes
                    var interpolant = primitive.PsIn0.InterpolateBarycentric(primitive.PsIn1, primitive.PsIn2, barycentric);

                    // pass to pixel shader
                    StagePixelShader(x, y, interpolant);

                    // increment (interpolate) scanline (going right)
                    scanline += deltaScanline;
                }
            }
        }

        #endregion
    }
}
