using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Engine.Render
{
    public abstract class RenderHost : IRenderHost
    {
        #region // storage
        public IntPtr HostHandle { get; private set; }

        #endregion

        #region // constructor

        protected RenderHost(IntPtr hostHandle)
        {
            HostHandle = hostHandle;
        }
        public void Dispose() {
            HostHandle = default;
        }

        #endregion
    }
}
