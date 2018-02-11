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

        /// <summary>
        /// Take the color from the given cursor position.
        /// </summary>
        /// <param name="x">Cursor X position.</param>
        /// <param name="y">Cursor Y position.</param>
        /// <returns></returns>
        public static Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            return Color.FromArgb((int)(pixel & 0x000000FF), (int)(pixel & 0x0000FF00) >> 8, (int)(pixel & 0x00FF0000) >> 16);
        }

        /// <summary>
        /// Take the color from the given cursor position.
        /// </summary>
        /// <param name="position">Cursor position.</param>
        /// <returns></returns>
        public static Color GetPixelColor(Point position)
        {
            return GetPixelColor(position.X, position.Y);
        }

    }

    public static class Screenshot
    {
        /// <summary>
        /// Take a centered slice of the screen.
        /// </summary>
        /// <param name="source">The point indicated by the mouse.</param>
        /// <param name="size">The size of the area to be picked up.</param>
        /// <returns></returns>
        public static Bitmap TakeCenterSnapshot(Point source, Size size)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            g.CopyFromScreen(new Point(source.X - size.Width / 2, source.Y - size.Height / 2), new Point(0, 0), size);
            return bmp;
        }

        /// <summary>
        /// Take a piece of the screen.
        /// </summary>
        /// <param name="source">The point indicated by the mouse.</param>
        /// <param name="size">The size of the area to be picked up.</param>
        /// <returns></returns>
        public static Bitmap TakeSnapshot(Point source, Size size)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            g.CopyFromScreen(new Point(source.X - size.Width, source.Y - size.Height), new Point(0, 0), size);
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

        /// <summary>
        /// Get the physical size of the display.
        /// </summary>
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

        /// <summary>
        /// Get the virtual size of the display after zoom.
        /// </summary>
        public static Size GetVirtualDisplaySize
        {
            get{
                return new Size(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            }
        }

        /// <summary>
        /// Get a percentage of the font zoom size.
        /// </summary>
        public static Point GetFontZoomPercentage
        {
            get {
                Size resolution = GetPhysicalDisplaySize;
                Size virtualscreen = GetVirtualDisplaySize;
                int zoomH = (resolution.Height * 100) / virtualscreen.Height;
                int zoomW = (resolution.Width * 100) / virtualscreen.Width;

                return new Point(zoomW, zoomH);
            }
        }

        /// <summary>
        /// Get the font zoom size.
        /// </summary>
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

        /// <summary>
        /// Take the real cursor position.
        /// </summary>
        /// <param name="cursor">Cursor position read.</param>
        /// <returns>Real cursor position</returns>
        public static PointF GetRealPoint(Point cursor)
        {
            return new PointF(cursor.X * GetFontZoom.X, cursor.Y * GetFontZoom.Y);
        }

        /// <summary>
        /// Take the real rounded cursor position.
        /// </summary>
        /// <param name="cursor">Cursor position read.</param>
        /// <returns>Real rounded cursor position</returns>
        public static Point GetRoundedRealPoint(Point cursor)
        {
            return new Point((int)Math.Round(cursor.X * GetFontZoom.X, 0),
                (int)Math.Round(cursor.Y * GetFontZoom.Y, 0));
        }

    }
}
