using GraphicsProject.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Drivers.Gdi.Materials
{
    /// <summary>
    /// Internal shader vertex interface.
    /// </summary>
    public interface IPsIn<TPsIn> :
        IInterpolate<TPsIn>
    {
        /// <summary>
        /// Clip space position (vertex shader output).
        /// </summary>
        Vector4F Position { get; }

        /// <summary>
        /// Clone vertex with new position.
        /// </summary>
        TPsIn ReplacePosition(Vector4F position);
    }
}
