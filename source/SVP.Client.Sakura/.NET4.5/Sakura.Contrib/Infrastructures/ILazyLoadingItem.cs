using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Contrib.Infrastructures
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="SV"></typeparam>
	public interface ILazyLoadingItem<SV>
		where SV : new()
	{
		#region イベント

		/// <summary>
		/// IsLoadedプロパティ内で呼び出すイベントハンドラ
		/// </summary>
		event EventHandler Loaded;

		#endregion イベント

		#region プロパティ

		/// <summary>
		/// Bindingで遅延ロードを実行したかどうかの判定フラグを取得します
		/// </summary>
		bool IsLoaded { get; }

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// 遅延読み込みで取得したデータを引数にして呼び出します。実装により必要なデータをコピーしてください。
		/// </summary>
		/// <param name="loadedData"></param>
		void LoadedFromData(SV loadedData);

		#endregion メソッド
	}
}
