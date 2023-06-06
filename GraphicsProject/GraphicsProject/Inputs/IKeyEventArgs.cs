using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Inputs
{
    public interface IKeyEventArgs
    {
        Key Key { get; }
        Modifiers Modifiers { get; }
    }
}
