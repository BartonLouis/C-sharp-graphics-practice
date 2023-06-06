using GraphicsProject.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Engine.Render
{
    internal class RenderHostSetup : IRenderHostSetup
    {

        #region // storage

        public IntPtr HostHandle { get; }
        public IInput HostInput { get; }


        #endregion

        #region // ctor

        public RenderHostSetup(IntPtr hostHandle, IInput hostInput)
        {
            HostHandle = hostHandle;
            HostInput = hostInput;
        }

        #endregion
    }
}
