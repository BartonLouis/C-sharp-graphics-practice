﻿using GraphicsProject.Common.Render;
using GraphicsProject.Drivers.Gdi.Render;
using GraphicsProject.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Engine.Operators
{
    public class Operator : IOperator
    {

        #region //storage

        protected IRenderHost RenderHost { get; private set; }

        protected IInput Input => RenderHost.HostInput;

        #endregion

        #region //ctor

        protected Operator(IRenderHost renderHost)
        {
            RenderHost = renderHost;

            Input.SizeChanged += InputOnSizeChanged;
            Input.KeyDown += InputOnKeyDown;
            Input.KeyUp += InputOnKeyUp;
            Input.MouseMove += InputOnMouseMove;
            Input.MouseDown += InputOnMouseDown;
            Input.MouseUp += InputOnMouseUp;
            Input.MouseWheel += InputOnMouseWheel;
        }

        public virtual void Dispose()
        {
            Input.SizeChanged -= InputOnSizeChanged;
            Input.KeyDown -= InputOnKeyDown;
            Input.KeyUp -= InputOnKeyUp;
            Input.MouseMove -= InputOnMouseMove;
            Input.MouseDown -= InputOnMouseDown;
            Input.MouseUp -= InputOnMouseUp;
            Input.MouseWheel -= InputOnMouseWheel;

            RenderHost = default;
        }

        #endregion

        #region //sensors

        protected virtual void InputOnSizeChanged(object sender, ISizeEventArgs args)
        {
        }

        protected virtual void InputOnKeyDown(object sender, IKeyEventArgs args)
        {
        }

        protected virtual void InputOnKeyUp(object sender, IKeyEventArgs args) 
        {
        }

        protected virtual void InputOnMouseMove(object sender, IMouseEventArgs args)
        {
        }

        protected virtual void InputOnMouseDown(object sender, IMouseEventArgs args)
        {
        }

        protected virtual void InputOnMouseUp(object sender, IMouseEventArgs args)
        {
        }

        protected virtual void InputOnMouseWheel(object sender, IMouseEventArgs args)
        {
        }

        #endregion
    }
}
