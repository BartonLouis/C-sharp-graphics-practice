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
        }

        public void Dispose()
        {
            RenderHosts.ForEach(host => host.Dispose());
            RenderHosts = default;
        }

        #endregion
    }
}
