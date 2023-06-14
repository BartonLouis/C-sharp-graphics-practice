using GraphicsProject.Common.Camera;
using GraphicsProject.Inputs;
using GraphicsProject.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicsProject.Engine.Render
{
    /// <summary>
    /// Interface for render host.
    /// </summary>
    public interface IRenderHost :
        IDisposable
    {
        /// <summary>
        /// Handle of hosting window.
        /// </summary>
        IntPtr HostHandle { get; }

        /// <summary>
        /// Input from host.
        /// </summary>
        IInput HostInput { get; }

        /// <summary>
        /// Desired surface size.
        /// </summary>
        Size HostSize { get; }

        /// <inheritdoc cref="ICameraInfo"/>
        ICameraInfo CameraInfo { get; set; }

        /// <inheritdoc cref="Engine.Render.FpsCounter"/>
        FpsCounter FpsCounter { get; }

        /// <summary>
        /// Render.
        /// </summary>
        void Render(IEnumerable<IPrimitive> primitives);

        /// <summary>
        /// Fires when <see cref="CameraInfo"/> changed.
        /// </summary>
        event EventHandler<ICameraInfo> CameraInfoChanged;
    }
}
