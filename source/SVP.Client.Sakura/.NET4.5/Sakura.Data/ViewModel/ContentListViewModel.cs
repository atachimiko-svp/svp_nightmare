using ImpromptuInterface;
using log4net;
using Sakura.Core;
using Sakura.Core.Criteria;
using Sakura.Core.Infrastructures;
using Sakura.Core.Presentation;
using Sakura.Data.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Sakura.Data.ViewModel
{
	public class ContentListViewModel : SDocumentViewModelBase
	{


		#region フィールド

		long _Criteria_CategoryId = 0L;

		static ILog LOG = LogManager.GetLogger(typeof(ContentListViewModel));

		IReadOnlyCollection<IContentLazyItem> _Items;

		#endregion フィールド


		#region コンストラクタ

		public ContentListViewModel()
		{

		}

		#endregion コンストラクタ


		#region プロパティ

		public IReadOnlyCollection<IContentLazyItem> Items
		{
			get { return _Items; }
			private set
			{
				_Items = value;
				RaisePropertyChanged();
			}
		}

		#endregion プロパティ


		#region メソッド

		public async void ClearBitmapData()
		{
			// デバッグコードの残骸
			// メソッドは削除します。
		}

		/// <summary>
		/// 項目実行
		/// </summary>
		public async void ItemExecute(IContentLazyItem item)
		{
			// EMPTY
			LOG.InfoFormat("ItemExecute {0}",item.Title);

			var p = new { Content = item };
			StartPerspective(PerspectiveNames.Preview, p);
		}

		public override async void OnActiveViewModel(string perspectiveName, object param)
		{
			LOG.DebugFormat("Execute {0}", this.CurrentPerspectiveName);

			if (param != null)
			{
				var myInterface = param.ActLike<IPerspectiveP1Arg>();
				_Criteria_CategoryId = myInterface.CategoryId;

				await RunStartServerDataLoad();
			}
		}

		public override void OnDeActiveViewModel(string perspectiveName)
		{
			LOG.DebugFormat("Execute {0}", this.CurrentPerspectiveName);
		}

		/// <summary>
		/// サーバからのデータ読み込みを開始する
		/// </summary>
		public async void StartServerDataLoad()
		{
			LOG.InfoFormat("Execute StartServerDataLoad ContentId={0}", _Criteria_CategoryId);

			// Guard
			if (_Criteria_CategoryId == 0) return; // 対象カテゴリが未設定の場合、読み込みを行わない

			await RunStartServerDataLoad();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private async Task RunStartServerDataLoad()
		{
			await ((ISearchContent)ApplicationContext.ContentRepository).ExecFindByCategory(new SVP.CIL.Domain.Category
			{
				Id = _Criteria_CategoryId
			});
			this.Items = ApplicationContext.ContentRepository.Items;
		}
		#endregion メソッド

	}
}
