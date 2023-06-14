using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Drivers.Gdi.Materials
{
    /// <summary>
    /// Shader library.
    /// </summary>
    public class ShaderLibrary
    {
        #region // storage

        /// <summary>
        /// <see cref="Position"/> material shader.
        /// </summary>
        public Position.Shader ShaderPosition { get; set; }

        #endregion

        #region // ctor

        /// <summary />
        public ShaderLibrary()
        {
            ShaderPosition = new Position.Shader();
        }

        #endregion
    }
}
