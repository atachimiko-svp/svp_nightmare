using Sakura.Core;
using Sakura.Core.Infrastructures;
using Sakura.Core.Presentation;
using Sakura.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Applus.Manager
{
	public class UxManager : IUxManager
	{


		#region Private フィールド

		Perspective _CurrentPerspective = null;

		List<Perspective> _PerspectiveList = new List<Perspective>();

		List<PerspectiveViewModelBase> _ViewModels = new List<PerspectiveViewModelBase>();

		#endregion Private フィールド


		#region Public コンストラクタ

		public UxManager()
		{
			var doc1 = new ContentListViewModel();
			var doc2 = new ContentPreviewViewModel();
			var pane1 = new LabelTreeExplorerViewModel();

			_ViewModels.Add(doc1);
			_ViewModels.Add(doc2);
			_ViewModels.Add(pane1);

			// パースペクティブ初期化
			// 非表示にするものは、明示的にNULLを設定します。
			var p1 = new Perspective(Enum.GetName(typeof(PerspectiveNames), PerspectiveNames.ContentList_Thumbnail));
			p1.PerspectiveVmDict.Add("Document", doc1);
			p1.PerspectiveVmDict.Add("FL1", pane1);
			p1.PerspectiveVmDict.Add("DockRight", null);
			this._PerspectiveList.Add(p1);

			var p2 = new Perspective(Enum.GetName(typeof(PerspectiveNames), PerspectiveNames.Preview));
			p2.PerspectiveVmDict.Add("Document", doc2);
			p2.PerspectiveVmDict.Add("FL1", null);
			p2.PerspectiveVmDict.Add("DockRight", null);
			this._PerspectiveList.Add(p2);
		}

		#endregion Public コンストラクタ


		#region Public イベント

		public event ActiveViewModelEventHandler OnActiveViewModel;
		public event DeActiveViewModelEventHandler OnDeActiveViewModel;

		#endregion Public イベント


		#region Public メソッド

		public void ChangePerspective(string perspectiveName, object param = null)
		{
			// Guard
			var pers = GetPerspective(perspectiveName);
			if (pers == null)
				throw new ApplicationException();

			if (OnDeActiveViewModel != null)
				OnDeActiveViewModel(perspectiveName);

			// イベントハンドラ解除
			if (_CurrentPerspective != null)
			{

				foreach (var pp in _CurrentPerspective.PerspectiveVmDict)
				{
					if (pp.Value != null)
					{
						OnActiveViewModel -= pp.Value.ActiveViewModel;
						OnDeActiveViewModel -= pp.Value.DeActiveViewModel;
					}
				}
			}

			_CurrentPerspective = pers;
			foreach (var pp in pers.PerspectiveVmDict)
			{

				if (pp.Value != null)
				{
					OnActiveViewModel += pp.Value.ActiveViewModel;
					OnDeActiveViewModel += pp.Value.DeActiveViewModel;
				}

				ApplicationContext.Workspace.AttachViewModel(pp.Key, pp.Value);
			}

			if (OnActiveViewModel != null)
				OnActiveViewModel(new ActiveViewModelEventArgs
				{
					PerspectiveName = perspectiveName,
					Param = param
				});
		}

		#endregion Public メソッド


		#region Private メソッド

		Perspective GetPerspective(string perspectiveName)
		{
			var r = from v in _PerspectiveList
					where v.PerspectiveName == perspectiveName
					select v;
			return r.FirstOrDefault();
		}

		#endregion Private メソッド

	}
}
