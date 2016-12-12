using log4net;
using Livet;
using Sakura.Core.Presentation;
using Sakura.Data.Struction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sakura.Core;

namespace Sakura.Data.ViewModel
{

	public class LabelTreeExplorerViewModel : PaneViewModelBase
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(LabelTreeExplorerViewModel));

		ObservableSynchronizedCollection<LabelTreeItem> _Items = new ObservableSynchronizedCollection<LabelTreeItem>();

		#endregion フィールド


		#region コンストラクタ

		public LabelTreeExplorerViewModel()
		{
			Items = ViewModelHelper.CreateReadOnlyDispatcherCollection<LabelTreeItem, LabelTreeItem>(
				_Items,
				i => i,
				DispatcherHelper.UIDispatcher
				);


			for (int i = 0; i < 30; i++)
			{
				var n = new LabelTreeItem { Name = "TreeItem " + i };

				n.Load();
				_Items.Add(n);
			}
		}

		#endregion コンストラクタ


		#region プロパティ

		public ReadOnlyDispatcherCollection<LabelTreeItem> Items { get; set; }

		public LabelTreeItem SelectedLabelTreeItem
		{
			get; set;
		}

		#endregion プロパティ


		#region メソッド

		public async void ItemExecute(LabelTreeItem item)
		{
			LOG.InfoFormat("LabelTreeExplorerViewModel {0}", item.Name);

			var param = new
			{
				CategoryId = 1L // TOOD: カテゴリIDもしくはラベルID
			};
			StartPerspective(PerspectiveNames.ContentList_Thumbnail, param);
		}

		public override void OnActiveViewModel(string perspectiveName, object param)
		{
			LOG.DebugFormat("Execute {0}", this.CurrentPerspectiveName);
		}

		public override void OnDeActiveViewModel(string perspectiveName)
		{
			LOG.DebugFormat("Execute {0}", this.CurrentPerspectiveName);
		}

		/// <summary>
		/// ラベル項目選択イベントハンドラ
		/// 
		/// </summary>
		public async void Selected()
		{
			LOG.Info("Selected");
		}

		#endregion メソッド

	}

}
