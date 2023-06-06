using GraphicsProject.Win;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Drivers
{
    public class RenderHost : Engine.Render.RenderHost
    {

        private Graphics GraphicsHost { get; set; }
        private Font FontConsolas12 { get; set; }
        private BufferedGraphics BufferedGraphics { get; set; }

        public RenderHost(IntPtr hostHandle) : base(hostHandle)
        {
            GraphicsHost = Graphics.FromHwnd(HostHandle);
            BufferedGraphics = BufferedGraphicsManager.Current.Allocate(GraphicsHost, new Rectangle(Point.Empty, W.GetClientRectangle(HostHandle).Size));


            FontConsolas12 = new Font("Consolas", 12);
        }


        public override void Dispose()
        {
            GraphicsHost.Dispose();
            GraphicsHost = default;
            BufferedGraphics.Dispose();
            BufferedGraphics = default;
            FontConsolas12.Dispose();
            FontConsolas12 = default;

            base.Dispose();
        }

        protected override void RenderInternal()
        {
            BufferedGraphics.Graphics.Clear(Color.Black);
            BufferedGraphics.Graphics.DrawString($"Fps: {FpsCounter.FpsRender:0.000}", FontConsolas12, Brushes.Red, 0, 0);

            BufferedGraphics.Render();
        }
    }
}
