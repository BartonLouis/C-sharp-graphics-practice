using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MathNet.Spatial.Euclidean;

namespace GraphicsProject.Inputs
{
    public interface IMouseEventArgs
    {
        Point2D Position { get; }

        MouseButtons Buttons { get; }

        int WheelDelta { get; }

        int ClickCount { get; }
    }
}
