﻿using System;
using System.Collections.Generic;
using GraphicsProject.Drivers.Gdi.Materials;
using GraphicsProject.Materials;
using GraphicsProject.Mathematics.Extensions;

using GraphicsProject.Common;
using GraphicsProject.Utilities;
using GraphicsProject.Mathematics;
using IVertex = GraphicsProject.Drivers.Gdi.Materials.IVertex;
using GraphicsProject.Drivers.Gdi.Render.Rasterization;

namespace GraphicsProject.Drivers.Gdi.Render
{
    /// <inheritdoc cref="IPipeline{TVsIn,TPsIn}"/>
    public unsafe partial class Pipeline<TVsIn, TPsIn> :
        IPipeline<TVsIn, TPsIn>
        where TVsIn : unmanaged
        where TPsIn : unmanaged, IPsIn<TPsIn>
    {
        #region // storage

        /// <summary>
        /// Ongoing shader.
        /// </summary>
        private IShader<TVsIn, TPsIn> Shader { get; set; }

        /// <summary>
        /// Ongoing render host.
        /// </summary>
        private RenderHost RenderHost => Shader.RenderHost;

        #endregion

        #region // ctor

        /// <summary />
        public Pipeline(IShader<TVsIn, TPsIn> shader)
        {
            Shader = shader;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Shader = default;
        }

        #endregion

        #region // public access

        /// <inheritdoc />
        public void Render(IBufferBinding<TVsIn> bufferBinding, int countVertices, PrimitiveTopology primitiveTopology)
        {
            void Point(uint index0)
            {
                var vsin = stackalloc TVsIn[1];
                *vsin = bufferBinding.GetVsIn(index0);
                StageVertexShader(vsin, 1, primitiveTopology);
            }
            void Line(uint index0, uint index1)
            {
                var vsin = stackalloc TVsIn[2];
                *vsin = bufferBinding.GetVsIn(index0);
                *(vsin + 1) = bufferBinding.GetVsIn(index1);
                StageVertexShader(vsin, 2, primitiveTopology);
            }
            void Triangle(uint index0, uint index1, uint index2)
            {
                var vsin = stackalloc TVsIn[3];
                *vsin = bufferBinding.GetVsIn(index0);
                *(vsin + 1) = bufferBinding.GetVsIn(index1);
                *(vsin + 2) = bufferBinding.GetVsIn(index2);
                StageVertexShader(vsin, 3, primitiveTopology);
            }

            TraversePrimitives(primitiveTopology, countVertices, Point, Line, Triangle);
        }

        #endregion

        #region // routines

        /// <summary>
        /// Delegate for processing point given one index.
        /// </summary>
        private delegate void ProcessPointDelegate(uint index0);

        /// <summary>
        /// Delegate for processing line given two indices.
        /// </summary>
        private delegate void ProcessLineDelegate(uint index0, uint index1);

        /// <summary>
        /// Delegate for processing triangle given three indices.
        /// </summary>
        private delegate void ProcessTriangleDelegate(uint index0, uint index1, uint index2);

        /// <summary>
        /// Construct primitive definitions and pass to provided processors.
        /// </summary>
        private static void TraversePrimitives(PrimitiveTopology primitiveTopology, int countVertices,
            ProcessPointDelegate processPoint, ProcessLineDelegate processLine, ProcessTriangleDelegate processTriangle)
        {
            switch (primitiveTopology)
            {
                case PrimitiveTopology.PointList:
                    for (var i = 0u; i < countVertices; i++)
                    {
                        processPoint(i);
                    }
                    break;

                case PrimitiveTopology.LineList:
                    for (var i = 0u; i < countVertices; i += 2)
                    {
                        processLine(i, i + 1);
                    }
                    break;

                case PrimitiveTopology.LineStrip:
                    for (var i = 0u; i < countVertices - 1; i++)
                    {
                        processLine(i, i + 1);
                    }
                    break;

                case PrimitiveTopology.TriangleList:
                    for (var i = 0u; i < countVertices; i += 3)
                    {
                        processTriangle(i, i + 1, i + 2);
                    }
                    break;

                case PrimitiveTopology.TriangleStrip:
                    var flip = false;
                    for (var i = 0u; i < countVertices - 2; i++)
                    {
                        if (flip)
                        {
                            processTriangle(i, i + 2, i + 1);
                        }
                        else
                        {
                            processTriangle(i, i + 1, i + 2);
                        }
                        flip = !flip;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(primitiveTopology), primitiveTopology, default);
            }
        }

        #endregion

        #region // stages

        /// <summary>
        /// Vertex shader stage.
        /// </summary>
        private void StageVertexShader(TVsIn* vsin, int count, PrimitiveTopology primitiveTopology)
        {
            var psin = stackalloc TPsIn[count];
            for (var i = 0; i < count; i++)
            {
                Shader.VertexShader(*(vsin + i), out *(psin + i));
            }
            StageVertexPostProcessing(psin, count, primitiveTopology);
        }

        /// <summary>
        /// Vertex post-processing stage.
        /// </summary>
        private void StageVertexPostProcessing(TPsIn* psin, int count, PrimitiveTopology primitiveTopology)
        {
            void Point(uint index0)
            {
                VertexPostProcessingPoint(ref *(psin + index0));
            }
            void Line(uint index0, uint index1)
            {
                VertexPostProcessingLine(ref *(psin + index0), ref *(psin + index1));
            }
            void Triangle(uint index0, uint index1, uint index2)
            {
                VertexPostProcessingTriangle(ref *(psin + index0), ref *(psin + index1), ref *(psin + index2));
            }

            TraversePrimitives(primitiveTopology, count, Point, Line, Triangle);
        }

        /// <summary>
        /// Pixel (fragment) shader stage.
        /// </summary>
        private void StagePixelShader(int x, int y, in TPsIn psin)
        {
            // sanity check
            if (x < 0 || y < 0 || x >= RenderHost.BackBuffer.Size.Width || y >= RenderHost.BackBuffer.Size.Height)
            {
                return;
            }

            var success = Shader.PixelShader(psin, out var psout);

            // check for discard
            if (!success)
            {
                return;
            }

            StageOutputMerger(x, y, psout);
        }

        /// <summary>
        /// Output merger stage.
        /// </summary>
        private void StageOutputMerger(int x, int y, Vector4F psout)
        {
            RenderHost.BackBuffer.SetValue(x, y, psout.ToArgb());
        }

        #endregion

        #region // vertex post-processing

        /// <summary>
        /// Post-process single vertex.
        /// </summary>
        private void VertexPostProcessing(ref TPsIn psin, out Vector4F positionScreen)
        {
            // perspective division (clip space to NDC)
            var ndc = psin.Position / psin.Position.W;
            psin = psin.ReplacePosition(ndc);

            // NDC to screen space
            positionScreen = RenderHost.CameraInfo.Cache.MatrixViewport.Transform(psin.Position);
        }

        #endregion
    }
}
