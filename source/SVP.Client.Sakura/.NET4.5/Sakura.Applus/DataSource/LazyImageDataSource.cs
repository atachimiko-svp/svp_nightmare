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

		/// <summary>
		/// 画像のキャッシュを行ったかどうかのフラグ
		/// </summary>
		/// <remarks>
		/// 読み込みに失敗していても、このフラグをTrueに設定すべきです。
		/// </remarks>
		public bool IsCached = false;

		/// <summary>
		/// コンテントID
		/// </summary>
		long _ContentId;

		/// <summary>
		/// プレビュー表示用ビットマップ
		/// </summary>
		BitmapSource _PreviewBitmap;

		/// <summary>
		/// 画像のサムネイルビットマップ
		/// </summary>
		BitmapSource _Thumbnail;
		/// <summary>
		/// 画像のタイトル
		/// </summary>
		string _Title;

		#endregion フィールド


		#region コンストラクタ

		public ImageLazyItem(long contentId)
		{
			this.ContentId = contentId;
		}

		#endregion コンストラクタ


		#region プロパティ

		public long ContentId
		{
			get { return _ContentId; }
			private set { _ContentId = value; }
		}

		public BitmapSource PreviewBitmap
		{
			get { return _PreviewBitmap; }
			private set
			{
				if (_PreviewBitmap == value) return;
				_PreviewBitmap = value;
				RaisePropertyChanged();
			}
		}

		public BitmapSource Thumbnail
		{
			get
			{ return _Thumbnail; }
			private set
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

		public void ClearPreviewBitmap()
		{
			this.PreviewBitmap = null;
		}

		public override void LoadedFromData(ServerData loadedData, bool force)
		{
			if (IsCached) return; // 読み込み済みの場合は、再度読み込みは行わない。
			IsCached = true;

			if (loadedData.UpdateFlag && loadedData.ThumbnailBytes != null)
			{
				using (MemoryStream ms = new MemoryStream(loadedData.ThumbnailBytes))
				{
					ms.Seek(0, SeekOrigin.Begin); // 念のためカーソル位置を先頭番地に移動
					BitmapImage bi = new BitmapImage();
					bi.BeginInit();
					bi.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
					bi.CacheOption = BitmapCacheOption.OnLoad;
					bi.DecodePixelHeight = 80;
					bi.StreamSource = ms;
					bi.EndInit();
					bi.Freeze();
					this.Thumbnail = bi;
				}
			}
			
		}

		public override void Unload()
		{
			this.Thumbnail = null;
			this.IsCached = false;
			base.Unload();
		}
		public void UpdatePreviewBitmap(byte[] bitmapbytes)
		{
			using (MemoryStream ms = new MemoryStream(bitmapbytes))
			{
				ms.Seek(0, SeekOrigin.Begin); // 念のためカーソル位置を先頭番地に移動
				BitmapImage bi = new BitmapImage();
				bi.BeginInit();
				bi.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
				bi.CacheOption = BitmapCacheOption.OnLoad;
				bi.StreamSource = ms;
				bi.EndInit();
				bi.Freeze();
				this.PreviewBitmap = bi;
			}
		}

		#endregion メソッド

	}

	public class LazyImageDataSource : LazyDataSourceBase<ImageLazyItem, ServerData>
	{


		#region メソッド

		protected override async Task<ServerData> GetData(ImageLazyItem sender, bool force)
		{
			bool isLoadServer = false;

			// [サーバからのデータ取得条件]
			// サーバからデータロードしていない or 
			// サーバからデータロードしているが強制読み込み or
			// サーバからデータロードしているが、キャッシュクリアされている
			if (!sender.IsCached) isLoadServer = true;
			if (sender.IsLoaded && force) isLoadServer = true;
			if (sender.IsLoaded && sender.IsCached) isLoadServer = true;

			if (isLoadServer)
			{
				return await Task.Delay(0).ContinueWith(_ =>
				{
					var rmd = new Random();
					Thread.Sleep(rmd.Next(200, 1000));

				return new ServerData
					{
						UpdateFlag = true,
						ThumbnailBytes = File.ReadAllBytes(@"C:\016_47743597_p7_master1200.jpg")
					};
				});
			}else
			{
				return await Task.Delay(0).ContinueWith(_ =>
			{
				var rmd = new Random();
					//Thread.Sleep(rmd.Next(200, 1000));

					return new ServerData
				{
					UpdateFlag = false,
				};
			});
			}
		}

		#endregion メソッド

	}


	/// <summary>
	/// 遅延読み込みでサーバから取得し、ImageLazyItemへ渡すデータ形式
	/// リファクタリング対象
	/// </summary>
	public class ServerData {


		#region フィールド

		public byte[] ThumbnailBytes;

		/// <summary>
		/// サーバから最新情報を取得したかどうかを示すフラグ
		/// </summary>
		public bool UpdateFlag;

		#endregion フィールド

	}
}
