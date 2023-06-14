using GraphicsProject.Common.Camera;
using GraphicsProject.Common.Camera.Projections;
using GraphicsProject.Engine.Render;
using GraphicsProject.Inputs;
using GraphicsProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Engine.Operators
{
    public class OperatorCameraZoom : Operator
    {
        #region //storage

        #endregion

        #region //ctor
        public OperatorCameraZoom(IRenderHost renderHost) : base(renderHost) { }

        #endregion

        #region //routines

        protected override void InputOnMouseWheel(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseWheel(sender, args);

            // copy camera ref
            var cameraInfo = RenderHost.CameraInfo;

            // default scaling
            const double scale = 0.05;
            const double scaleForward = 1 + scale;
            const double scaleBackward = 2 - 1 / (1 - scale);

            // calculate how much zoom
            var scaleCurrent = args.WheelDelta > 0? scaleForward : scaleBackward;
            var eyeVector = cameraInfo.Target - cameraInfo.Position;
            var offset = eyeVector.ScaleBy(scaleCurrent) - eyeVector;

            var position = cameraInfo.Position + offset;

            var projection = cameraInfo.Projection is ProjectionOrthographic po
                ? ProjectionOrthographic.FromDistance(po.NearPlane, po.FarPlane, (cameraInfo.Target - position).Length, cameraInfo.Viewport.AspectRatio)
                : cameraInfo.Projection.Cloned();

            // set camera info
            RenderHost.CameraInfo = new CameraInfo(position, cameraInfo.Target, cameraInfo.UpVector, projection, cameraInfo.Viewport);
        }

        #endregion
    }
}
