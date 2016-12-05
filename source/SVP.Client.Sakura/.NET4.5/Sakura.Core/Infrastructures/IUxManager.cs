using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Core.Infrastructures
{
	public delegate void ActiveViewModelEventHandler(string perspectiveName);

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

		void ChangePerspective(string perspectiveName);

		#endregion メソッド

	}
}
