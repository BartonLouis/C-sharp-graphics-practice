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

        #endregion
    }
}
