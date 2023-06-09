using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Common.Camera.Projections
{
    internal interface IProjectionPerspective : IProjection
    {
        double FieldOfViewY { get; }
        double AspectRatio { get; }
    }
}
