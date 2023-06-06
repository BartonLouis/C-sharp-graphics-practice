using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Drivers
{
    public class RenderHost : Engine.Render.RenderHost
    {
        public RenderHost(IntPtr hostHandle) : base(hostHandle)
        {
        }
    }
}
