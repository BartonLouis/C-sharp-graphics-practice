﻿using GraphicsProject.Drivers.Gdi.Materials;
using GraphicsProject.Engine.Render;
using GraphicsProject.Materials;
using GraphicsProject.Mathematics;
using GraphicsProject.Mathematics.Extensions;
using GraphicsProject.Utilities;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace GraphicsProject.Drivers.Gdi.Render
{
    /// <summary>
    /// Gdi render host.
    /// </summary>
    public class RenderHost :
        Engine.Render.RenderHost
    {
        #region // storage

        /// <summary>
        /// Graphics retrieved from <see cref="IRenderHost.HostHandle"/>.
        /// </summary>
        private Graphics GraphicsHost { get; set; }

        /// <summary>
        /// Device context of <see cref="GraphicsHost"/>.
        /// </summary>
        private IntPtr GraphicsHostDeviceContext { get; set; }

        /// <summary>
        /// Double buffer wrapper.
        /// </summary>
        private BufferedGraphics BufferedGraphics { get; set; }

        /// <summary>
        /// Back buffer.
        /// </summary>
        public DirectBitmap BackBuffer { get; private set; }

        /// <summary>
        /// Shader library.
        /// </summary>
        public ShaderLibrary ShaderLibrary { get; private set; }

        /// <summary>
        /// Font for drawing text with <see cref="System.Drawing"/> objects.
        /// </summary>
        private Font FontConsolas12 { get; set; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RenderHost(IRenderHostSetup renderHostSetup) :
            base(renderHostSetup)
        {
            GraphicsHost = Graphics.FromHwnd(HostHandle);
            GraphicsHostDeviceContext = GraphicsHost.GetHdc();
            CreateSurface(HostInput.Size);
            CreateBuffers(BufferSize);
            ShaderLibrary = new ShaderLibrary(this);
            FontConsolas12 = new Font("Consolas", 12);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            FontConsolas12.Dispose();
            FontConsolas12 = default;

            ShaderLibrary.Dispose();
            ShaderLibrary = default;

            DisposeBuffers();
            DisposeSurface();

            GraphicsHost.ReleaseHdc(GraphicsHostDeviceContext);
            GraphicsHostDeviceContext = default;

            GraphicsHost.Dispose();
            GraphicsHost = default;

            base.Dispose();
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        protected override void ResizeHost(Size size)
        {
            base.ResizeHost(size);

            DisposeSurface();
            CreateSurface(size);
        }

        /// <inheritdoc />
        protected override void ResizeBuffers(Size size)
        {
            base.ResizeBuffers(size);

            DisposeBuffers();
            CreateBuffers(size);
        }

        private void CreateBuffers(Size size)
        {
            BackBuffer = new DirectBitmap(size);
        }

        private void DisposeBuffers()
        {
            BackBuffer.Dispose();
            BackBuffer = default;
        }

        private void CreateSurface(Size size)
        {
            BufferedGraphics = BufferedGraphicsManager.Current.Allocate(GraphicsHostDeviceContext, new Rectangle(Point.Empty, size));
            BufferedGraphics.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        private void DisposeSurface()
        {
            BufferedGraphics.Dispose();
            BufferedGraphics = default;
        }

        #endregion

        #region // render

        /// <inheritdoc />
        protected override void RenderInternal(IEnumerable<IModel> models)
        {
            // clear buffers
            BackBuffer.Clear(Color.Black);

            // render models
            RenderModels(models);

            // draw fps
            BackBuffer.Graphics.DrawString(FpsCounter.FpsString, FontConsolas12, Brushes.Red, 0, 0);

            // flush and swap buffers
            BufferedGraphics.Graphics.DrawImage(
                BackBuffer.Bitmap,
                new RectangleF(PointF.Empty, HostSize),
                new RectangleF(new PointF(-0.5f, -0.5f), BufferSize),
                GraphicsUnit.Pixel);
            BufferedGraphics.Render(GraphicsHostDeviceContext);
        }

        /// <summary>
        /// Draw models.
        /// </summary>
        private void RenderModels(IEnumerable<IModel> models)
        {
            foreach (var model in models)
            {
                using (var gfxModel = GfxModel.Factory(this, model))
                {
                    gfxModel.Render(GetMatrixForVertexShader(this, model.Space));
                }
            }
        }

        /// <summary>
        /// Get default matrix for vertex shader.
        /// </summary>
        private static Matrix4D GetMatrixForVertexShader(IRenderHost renderHost, Space space)
        {
            switch (space)
            {
                case Space.World:
                    return renderHost.CameraInfo.Cache.MatrixViewProjection;

                case Space.View:
                    return Matrix4D.Identity;

                case Space.Screen:
                    return renderHost.CameraInfo.Cache.MatrixViewportInverse;

                default:
                    throw new ArgumentOutOfRangeException(nameof(space), space, default);
            }
        }

        #endregion
    }
}
