using System;
using System.Drawing;
using System.Windows.Forms;

namespace VolturaTextClock
{
    internal static class ImageOverlay
    {
        internal static void InitializeImage(ref PictureBox pb)
        {
            Log.LogCaller();
            try
            {
                using Bitmap c = new Bitmap(pb.Image);
                pb.InitialImage = ToolStripRenderer.CreateDisabledImage(c);
                pb.BackgroundImage = pb.Image;
                pb.Image = pb.InitialImage;
            }
            catch (Exception ex)
            {
                Log.Error = ex;
            }
        }

        internal static void SwitchImage(PictureBox pb)
        {
            Log.LogCaller();
            pb.InitialImage = pb.Image;
            pb.Image = pb.BackgroundImage;
            pb.BackgroundImage = pb.InitialImage;
            CleanupMemory();
        }

        internal static void CleanupMemory()
        {
            Log.LogCaller();
            GC.Collect();
            GC.WaitForFullGCComplete(1000);
        }
    }
}
