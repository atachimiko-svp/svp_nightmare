using Sakura.Structures;
using Sakura.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace Sakura.Utils
{
	/// <summary>
	/// http://grabacr.net/archives/1132
	/// </summary>
	public static class PerMonitorDpiService
	{

		#region プロパティ

		public static bool IsSupported
		{
			get
			{
				var version = Environment.OSVersion.Version;
				return version.Major == 6 && version.Minor == 3;
			}
		}

		#endregion プロパティ


		#region メソッド

		public static Dpi GetDpi(this HwndSource hwndSource, MonitorDpiType dpiType = MonitorDpiType.Default)
		{
			if (!IsSupported) return Dpi.Default;

			var hmonitor = NativeMethods.MonitorFromWindow(
				hwndSource.Handle,
				MonitorDefaultTo.MONITOR_DEFAULTTONEAREST);

			uint dpiX = 1, dpiY = 1;
			NativeMethods.GetDpiForMonitor(hmonitor, dpiType, ref dpiX, ref dpiY);

			return new Dpi(dpiX, dpiY);
		}

		#endregion メソッド
	}
}
