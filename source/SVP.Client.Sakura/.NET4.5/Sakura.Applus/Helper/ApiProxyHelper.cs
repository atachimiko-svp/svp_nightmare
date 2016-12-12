using SVP.CIL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfClientProxyGenerator;
using WcfClientProxyGenerator.Async;

namespace Sakura.Applus.Helper
{
	public static class ApiProxyHelper
	{


		#region メソッド

		public static IAsyncProxy<IApplicationInterfaceService> CreateAsyncProxy()
		{
			var proxy = WcfClientProxy.CreateAsyncProxy<IApplicationInterfaceService>(c =>
				{
					c.SetEndpoint("netNamedPipeBinding_AppApi");
				});
			return proxy;
		}

		public static IApplicationInterfaceService CreateProxy()
		{
			var proxy = WcfClientProxy.Create<IApplicationInterfaceService>(c =>
				{
					c.SetEndpoint("netNamedPipeBinding_AppApi");
				});
			return proxy;
		}

		#endregion メソッド
	}
}
