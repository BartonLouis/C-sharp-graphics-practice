using GraphicsProject.Common.Camera;
using GraphicsProject.Engine.Render;
using GraphicsProject.Inputs;
using GraphicsProject.Mathematics.Extensions;
using GraphicsProject.Utilities;
using MathNet.Spatial.Euclidean;

namespace GraphicsProject.Engine.Operators
{
    public class OperatorCameraPan : Operator
    {
        #region //storage
        private ICameraInfo MouseDownCameraInfo { get; set; }
        private Plane? MouseDownPlane { get; set; }
        private Point3D? MouseDownOnPlane { get; set; }

        #endregion

        #region //ctor
        public OperatorCameraPan(IRenderHost renderHost) : base(renderHost) { }

        public override void Dispose()
        {
            base.Dispose();
            MouseDownCameraInfo = default;
            MouseDownPlane = default;
            MouseDownOnPlane = default;
        }
        #endregion

        #region //routines

        private static Point3D GetPanOrigin(ICameraInfo cameraInfo)
        {
            return cameraInfo.Target;
        }

        protected override void InputOnMouseDown(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseDown(sender, args);

            if (args.Buttons != MouseButtons.Right) return;
            MouseDownCameraInfo = RenderHost.CameraInfo.Cloned();

            var panOrigin = GetPanOrigin(MouseDownCameraInfo);
            MouseDownPlane = new Plane(panOrigin, MouseDownCameraInfo.GetEyeDirection());
            var mouseRay = MouseDownCameraInfo.GetMouseRay(Mathematics.Space.Screen, args.Position.ToPoint3D());

            MouseDownOnPlane = MouseDownPlane.Value.IntersectionWith(mouseRay);

        }

        protected override void InputOnMouseUp(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseUp(sender, args);

            if (args.Buttons != MouseButtons.Right) return;

            MouseDownCameraInfo = default;
            MouseDownPlane = default;
            MouseDownOnPlane = default;
        }

        protected override void InputOnMouseMove(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseMove(sender, args);

            if (MouseDownCameraInfo is null || !MouseDownPlane.HasValue || !MouseDownOnPlane.HasValue) return;

            var mouseRay = MouseDownCameraInfo.GetMouseRay(Mathematics.Space.Screen, args.Position.ToPoint3D());
            var mouseMoveOnPlane = MouseDownPlane.Value.IntersectionWith(mouseRay);
            var offset = mouseMoveOnPlane - MouseDownOnPlane.Value;

            var target = MouseDownCameraInfo.Target - offset;
            var position = MouseDownCameraInfo.Position - offset;
            var cameraInfo = RenderHost.CameraInfo;

            RenderHost.CameraInfo = new CameraInfo(position, target, cameraInfo.UpVector, cameraInfo.Projection.Cloned(), cameraInfo.Viewport);

        }

        #endregion

    }
}
