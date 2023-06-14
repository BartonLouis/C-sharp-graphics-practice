﻿using GraphicsProject.Mathematics.Extensions;
using GraphicsProject.Mathematics;
using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using GraphicsProject.Materials;
using System.Drawing;
using System.IO;

namespace GraphicsProject.Client
{
    /// <summary>
    /// Class for seeding some defaults.
    /// </summary>
    public static class Seed
    {
        #region // storage

        /// <summary>
        /// Collection of polylines representing edges of cube 1x1x1 at (0, 0, 0).
        /// </summary>
        private static readonly Vector3F[][] CubePolylines = new[]
        {
            // bottom
            new[]
            {
                new Vector3F(0, 0, 0),
                new Vector3F(1, 0, 0),
                new Vector3F(1, 1, 0),
                new Vector3F(0, 1, 0),
                new Vector3F(0, 0, 0),
            },
            // top
            new[]
            {
                new Vector3F(0, 0, 1),
                new Vector3F(1, 0, 1),
                new Vector3F(1, 1, 1),
                new Vector3F(0, 1, 1),
                new Vector3F(0, 0, 1),
            },
            // sides
            new[] { new Vector3F(0, 0, 0), new Vector3F(0, 0, 1), },
            new[] { new Vector3F(1, 0, 0), new Vector3F(1, 0, 1), },
            new[] { new Vector3F(1, 1, 0), new Vector3F(1, 1, 1), },
            new[] { new Vector3F(0, 1, 0), new Vector3F(0, 1, 1), },
        }.Select(polyline => MatrixEx.Translate(-0.5, -0.5, -0.5).Transform(polyline).ToArray()).ToArray();

        /// <summary>
        /// Point cloud of a bunny.
        /// </summary>
        private static readonly IPrimitive[] PointCloudBunny = new Func<IPrimitive[]>(() =>
        {
            // adjust for different coordinate system
            var matrix = MatrixEx.Scale(10) * MatrixEx.Rotate(QuaternionEx.AroundAxis(UnitVector3D.XAxis, Math.PI * 0.5));
            // point cloud source: http://graphics.stanford.edu/data/3Dscanrep/
            var vertices = StreamPointCloud_XYZ(@"..\..\Resources\bunny.xyz")
                .Select(vertex => new Materials.Position.Vertex(matrix.Transform(vertex)))
                .ToArray();
            return new IPrimitive[]
            {
                // construct point list (point cloud) primitive
                new Materials.Position.Primitive
                (
                    new PrimitiveBehaviour(Space.World),
                    PrimitiveTopology.PointList,
                    vertices,
                    Color.White
                )
            };
        })();

        #endregion

        /// <summary>
        /// Get period leftover ratio in given timespan.
        /// </summary>
        private static double GetTimeSpanPeriodRatio(TimeSpan duration, TimeSpan periodDuration)
        {
            return duration.TotalMilliseconds % periodDuration.TotalMilliseconds / periodDuration.TotalMilliseconds;
        }

        /// <summary>
        /// Get graphical primitives.
        /// </summary>
        public static IEnumerable<IPrimitive> GetPrimitives()
        {
            return GetPrimitivesAxisPoints()
                .Concat(GetPrimitivesPointCloud())
                ;
        }

        /// <summary>
        /// Get primitives showing difference between <see cref="Space.Screen"/> and <see cref="Space.View"/>.
        /// </summary>
        private static IEnumerable<IPrimitive> GetPrimitivesScreenViewLines()
        {
            // screen space
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.Screen),
                PrimitiveTopology.LineStrip,
                new[]
                {
                    new Materials.Position.Vertex(new Vector3F(3, 20, 0)),
                    new Materials.Position.Vertex(new Vector3F(140, 20, 0)),
                },
                Color.Gray
            );

            // view space
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.View),
                PrimitiveTopology.LineStrip,
                new[]
                {
                    new Materials.Position.Vertex(new Vector3F(-0.9f, -0.9f, 0)),
                    new Materials.Position.Vertex(new Vector3F(0.9f, -0.9f, 0)),
                },
                Color.Gray
            );
        }

        /// <summary>
        /// Get primitives representing world axis at world origin (each length of 1).
        /// </summary>
        private static IEnumerable<IPrimitive> GetPrimitivesWorldAxis()
        {
            // x axis
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.LineStrip,
                new[]
                {
                    new Materials.Position.Vertex(new Vector3F(0, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(1, 0, 0)),
                },
                Color.Red
            );

            // y axis
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.LineStrip,
                new[]
                {
                    new Materials.Position.Vertex(new Vector3F(0, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(0, 1, 0)),
                },
                Color.LawnGreen
            );

            // z axis
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.LineStrip,
                new[]
                {
                    new Materials.Position.Vertex(new Vector3F(0, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(0, 0, 1)),
                },
                Color.Blue
            );
        }

        /// <summary>
        /// Get some primitives to demonstrate hierarchical matrix multiplication.
        /// </summary>
        private static IEnumerable<IPrimitive> GetPrimitivesCubes()
        {
            var duration = new TimeSpan(DateTime.UtcNow.Ticks);

            // world space bigger cube
            var angle = GetTimeSpanPeriodRatio(duration, new TimeSpan(0, 0, 0, 5)) * Math.PI * 2;
            var matrixModel =
                MatrixEx.Scale(0.5) *
                MatrixEx.Rotate(UnitVector3D.Create(1, 0, 0), angle) *
                MatrixEx.Translate(1, 0, 0);

            foreach (var cubePolyline in CubePolylines)
            {
                yield return new Materials.Position.Primitive
                (
                    new PrimitiveBehaviour(Space.World),
                    PrimitiveTopology.LineStrip,
                    matrixModel.Transform(cubePolyline).Select(position => new Materials.Position.Vertex(position)).ToArray(),
                    Color.White
                );
            }

            // world space smaller cube
            angle = GetTimeSpanPeriodRatio(duration, new TimeSpan(0, 0, 0, 1)) * Math.PI * 2;
            matrixModel =
                MatrixEx.Scale(0.5) *
                MatrixEx.Rotate(UnitVector3D.Create(0, 1, 0), angle) *
                MatrixEx.Translate(0, 1, 0) *
                matrixModel;

            foreach (var cubePolyline in CubePolylines)
            {
                yield return new Materials.Position.Primitive
                (
                    new PrimitiveBehaviour(Space.World),
                    PrimitiveTopology.LineStrip,
                    matrixModel.Transform(cubePolyline).Select(position => new Materials.Position.Vertex(position)).ToArray(),
                    Color.Yellow
                );
            }
        }

        /// <summary>
        /// Get some point primitives to represent world axis.
        /// </summary>
        private static IEnumerable<IPrimitive> GetPrimitivesAxisPoints()
        {
            const int freq = 100;

            // x axis
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.PointList,
                Enumerable.Range(0, freq).Select(i => new Materials.Position.Vertex(new Vector3F((float)i / freq, 0, 0))).ToArray(),
                Color.Red
            );

            // y axis
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.PointList,
                Enumerable.Range(0, freq).Select(i => new Materials.Position.Vertex(new Vector3F(0, (float)i / freq, 0))).ToArray(),
                Color.LawnGreen
            );

            // z axis
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.PointList,
                Enumerable.Range(0, freq).Select(i => new Materials.Position.Vertex(new Vector3F(0, 0, (float)i / freq))).ToArray(),
                Color.Blue
            );
        }

        /// <summary>
        /// Get some point cloud primitives.
        /// </summary>
        private static IEnumerable<IPrimitive> GetPrimitivesPointCloud()
        {
            return PointCloudBunny;
        }

        /// <summary>
        /// Read *.xyz point cloud file.
        /// </summary>
        public static IEnumerable<Vector3F> StreamPointCloud_XYZ(string filePath)
        {
            using (var inputStream = new FileStream(filePath, FileMode.Open))
            {
                var pointCount = inputStream.Length / (4 * 3); // 4 bytes per float, 3 floats per vertex
                using (var reader = new BinaryReader(inputStream))
                {
                    for (var i = 0L; i < pointCount; i++)
                    {
                        yield return new Vector3F(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                }
            }
        }
    }
}
