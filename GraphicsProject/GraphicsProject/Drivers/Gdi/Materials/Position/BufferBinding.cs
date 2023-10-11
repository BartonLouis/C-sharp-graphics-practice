using GraphicsProject.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Drivers.Gdi.Materials.Position
{
    /// <inheritdoc />
    public class BufferBinding
        : IBufferBinding<VsIn>
    {
        /// <summary>
        /// Position buffer.
        /// </summary>
        public Vector3F[] Positions { get; }

        /// <summary />
        public BufferBinding(Vector3F[] positions)
        {
            Positions = positions;
        }

        /// <inheritdoc />
        public VsIn GetVsIn(uint index)
        {
            return new VsIn
            (
                Positions[index]
            );
        }
    }
}
