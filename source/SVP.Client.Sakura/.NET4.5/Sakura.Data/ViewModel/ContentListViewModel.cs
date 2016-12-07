using log4net;
using Sakura.Core;
using Sakura.Core.Infrastructures;
using Sakura.Core.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Sakura.Data.ViewModel
{
	public class ContentListViewModel : SDocumentViewModelBase
	{
		

		#region フィールド

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

		public override void OnActiveViewModel(string perspectiveName)
		{
			LOG.DebugFormat("Execute {0}", this.CurrentPerspectiveName);

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
			LOG.Info("Execute StartServerDataLoad");
			this.Items = ApplicationContext.ContentRepository.Items;
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		#endregion メソッド

	}
}
