using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Inputs
{
    internal class SizeEventArgs : EventArgs, ISizeEventArgs
    {
        public Size NewSize { get; set; }

        public SizeEventArgs(Size newSize)
        {
            NewSize = newSize;
        }
    }
}
