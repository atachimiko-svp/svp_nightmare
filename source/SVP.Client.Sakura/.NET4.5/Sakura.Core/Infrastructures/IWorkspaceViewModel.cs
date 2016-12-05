using Sakura.Core.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Core.Infrastructures
{
	public interface IWorkspaceViewModel
	{


		#region メソッド

		void AttachViewModel(string position, PerspectiveViewModelBase perspectiveVm);

		#endregion メソッド

	}
}
