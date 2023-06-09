using GraphicsProject.Common.Camera;
using GraphicsProject.Inputs;
using System;

namespace GraphicsProject.Common.Render
{
    public interface IRenderHost : IDisposable
    {
        IntPtr HostHandle { get; }
        IInput HostInput { get; }

        ICameraInfo CameraInfo { get; set; }

        FpsCounter FpsCounter { get; }

        void Render();

        event EventHandler<ICameraInfo> CameraInfoChanged;
    }
}
