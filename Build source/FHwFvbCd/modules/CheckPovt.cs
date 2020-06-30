using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Stealer.modules
{
	// Token: 0x0200003B RID: 59
	internal class CheckPovt
	{
		// Token: 0x0600013C RID: 316 RVA: 0x0000DFD8 File Offset: 0x0000C1D8
		public static void CheckPril()
		{
			if (!CheckPovt.Inizialize())
			{
				Environment.Exit(0);
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000DFE8 File Offset: 0x0000C1E8
		private static string GetGUID()
		{
			string text;
			try
			{
				Assembly assembly = typeof(CheckPovt).Assembly;
				GuidAttribute guidAttribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
				text = guidAttribute.Value;
			}
			catch
			{
				text = "CF2D4313-33DE-489D-9721-6AFF69841DEY";
			}
			return text.ToUpper();
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000E048 File Offset: 0x0000C248
		private static bool Inizialize()
		{
			bool result;
			Mutex obj = new Mutex(true, CheckPovt.GetGUID(), ref result);
			GC.KeepAlive(obj);
			return result;
		}
	}
}
