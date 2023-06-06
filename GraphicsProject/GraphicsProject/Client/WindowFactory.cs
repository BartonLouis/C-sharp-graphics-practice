using GraphicsProject.Engine.Render;
using GraphicsProject.Utilities;
using GraphicsProject.Win;
using System;
using System.Collections.Generic;
using System.Drawing.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Client
{
    public static class WindowFactory
    {
        public static IReadOnlyList<IRenderHost> SeedWindows()
        {
            var size = new System.Drawing.Size(720, 480);

            var renderHosts = new[] 
            {
                CreateWindowsForm(size, "Hello World!", h => new Drivers.RenderHost(h)),
                CreateWindowWpf(size, "Hello World2!", h => new Drivers.RenderHost(h))
            };
            SortWindows(renderHosts);
            return renderHosts;
        }

        private static IRenderHost CreateWindowsForm(System.Drawing.Size size, string title, Func<IntPtr, IRenderHost> ctorRenderHost)
        {
            var window = new System.Windows.Forms.Form
            {
                Size = size,
                Text = title
            };
            var hostControl = new System.Windows.Forms.Panel()
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                BackColor = System.Drawing.Color.Transparent,
                ForeColor = System.Drawing.Color.Transparent
            };
            window.Controls.Add(hostControl);

            hostControl.MouseEnter += (sender, args) =>
            {
                if (System.Windows.Forms.Form.ActiveForm != window) window.Activate();
                if (!hostControl.Focused) hostControl.Focus();
            };

            window.FormClosed += (sender, args) => System.Windows.Application.Current.Shutdown();
            window.Show();

            return ctorRenderHost(hostControl.Handle);
            
        }

        private static IRenderHost CreateWindowWpf(System.Drawing.Size size, string title, Func<IntPtr, IRenderHost> ctorRenderHost)
        {
            var window = new System.Windows.Window
            {
                Width = size.Width,
                Height = size.Height,
                Title = title
            };

            var hostControl = new System.Windows.Controls.Grid
            {
                Background = System.Windows.Media.Brushes.Transparent,
                Focusable = true
            };

            window.Content = hostControl;

            hostControl.MouseEnter += (sender, args) =>
            {
                if (!window.IsActive) window.Activate();
                if (!hostControl.IsFocused) hostControl.Focus();
            };

            window.Closed += (sender, args) => System.Windows.Application.Current.Shutdown();
            window.Show();

            return ctorRenderHost(hostControl.Handle());

        }

        private static void SortWindows(IEnumerable<IRenderHost> renderHosts) {
            var windowInfos = renderHosts.Select(renderHost => new WindowInfo(renderHost.HostHandle).GetRoot()).ToArray();

            var maxSize = new System.Drawing.Size(windowInfos.Max(a => a.RectangleWindow.Width), windowInfos.Max(a => a.RectangleWindow.Height));

            var maxColumns = (int)Math.Ceiling(Math.Sqrt(windowInfos.Length));
            var maxRows = (int)Math.Ceiling((double)windowInfos.Length / maxColumns);

            var primaryScreen = System.Windows.Forms.Screen.PrimaryScreen;
            var left = primaryScreen.WorkingArea.Width / 2 - maxColumns * maxSize.Width / 2;
            var top = primaryScreen.WorkingArea.Height / 2 - maxRows * maxSize.Height / 2;

            for(int row = 0; row < maxRows; row++)
            {
                for (int column = 0; column < maxColumns; column++)
                {
                    var i = row * maxColumns + column;
                    if (i >= windowInfos.Length) return;

                    var x = column * maxSize.Width + left;
                    var y = row * maxSize.Height + top;

                    var windowInfo = windowInfos[i];
                    User32.MoveWindow(windowInfo.Handle, x, y, windowInfo.RectangleWindow.Width, windowInfo.RectangleWindow.Height, false);
                }
            }
        }
    }
}
