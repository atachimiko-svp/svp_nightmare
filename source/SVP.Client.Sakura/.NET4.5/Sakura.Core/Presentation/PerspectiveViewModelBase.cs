using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Core.Presentation
{
	public abstract class PerspectiveViewModelBase : Livet.ViewModel
	{


		#region フィールド

		protected string CurrentPerspectiveName = string.Empty;

		#endregion フィールド


		#region メソッド

		public void ActiveViewModel(string perspectiveName)
		{
			OnActiveViewModel(perspectiveName);
			this.CurrentPerspectiveName = perspectiveName;
		}

		public void DeActiveViewModel(string perspectiveName)
		{
			OnDeActiveViewModel(perspectiveName);
			this.CurrentPerspectiveName = string.Empty;
		}

		public abstract void OnActiveViewModel(string perspectiveName);
		public abstract void OnDeActiveViewModel(string perspectiveName);

		public void StartPerspective(string perspectiveName)
		{
			ApplicationContext.Ux.ChangePerspective(perspectiveName);
		}

		#endregion メソッド

	}
}
