using log4net;
using Sakura.Core;
using Sakura.Data.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sakura.Data.ViewModel
{
	public class MDFMainViewModel : MDFViewModelBase
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(MDFMainViewModel));




		#endregion フィールド


		#region プロパティ


		#endregion プロパティ


		#region メソッド

		public override void OnActiveViewModel(string perspectiveName, object param)
		{
			LOG.DebugFormat("Execute {0}", this.CurrentPerspectiveName);
		}

		public override void OnDeActiveViewModel(string perspectiveName)
		{
			LOG.DebugFormat("Execute {0}", this.CurrentPerspectiveName);
		}



		#endregion メソッド
	}
}
