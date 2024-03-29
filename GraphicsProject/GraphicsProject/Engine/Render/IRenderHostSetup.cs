﻿using GraphicsProject.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Engine.Render
{
    public interface IRenderHostSetup
    {

        IntPtr HostHandle { get; }
        IInput HostInput { get; }
    }
}
