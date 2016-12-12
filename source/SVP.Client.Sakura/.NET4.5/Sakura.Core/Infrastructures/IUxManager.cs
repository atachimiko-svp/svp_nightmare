using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Core.Infrastructures
{
	public delegate void ActiveViewModelEventHandler(ActiveViewModelEventArgs args);

	public delegate void DeActiveViewModelEventHandler(string perspectiveName);

	/// <summary>
	/// 
	/// </summary>
	public interface IUxManager
	{


		#region イベント

		event ActiveViewModelEventHandler OnActiveViewModel;
		event DeActiveViewModelEventHandler OnDeActiveViewModel;

		#endregion イベント


		#region メソッド

		void ChangePerspective(string perspectiveName, object param = null);

		#endregion メソッド

	}

	public class ActiveViewModelEventArgs : EventArgs
	{
		public string PerspectiveName { get; set; }
		public object Param { get; set; }
	}
}
