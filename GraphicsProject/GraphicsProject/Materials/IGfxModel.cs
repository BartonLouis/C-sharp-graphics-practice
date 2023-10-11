using GraphicsProject.Mathematics;
using System;

namespace GraphicsProject.Materials
{
    /// <summary>
    /// <see cref="IModel"/> for particular driver.
    /// </summary>
    public interface IGfxModel :
        IDisposable
    {
        /// <summary>
        /// Render to mounted surface.
        /// </summary>
        void Render(in Matrix4D matrixToClip);
    }
}
