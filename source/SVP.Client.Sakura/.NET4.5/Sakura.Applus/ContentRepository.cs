using Sakura.Core.Criteria;
using Sakura.Core.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Sakura.Applus.DataSource;

namespace Sakura.Applus
{
	public class ContentRepository : ISearchContent, IContentRepository
	{


		#region フィールド

		LazyImageDataSource _DataSource = new LazyImageDataSource();

		IContentLazyItem _SelectedItem;

		#endregion フィールド


		#region コンストラクタ

		public ContentRepository()
		{
			for (int i = 1; i <= 300; i++)
			{
				this._DataSource.AddItem(new ImageLazyItem { Title = i.ToString() });
			}
		}

		#endregion コンストラクタ


		#region イベント

		public event ChangeSelectedItemEventHandler ChangeSelectedItem;

		#endregion イベント


		#region プロパティ

		public IReadOnlyCollection<IContentLazyItem> Items
		{
			get
			{
				return new List<IContentLazyItem>(_DataSource.Items.Cast<IContentLazyItem>());
			}
		}

		public IContentLazyItem SelectedItem
		{
			get
			{
				return _SelectedItem;
			}
			private set
			{
				var old = _SelectedItem;
				_SelectedItem = value;

				RaiseChangeSelectedItem(old, _SelectedItem);
			}
		}

		#endregion プロパティ


		#region メソッド

		public void NextContent()
		{

		}

		public void PrevContent()
		{

		}
		void RaiseChangeSelectedItem(IContentLazyItem old, IContentLazyItem newItem)
		{
			if (ChangeSelectedItem != null)
			{
				var args = new ChangeSelectedItemEventArgs { OldItem = old, NewItem = newItem };
				ChangeSelectedItem(this, args);
			}
		}

		#endregion メソッド

	}
}
