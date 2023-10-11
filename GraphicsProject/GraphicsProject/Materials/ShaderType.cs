using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Materials
{
    /// <summary>
    /// Targeted shader type.
    /// </summary>
    public enum ShaderType
    {
        /// <summary>
        /// Undefined or unknown.
        /// </summary>
        Undefined,

        /// <summary>
        /// Mono-colored.
        /// </summary>
        Position,

        /// <summary>
        /// Vertices with individual colors.
        /// </summary>
        PositionColor,
    }
}
