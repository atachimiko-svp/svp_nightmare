using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Core.Infrastructures
{
	public delegate void ChangeSelectedItemEventHandler(object sender, ChangeSelectedItemEventArgs eventargs);

	public interface IContentRepository
	{


		#region イベント

		event ChangeSelectedItemEventHandler ChangeSelectedItem;

		#endregion イベント


		#region プロパティ

		IReadOnlyCollection<IContentLazyItem> Items { get; }

		IContentLazyItem SelectedItem { get; }

		#endregion プロパティ


		#region メソッド

		Task<bool> LoadContentData(IContentLazyItem content);

		#endregion メソッド
	}

	public class ChangeSelectedItemEventArgs : EventArgs
	{


		#region プロパティ

		public IContentLazyItem NewItem { get; set; }
		public IContentLazyItem OldItem { get; set; }

		#endregion プロパティ

	}
}
