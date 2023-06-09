using GraphicsProject.Common.Render;
using GraphicsProject.Inputs;
using GraphicsProject.Utilities;
using GraphicsProject.Win;
using System;
using System.Collections.Generic;
using System.Linq;
using GraphicsProject.Drivers.Gdi.Render;

namespace GraphicsProject.Client
{
    public static class WindowFactory
    {
        public static IReadOnlyList<IRenderHost> SeedWindows()
        {
            var size = new System.Drawing.Size(720, 480);

            var renderHosts = new[] 
            {
                CreateWindowsForm(size, "Hello World!", rhs => new Drivers.Gdi.Render.RenderHost(rhs)),
                //CreateWindowWpf(size, "Hello World2!", rhs => new Drivers.Gdi.Render.RenderHost(rhs))
            };
            SortWindows(renderHosts);
            return renderHosts;
        }

        public static System.Windows.Forms.Control CreateHostControl()
        {
            var hostControl = new System.Windows.Forms.Panel {
                Dock = System.Windows.Forms.DockStyle.Fill,
                BackColor = System.Drawing.Color.Transparent,
                ForeColor = System.Drawing.Color.Transparent
            };


            // Focus control so we can use the mouse to click on this window
            void EnsureFocus(System.Windows.Forms.Control control) {
                if (!control.Focused) control.Focus();
            }


            hostControl.MouseEnter += (sender, args) => EnsureFocus(hostControl);
            hostControl.MouseClick += (sender, args) => EnsureFocus(hostControl);

            return hostControl;

        }

        private static IRenderHost CreateWindowsForm(System.Drawing.Size size, string title, Func<IRenderHostSetup, IRenderHost> ctorRenderHost)
        {
            var window = new System.Windows.Forms.Form
            {
                Size = size,
                Text = title
            };
            var hostControl = CreateHostControl();
            window.Controls.Add(hostControl);

            window.FormClosed += (sender, args) => System.Windows.Application.Current.Shutdown();
            window.Show();

            return ctorRenderHost(new RenderHostSetup(hostControl.Handle(), new InputForms(hostControl)));
            
        }

        private static IRenderHost CreateWindowWpf(System.Drawing.Size size, string title, Func<IRenderHostSetup, IRenderHost> ctorRenderHost)
        {
            var window = new System.Windows.Window
            {
                Width = size.Width,
                Height = size.Height,
                Title = title
            };

            var hostControl = CreateHostControl();
            var windowsFormsHost = new System.Windows.Forms.Integration.WindowsFormsHost
            {
                Child = hostControl
            };

            window.Content = windowsFormsHost;

            window.Closed += (sender, args) => System.Windows.Application.Current.Shutdown();
            window.Show();

            return ctorRenderHost(new RenderHostSetup(hostControl.Handle(), new InputForms(hostControl)));

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
