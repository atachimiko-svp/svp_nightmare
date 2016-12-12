using Sakura.Core.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Core.Presentation
{
	public abstract class PerspectiveViewModelBase : Livet.ViewModel
	{


		#region Protected フィールド

		protected string CurrentPerspectiveName = string.Empty;

		#endregion Protected フィールド


		#region Public メソッド

		public void ActiveViewModel(ActiveViewModelEventArgs args)
		{
			OnActiveViewModel(args.PerspectiveName, args.Param);
			this.CurrentPerspectiveName = args.PerspectiveName;
		}

		public void DeActiveViewModel(string perspectiveName)
		{
			OnDeActiveViewModel(perspectiveName);
			this.CurrentPerspectiveName = string.Empty;
		}

		public abstract void OnActiveViewModel(string perspectiveName, object param);
		public abstract void OnDeActiveViewModel(string perspectiveName);

		public void StartPerspective(PerspectiveNames perspectiveName, object param = null)
		{
			var textName = Enum.GetName(typeof(PerspectiveNames), perspectiveName);
			ApplicationContext.Ux.ChangePerspective(textName, param);
		}

		#endregion Public メソッド

	}
}
