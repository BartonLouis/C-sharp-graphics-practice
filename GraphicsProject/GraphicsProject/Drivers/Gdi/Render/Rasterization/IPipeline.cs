using GraphicsProject.Drivers.Gdi.Materials;
using GraphicsProject.Drivers.Gdi.Render.Rasterization;
using GraphicsProject.Materials;
using System;

namespace GraphicsProject.Drivers.Gdi.Render
{
    /// <summary>
    /// Graphics pipeline interface.
    /// </summary>
    public interface IPipeline<in TVsIn, TPsIn> :
        IDisposable
        where TVsIn : unmanaged
        where TPsIn : unmanaged, IPsIn<TPsIn>
    {
        /// <summary>
        /// Render vertices.
        /// </summary>
        void Render(IBufferBinding<TVsIn> bufferBinding, int countVertices, PrimitiveTopology primitiveTopology);
    }
}
