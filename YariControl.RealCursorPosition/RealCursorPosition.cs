using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace YariControl.RealCursorPosition
{
    public static class ScreenPixelColor
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

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

    public static class DisplayTools
    {
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        private enum DeviceCap
        {
            Desktopvertres = 117,
            Desktophorzres = 118
        }

        public static Size GetPhysicalDisplaySize()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();

            int physicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.Desktopvertres);
            int physicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.Desktophorzres);

            return new Size(physicalScreenWidth, physicalScreenHeight);
        }

        public static System.Drawing.Size GetVirtualDisplaySize()
        {
            return new Size(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
        }

        public static int GetFontZoom()
        {
            Size resolution = GetPhysicalDisplaySize();
            Size virtualscreen = GetVirtualDisplaySize();
            int zoomH = (resolution.Height * 100) / virtualscreen.Height;
            int zoomW = (resolution.Width * 100) / virtualscreen.Width;

            return zoomH == zoomW ? zoomH : 100;
        }

        public static PointF GetRealPoint(Point cursor)
        {
            return new PointF(cursor.X * ((float)GetFontZoom() / 100),
                cursor.Y * ((float)GetFontZoom() / 100));
        }

        public static Point GetRoundedRealPoint(Point cursor)
        {
            return new Point((int)Math.Round(cursor.X * ((float)GetFontZoom() / 100), 0),
                (int)Math.Round(cursor.Y * ((float)GetFontZoom() / 100), 0));
        }

    }
}
