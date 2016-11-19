using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico
{
	internal class ServiceRunner
	{

		#region メソッド

		[STAThread]
		private static void Main(string[] args)
		{
			System.Threading.Thread.CurrentThread.Name = "Nadesico application thread";
#if RELEASE
			EnsureThat.Ensure.Off()
#endif
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
			{
				new NadesicoWindowsService()
			};
			ServiceBase.Run(ServicesToRun);
		}

		#endregion メソッド
	}
}
