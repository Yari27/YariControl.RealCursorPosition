using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace YariControl.RealCursorPosition
{

    public static class ScreenPixelColor
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        public static Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            return Color.FromArgb((int)(pixel & 0x000000FF), (int)(pixel & 0x0000FF00) >> 8, (int)(pixel & 0x00FF0000) >> 16);
        }

        public static Color GetPixelColor(Point position)
        {
            return GetPixelColor(position.X, position.Y);
        }

        public static Bitmap TakeCenterSnapshot(Point source, Size size)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            g.CopyFromScreen(new Point(source.X - size.Width / 2, source.Y - size.Height / 2), new Point(0, 0), size);
            return bmp;
        }
    }

    public static class DisplayScreenTools
    {
        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        private enum DeviceCap
        {
            Desktopvertres = 117,
            Desktophorzres = 118
        }

        public static Size GetPhysicalDisplaySize
        {
            get {
                Graphics g = Graphics.FromHwnd(IntPtr.Zero);
                IntPtr desktop = g.GetHdc();
                
                int physicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.Desktopvertres);
                int physicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.Desktophorzres);

                return new Size(physicalScreenWidth, physicalScreenHeight);
            }
        }

        public static Size GetVirtualDisplaySize
        {
            get{
                return new Size(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            }
        }

        public static Point GetFontZoomProcentage
        {
            get {
                Size resolution = GetPhysicalDisplaySize;
                Size virtualscreen = GetVirtualDisplaySize;
                int zoomH = (resolution.Height * 100) / virtualscreen.Height;
                int zoomW = (resolution.Width * 100) / virtualscreen.Width;

                return new Point(zoomW, zoomH);
            }
        }

        public static PointF GetFontZoom
        {
            get
            {
                Size resolution = GetPhysicalDisplaySize;
                Size virtualscreen = GetVirtualDisplaySize;
                float zoomH = (resolution.Height * 100) / virtualscreen.Height;
                float zoomW = (resolution.Width * 100) / virtualscreen.Width;

                return new PointF(zoomW / 100, zoomH / 100);
            }
        }

        public static PointF GetRealPoint(Point cursor)
        {
            return new PointF(cursor.X * GetFontZoom.X, cursor.Y * GetFontZoom.Y);
        }

        public static Point GetRoundedRealPoint(Point cursor)
        {
            return new Point((int)Math.Round(cursor.X * GetFontZoom.X, 0),
                (int)Math.Round(cursor.Y * GetFontZoom.Y, 0));
        }

    }
}
