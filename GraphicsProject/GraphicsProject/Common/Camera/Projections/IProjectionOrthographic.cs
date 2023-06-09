using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Common.Camera.Projections
{
    public interface IProjectionOrthographic : IProjection
    {
        double FieldWidth { get; }
        double FieldHeight { get; }

    }
}
