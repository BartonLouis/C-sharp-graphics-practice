using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Common.Camera.Projections
{
    /// <summary>
    /// Orthographic projection.
    /// </summary>
    public interface IProjectionOrthographic :
        IProjection
    {
        /// <summary>
        /// Width of the view volume.
        /// </summary>
        double FieldWidth { get; }

        /// <summary>
        /// Height of the view volume.
        /// </summary>
        double FieldHeight { get; }
    }
}
