using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Engine.Render
{
    public interface IRenderHost : IDisposable
    {
        IntPtr HostHandle { get; }


    }
}
