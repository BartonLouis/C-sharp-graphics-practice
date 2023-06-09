using GraphicsProject.Common;
using GraphicsProject.Common.Render;
using GraphicsProject.Mathematics;
using GraphicsProject.Mathematics.Extensions;
using GraphicsProject.Utilities;
using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace GraphicsProject.Drivers.Gdi.Render
{
    public class RenderHost :
         Common.Render.RenderHost
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
        private DirectBitmap BackBuffer { get; set; }

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
            CreateSurface(HostSize);
            CreateBuffers(BufferSize);
            FontConsolas12 = new Font("Consolas", 12);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            FontConsolas12.Dispose();
            FontConsolas12 = default;

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
        protected override void RenderInternal()
        {
            var graphics = BackBuffer.Graphics;

            // Clear background
            graphics.Clear(Color.Black);

            DrawWorldAxis();
            DrawGeometry();

            // Debug Text
            graphics.DrawString($"{FpsCounter.FpsString}", FontConsolas12, Brushes.Red, 0, 0);
            graphics.DrawString($"Buffer   = {BufferSize.Width}, {BufferSize.Height}", FontConsolas12, Brushes.Cyan, 0, 16);
            graphics.DrawString($"Viewport = {HostSize.Width}, {HostSize.Height}", FontConsolas12, Brushes.Cyan, 0, 32);


            // Flush and Swap Buffers
            BufferedGraphics.Graphics.DrawImage(BackBuffer.Bitmap, new RectangleF(PointF.Empty, HostSize), new RectangleF(new PointF(-0.5f, -0.5f), BufferSize), GraphicsUnit.Pixel);
            BufferedGraphics.Render(GraphicsHostDeviceContext);
        }

        private void DrawLineScreenSpace(Graphics graphics, Pen pen, Point3D startScreen, Point3D endScreen)
        {
            graphics.DrawLine(pen, (float)startScreen.X, (float)startScreen.Y, (float)endScreen.X, (float)endScreen.Y);
        }

        private void DrawLineViewSpace(Graphics graphics, Pen pen, Point3D startView, Point3D endView)
        {
            Point3D startScreen = TransformFromViewSpaceToScreenSpace(CameraInfo.Viewport, startView);
            Point3D endScreen = TransformFromViewSpaceToScreenSpace(CameraInfo.Viewport, endView);
            DrawLineScreenSpace(graphics, pen, startScreen, endScreen);
        }

        private static Point3D TransformFromViewSpaceToScreenSpace(Viewport viewport, Point3D point)
        {
            return new Point3D
                (
                (point.X + 1) * 0.5 * viewport.Width + viewport.X,
                (1 - point.Y) * 0.5 * viewport.Height + viewport.Y,
                0
                );
        }

        private void DrawPolyLine(IEnumerable<Point3D> points, Space space, Pen pen)
        {
            switch (space)
            {
                case Space.World:

                    DrawPolyLineScreenSpace((CameraInfo.Cache.MatrixViewProjectionViewport).Transform(points), pen);
                    break;
                    throw new NotSupportedException();
                case Space.View:
                    DrawPolyLineScreenSpace(CameraInfo.Cache.MatrixViewport.Transform(points), pen);
                    break;
                case Space.Screen:
                    DrawPolyLineScreenSpace(points, pen);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(space), space, null);

            }
        }

        private void DrawPolyLineScreenSpace(IEnumerable<Point3D> pointsScreen, Pen pen)
        {
            var from = default(Point3D?);
            foreach (var pointScreen in pointsScreen)
            {
                if (from.HasValue)
                {
                    BackBuffer.Graphics.DrawLine(pen, (float)from.Value.X, (float)from.Value.Y, (float)pointScreen.X, (float)pointScreen.Y);
                }
                from = pointScreen;
            }
        }

        private void DrawWorldAxis()
        {
            DrawPolyLine(new[] { new Point3D(0, 0, 0), new Point3D(1, 0, 0), new Point3D(0.8, 0.1, 0), new Point3D(0.8, -0.1, 0), new Point3D(1, 0, 0) }, Space.World, Pens.Red);
            DrawPolyLine(new[] { new Point3D(0, 0, 0), new Point3D(0, 1, 0), new Point3D(0.1, 0.8, 0), new Point3D(-0.1, 0.8, 0), new Point3D(0, 1, 0) }, Space.World, Pens.LawnGreen);
            DrawPolyLine(new[] { new Point3D(0, 0, 0), new Point3D(0, 0, 1), new Point3D(0, 0.1, 0.8), new Point3D(0, -0.1, 0.8), new Point3D(0, 0, 1) }, Space.World, Pens.Blue);
        }

        private static readonly Point3D[][] CubePolyLines = new[]
        {
            new[]
            {
                new Point3D(0, 0, 0),
                new Point3D(0, 1, 0),
                new Point3D(1, 1, 0),
                new Point3D(1, 0, 0),
                new Point3D(0, 0, 0)
            },
            new[]
            {
                new Point3D(0, 0, 1),
                new Point3D(0, 1, 1),
                new Point3D(1, 1, 1),
                new Point3D(1, 0, 1),
                new Point3D(0, 0, 1)
            },
            new[]{ new Point3D(0, 0, 0), new Point3D(0, 0, 1)},
            new[]{ new Point3D(0, 1, 0), new Point3D(0, 1, 1)},
            new[]{ new Point3D(1, 1, 0), new Point3D(1, 1, 1)},
            new[]{ new Point3D(1, 0, 0), new Point3D(1, 0, 1)}
        }.Select(cubePolyLine => MatrixEx.Translate(-0.5, -0.5, -0.5).Transform(cubePolyLine).ToArray()).ToArray();

        private void DrawGeometry()
        {
            var angle = GetDeltaTime(new TimeSpan(0, 0, 3)) * Math.PI * 2;

            var matrixTransform =
                MatrixEx.Scale(0.5) *
                MatrixEx.Rotate(UnitVector3D.Create(0, 0, 1), angle) *
                MatrixEx.Translate(.5, .5, .5);

            foreach (Point3D[] polyLine in CubePolyLines)
            {
                DrawPolyLine(matrixTransform.Transform(polyLine), Space.World, Pens.Cyan);
            }

            matrixTransform =
                MatrixEx.Scale(0.5) *
                MatrixEx.Rotate(UnitVector3D.Create(0, 1, 0), angle) *
                matrixTransform;

            foreach (Point3D[] polyLine in CubePolyLines)
            {
                DrawPolyLine(matrixTransform.Transform(polyLine), Space.World, Pens.Yellow);
            }

            matrixTransform =
                MatrixEx.Scale(0.5) *
                MatrixEx.Rotate(UnitVector3D.Create(1, 0, 0), angle) *
                matrixTransform;

            foreach (Point3D[] polyLine in CubePolyLines)
            {
                DrawPolyLine(matrixTransform.Transform(polyLine), Space.World, Pens.Magenta);
            }
        }

        #endregion

        #region // deltaTime
        public double GetDeltaTime(TimeSpan periodDuration)
        {
            return GetDeltaTime(FrameStarted, periodDuration);
        }

        public static double GetDeltaTime(DateTime timestamp, TimeSpan periodDuration)
        {
            return (timestamp.Second * 1000 + timestamp.Millisecond) % periodDuration.TotalMilliseconds / periodDuration.TotalMilliseconds;
        }

        #endregion
    }
}
