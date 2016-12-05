using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Core.Presentation
{
	public class Perspective
	{


		#region フィールド

		readonly string _PerspectiveName;
		Dictionary<string, PerspectiveViewModelBase> _PerspectiveVmDict = new Dictionary<string, PerspectiveViewModelBase>();

		#endregion フィールド


		#region コンストラクタ

		public Perspective(string perspectiveName)
		{
			this._PerspectiveName = perspectiveName;
		}

		#endregion コンストラクタ


		#region プロパティ

		public string PerspectiveName { get { return _PerspectiveName; } }

		public Dictionary<string, PerspectiveViewModelBase> PerspectiveVmDict { get { return _PerspectiveVmDict; } }

		#endregion プロパティ
	}
}
