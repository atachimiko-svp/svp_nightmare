using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Sakura.Win32
{
	internal class NativeMethods
	{
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr MonitorFromWindow(IntPtr hwnd, MonitorDefaultTo dwFlags);

		[DllImport("SHCore.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
		public static extern void GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, ref uint dpiX, ref uint dpiY);
	}

	internal enum MonitorDefaultTo
	{
		MONITOR_DEFAULTTONULL = 0,
		MONITOR_DEFAULTTOPRIMARY = 1,
		MONITOR_DEFAULTTONEAREST = 2,
	}

	/// Identifies dots per inch (dpi) type.
	/// </summary>
	public enum MonitorDpiType
	{
		/// <summary>
		/// MDT_Effective_DPI
		/// <para>Effective DPI that incorporates accessibility overrides and matches what Desktop Window Manage (DWM) uses to scale desktop applications.</para>
		/// </summary>
		EffectiveDpi = 0,

		/// <summary>
		/// MDT_Angular_DPI
		/// <para>DPI that ensures rendering at a compliant angular resolution on the screen, without incorporating accessibility overrides.</para>
		/// </summary>
		AngularDpi = 1,

		/// <summary>
		/// MDT_Raw_DPI
		/// <para>Linear DPI of the screen as measures on the screen itself.</para>
		/// </summary>
		RawDpi = 2,

		/// <summary>
		/// MDT_Default
		/// </summary>
		Default = EffectiveDpi,
	}
}
