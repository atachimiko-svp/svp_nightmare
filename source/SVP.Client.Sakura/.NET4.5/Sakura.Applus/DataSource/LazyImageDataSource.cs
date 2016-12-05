using Sakura.Contrib;
using Sakura.Core.Infrastructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Sakura.Applus.DataSource
{

	public class ImageLazyItem : LazyItemBase<ServerData>, IContentLazyItem
	{


		#region フィールド

		BitmapSource _Thumbnail;
		string _Title;
		/// <summary>
		/// 画像のキャッシュを行ったかどうかのフラグ
		/// </summary>
		/// <remarks>
		/// 読み込みに失敗していても、このフラグをTrueに設定すべきです。
		/// </remarks>
		private bool IsCached = false;

		#endregion フィールド


		#region プロパティ

		public BitmapSource Thumbnail
		{
			get
			{ return _Thumbnail; }
			set
			{
				if (_Thumbnail == value)
					return;
				_Thumbnail = value;
				RaisePropertyChanged();
			}
		}

		public string Title
		{
			get { return _Title; }
			set
			{
				if (_Title == value) return;
				_Title = value;
				RaisePropertyChanged();
			}
		}

		#endregion プロパティ


		#region メソッド

		public override void LoadedFromData(ServerData loadedData)
		{
			if (IsCached) return; // 読み込み済みの場合は、再度読み込みは行わない。
			IsCached = true;

			
			using (MemoryStream ms = new MemoryStream(loadedData.ThumbnailStream))
			{
				// めちゃくちゃメモリ使う(12MB)
				//var x = BitmapFrame.Create(data);
				//WriteableBitmap wbmp = new WriteableBitmap(x);
				//wbmp.Freeze();

				// 2MBくらい
				ms.Seek(0, SeekOrigin.Begin); // 念のためカーソル位置を先頭番地に移動
				BitmapImage bi = new BitmapImage();
				bi.BeginInit();
				bi.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
				bi.CacheOption = BitmapCacheOption.OnLoad;
				bi.StreamSource = ms;
				bi.EndInit();
				bi.Freeze();
				this.Thumbnail = bi;

				// ファイル直接指定(ファイルサイズとほぼ一致)
				//   ただし、ファイルロックする。
				//this.Thumbnail = new BitmapImage(new Uri(@"C:\016_47743597_p7_master1200.jpg")); ;

			}
			
		}

		public override void Unload()
		{
			this.Thumbnail = null;
		}

		#endregion メソッド

	}

	public class LazyImageDataSource : LazyDataSourceBase<ImageLazyItem, ServerData>
	{

		#region メソッド

		protected override async Task<ServerData> GetData()
		{
			return await Task.Delay(0).ContinueWith(_ =>
			{
				var rmd = new Random();
				Thread.Sleep(rmd.Next(200, 1000));

				//Console.WriteLine("画像の読み込みを行います");

				
				return new ServerData
				{
					ThumbnailStream = File.ReadAllBytes(@"C:\016_47743597_p7_master1200.jpg")
				};
			});
		}

		#endregion メソッド
	}


	/// <summary>
	/// 遅延読み込みでサーバから取得し、ImageLazyItemへ渡すデータ形式
	/// リファクタリング対象
	/// </summary>
	public class ServerData {
		public byte[] ThumbnailStream;
	}
}
