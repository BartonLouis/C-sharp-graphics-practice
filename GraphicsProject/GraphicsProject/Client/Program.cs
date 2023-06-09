using GraphicsProject.Common.Camera;
using GraphicsProject.Common.Render;
using GraphicsProject.Drivers.Gdi.Render;
using GraphicsProject.Utilities;
using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
namespace GraphicsProject.Client
{
    internal class Program : System.Windows.Application, IDisposable
    {
        #region // storage

        private IReadOnlyList<IRenderHost> RenderHosts { get; set; }

        #endregion

        #region // ctor

        public Program() {
            Startup += (sender, args) => Constructor();
            Exit += (sender, args) => Dispose();
        }

        private void Constructor()
        {
            RenderHosts = WindowFactory.SeedWindows();

            // render loop
            while (!Dispatcher.HasShutdownStarted)
            {
                DebugCameras(RenderHosts);
                Render(RenderHosts);
                System.Windows.Forms.Application.DoEvents();
            }
        }

        public void Dispose()
        {
            RenderHosts.ForEach(host => host.Dispose());
            RenderHosts = default;
        }

        #endregion

        #region // render

        private static void Render(IEnumerable<IRenderHost> renderHosts)
        {
            renderHosts.ForEach(host => host.Render());
        }

        private static void DebugCameras(IReadOnlyList<IRenderHost> renderHosts)
        {
            var utcNow = DateTime.UtcNow;
            const int radius = 2;

            for (var i = 0; i < renderHosts.Count; i++) {
                var t = Drivers.Gdi.Render.RenderHost.GetDeltaTime(utcNow, new TimeSpan(0, 0, 0, i % 2 == 0 ? 10 : 30));
                var angle = t * Math.PI * 2;
                angle *= i % 2 == 0 ? 1 : -1;

                var cameraInfo = renderHosts[i].CameraInfo;
                renderHosts[i].CameraInfo = new CameraInfo(
                    new Point3D(Math.Sin(angle) * radius, Math.Cos(angle) * radius, 2),
                    new Point3D(0, 0, 0.5),
                    cameraInfo.UpVector,
                    cameraInfo.Projection,
                    cameraInfo.Viewport
                    );

            }
        }

        #endregion
    }
}
