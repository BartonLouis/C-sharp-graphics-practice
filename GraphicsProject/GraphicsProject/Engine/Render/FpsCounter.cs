using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Common.Render
{
    public class FpsCounter : IDisposable
    {
        #region // storage
        public TimeSpan UpdateRate { get; }
        private Stopwatch StopWatchUpdate { get; set; }
        private Stopwatch StopWatchFrame { get; set; }

        private TimeSpan Elapsed { get; set; }
        private int FrameCount { get; set; }

        public double FpsRender { get; private set; }
        public double FpsGlobal { get; private set; }

        public string FpsString => $"FPS = {FpsRender:0} ({FpsGlobal:0})";


        #endregion

        #region // ctor
        public FpsCounter(TimeSpan updateRate)
        {
            UpdateRate = updateRate;
            StopWatchUpdate = new Stopwatch();
            StopWatchFrame = new Stopwatch();

            StopWatchUpdate.Start();
            Elapsed = TimeSpan.Zero;
        }
        
        public void Dispose()
        {
            StopWatchUpdate?.Stop();
            StopWatchUpdate = default;

            StopWatchFrame?.Stop();
            StopWatchFrame = default;
        }

        #endregion

        #region // routines

        public void StartFrame()
        {
            StopWatchFrame.Restart();
        }

        public void StopFrame()
        {
            StopWatchFrame.Stop();

            Elapsed += StopWatchFrame.Elapsed;
            FrameCount++;

            var updateElapsed = StopWatchUpdate.Elapsed;
            if (updateElapsed >= UpdateRate)
            {
                FpsRender = FrameCount / Elapsed.TotalSeconds;
                FpsGlobal = FrameCount / updateElapsed.TotalSeconds;

                StopWatchUpdate.Restart();
                Elapsed = TimeSpan.Zero;
                FrameCount = 0;
            }
        }
        #endregion
    }
}
