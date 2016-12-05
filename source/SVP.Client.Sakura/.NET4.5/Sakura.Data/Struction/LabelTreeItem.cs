using Livet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Data.Struction
{
	public class LabelTreeItem : Livet.NotificationObject
	{
		#region フィールド

		/// <summary>
		/// DataModeコレクション
		/// </summary>
		private readonly ObservableCollection<ServerLabelTestData> _children = new ObservableCollection<ServerLabelTestData>();

		/// <summary>
		/// タグの階層構造のための、子要素一覧コレクション
		/// </summary>
		/// <remarks>
		/// Halcyonが管理するタグDtaaModelの階層構造とは別です。
		/// この階層構造は、アプリケーションがツリーなどでタグ表示するための階層構造を構築します。
		/// </remarks>
		private readonly ReadOnlyDispatcherCollection<LabelTreeItem> _childrenro = null;

		/// <summary>
		/// 子要素がサーバから取得済みか示すフラグ
		/// </summary>
		private bool _isLoadedChildren = false;

		/// <summary>
		/// タグ名称
		/// </summary>
		private string _Name;

		/// <summary>
		/// タグのID
		/// </summary>
		private long _TagId;

		#endregion フィールド


		#region コンストラクタ

		public LabelTreeItem()
		{
			this._childrenro = ViewModelHelper.CreateReadOnlyDispatcherCollection<ServerLabelTestData, LabelTreeItem>(
				_children,
				prop => new LabelTreeItem
				{
					Name = prop.Name,
					HasChildServer = prop.IsChild,
					_TagId = prop.Id
				},
				DispatcherHelper.UIDispatcher);
		}

		#endregion コンストラクタ


		#region プロパティ

		/// <summary>
		/// TagTreeで表示する小階層の要素を取得します
		/// </summary>
		public ReadOnlyDispatcherCollection<LabelTreeItem> Children
		{
			get
			{
				if (!_isLoadedChildren)
				{
					_isLoadedChildren = true;
					if (HasChildServer)
						Load();
				}

				return _childrenro;
			}
		}

		/// <summary>
		/// 子要素があることをサーバから受けている場合、Trueを返す。
		/// </summary>
		public bool HasChildServer { get; set; }

		/// <summary>
		/// ラベルに表示されるラベル文字列
		/// </summary>
		public string Name
		{
			get
			{ return _Name; }
			set
			{
				if (_Name == value)
					return;
				_Name = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public long TagId { get { return _TagId; } }

		#endregion プロパティ


		#region メソッド

		public void Load(bool rootLoad = false)
		{
			// サーバからデータを取得してきた、というイメージ。
			// サーバからは、現在のタグの小階層タグの一覧とその小階層が更に小階層を持つかどうかのフラグを取得するような実装とする。

		}

		#endregion メソッド
	}

	/// <summary>
	/// サーバからデータを取得するという実装例で使用するデータ
	/// </summary>
	public class ServerLabelTestData
	{


		#region プロパティ

		public long Id { get; set; }
		public bool IsChild { get; set; }
		public string Name { get; set; }

		#endregion プロパティ

	}

}
