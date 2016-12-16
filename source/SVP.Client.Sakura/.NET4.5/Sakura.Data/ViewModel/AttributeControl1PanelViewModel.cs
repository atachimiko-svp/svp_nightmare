using log4net;
using Sakura.Core.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Data.ViewModel
{
	public class AttributeControl1PanelViewModel : PaneViewModelBase
	{
		static ILog LOG = LogManager.GetLogger(typeof(AttributeControl1PanelViewModel));

		#region Public メソッド

		public override void OnActiveViewModel(string perspectiveName, object param)
		{
			
		}

		public override void OnDeActiveViewModel(string perspectiveName)
		{
			
		}

		#endregion Public メソッド
	}
}
