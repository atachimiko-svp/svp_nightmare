using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Core.Presentation
{
	public abstract class PaneViewModelBase : PerspectiveViewModelBase
	{

		#region フィールド

		private bool _IsOpen = true;

		#endregion フィールド


		#region プロパティ

		public bool IsOpen
		{
			get { return _IsOpen; }
			set
			{
				_IsOpen = value;
				RaisePropertyChanged();
			}
		}

		#endregion プロパティ
	}
}
