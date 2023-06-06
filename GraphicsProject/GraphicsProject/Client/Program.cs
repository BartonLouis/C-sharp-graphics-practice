using GraphicsProject.Engine.Render;
using GraphicsProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Client
{
    internal class Program : System.Windows.Application, IDisposable
    {
        #region // storage

        private IReadOnlyList<IRenderHost> RenderHosts { get; set; }

        #endregion


        #region // constructor

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
