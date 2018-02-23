# YariControl.RealCursorPosition
Get the color / position from the screen with different zoom of the Font.
You can take real screen Font zoom or DPI.

## Preview how it works
Font Zoom 100:
![Font Zoom 100](https://raw.githubusercontent.com/Yari27/YariControl.RealCursorPosition/master/FontZoom100.jpg?resize=400,200)

Font Zoom 125:
![Font Zoom 125](https://raw.githubusercontent.com/Yari27/YariControl.RealCursorPosition/master/FontZoom125.jpg?resize=400,200)

Font Zoom 150:
![Font Zoom 150](https://raw.githubusercontent.com/Yari27/YariControl.RealCursorPosition/master/FontZoom150.png?resize=400,200)

Video:
https://youtu.be/Q4IPKKButEA

## How it use
Example use:
````csharp
using YariControl.RealCursorPosition;
````
`````C#
  private Point point;
  
  private void timer1_Tick(object sender, EventArgs e)
  {
      point = DisplayScreenTools.GetRoundedRealPoint(Cursor.Position);

      label1.Text = "Position: " + point.ToString();//print real position
      label2.Text = "Font Zoom: " + DisplayScreenTools.GetFontZoom + "%";//print Font Zoom percent
      label3.Text = "Screen DPI: " + DisplayScreenTools.GetScreenDpi;//print screen dpi

      panel1.BackColor = ScreenPixelColor.GetPixelColor(point);//pixel color from real cursor position
      pictureBox1.Image = Screenshot.TakeCenterSnapshot(point, pictureBox1.Size);//Get center screen shot
  }
`````

## NuGet package [![NuGet](https://img.shields.io/nuget/v/YariControl.RealCursorPosition.svg)](https://www.nuget.org/packages/YariControl.RealCursorPosition/) [![NuGet](https://img.shields.io/nuget/dt/YariControl.RealCursorPosition.svg)](https://www.nuget.org/packages/YariControl.RealCursorPosition/)
`````
PM> Install-Package YariControl.RealCursorPosition -Version 1.0.2.2
`````
`Created on .NET.Framework 3.0`
