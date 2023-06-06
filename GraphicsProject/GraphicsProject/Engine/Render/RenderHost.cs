using GraphicsProject.Inputs;
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
        public IInput HostInput { get; private set; }

        public FpsCounter FpsCounter { get; private set; }

        #endregion

        #region // ctor
        protected RenderHost(IRenderHostSetup renderHostSetup)
        {
            HostHandle = renderHostSetup.HostHandle;
            HostInput = renderHostSetup.HostInput;

            FpsCounter = new FpsCounter(new TimeSpan(0, 0, 0, 0, 1000));
        }
        public virtual void Dispose() {
            FpsCounter.Dispose();
            FpsCounter = default;

            HostHandle = default;

            HostInput.Dispose();
            HostInput = default;
        }
        #endregion

        #region // render

        public void Render()
        {
            FpsCounter.StartFrame();
            RenderInternal();
            FpsCounter.StopFrame();
        }

        protected abstract void RenderInternal();

        #endregion
    }
}
