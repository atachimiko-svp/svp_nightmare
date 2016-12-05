using log4net;
using Sakura.Data.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Data.ViewModel
{
	public class MDFSubViewModel : MDFViewModelBase
	{

		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(MDFSubViewModel));

		#endregion フィールド


		#region メソッド

		public override void OnActiveViewModel(string perspectiveName)
		{
			throw new NotImplementedException();
		}

		public override void OnDeActiveViewModel(string perspectiveName)
		{
			throw new NotImplementedException();
		}

		#endregion メソッド
	}
}
