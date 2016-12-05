using Livet;
using Sakura.Contrib.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sakura.Contrib
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T">遅延読込により、サーバから取得するデータ型</typeparam>
	public abstract class LazyItemBase<T> : NotificationObject, ILazyLoadingItem<T>
		where T : new()
	{

		#region フィールド

		// IsLoadedプロパティとバインドするための添付プロパティ
		public static readonly DependencyProperty LoadedProperty =
			DependencyProperty.RegisterAttached(
				"Loaded",
				typeof(bool),
				typeof(LazyItemBase<T>),
				new PropertyMetadata(false));

		private bool _IsLoaded = false;

		#endregion フィールド

		#region イベント

		/// <summary>
		/// IsLoadedプロパティが読み込まれたときに発生するイベント
		/// </summary>
		public event EventHandler Loaded;

		#endregion イベント


		#region プロパティ

		public bool IsLoaded
		{
			get
			{
				if (!_IsLoaded)
					OnLoaded();
				_IsLoaded = true;
				return true;
			}
		}

		#endregion プロパティ


		#region メソッド

		public static bool GetLoaded(DependencyObject obj)
		{
			return (bool)obj.GetValue(LoadedProperty);
		}

		public static void SetLoaded(DependencyObject obj, bool value)
		{
			obj.SetValue(LoadedProperty, value);
		}

		/// <summary>
		/// DataSourceが遅延読込を行いサーバから取得したデータを設定するために呼び出します。
		/// </summary>
		/// <param name="loadedData"></param>
		public abstract void LoadedFromData(T loadedData);

		/// <summary>
		/// 
		/// </summary>
		public virtual void Unload()
		{
			if (this._IsLoaded)
			{
				this._IsLoaded = false;
			}
		}

		private void OnLoaded()
		{
			var h = this.Loaded;
			if (h != null)
			{
				h(this, EventArgs.Empty);
			}
		}

		#endregion メソッド

	}
}
