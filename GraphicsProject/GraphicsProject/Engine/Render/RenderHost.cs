using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Engine.Render
{
    public abstract class RenderHost : IRenderHost
    {
        public IntPtr HostHandle { get; private set; }

        public FpsCounter FpsCounter { get; private set; }

        protected RenderHost(IntPtr hostHandle)
        {
            HostHandle = hostHandle;
            FpsCounter = new FpsCounter(new TimeSpan(0, 0, 0, 0, 1000));
        }
        public virtual void Dispose() {
            FpsCounter?.Dispose();
            FpsCounter = default;
            HostHandle = default;
        }

        public void Render()
        {
            FpsCounter.StartFrame();
            RenderInternal();
            FpsCounter.StopFrame();
        }

        protected abstract void RenderInternal();
    }
}
