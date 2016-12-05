using Akalib.Wpf;
using log4net;
using Sakura.Core;
using Sakura.Core.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sakura.Data.ViewModel
{
	public class ContentPreviewViewModel : SDocumentViewModelBase
	{


		#region フィールド

		public string _Title = "TestTitle";

		/// <summary>
		/// 画像の拡大縮小時の、リサイズ倍率
		/// </summary>
		/// <remarks>
		/// この値を大きくすると、少ないスクロール量でもスケールの変動が大きな(大胆に)変化となります。
		/// この値を小さくすると、スクロール量に対してスクロールの変動が小さな変化となります。
		/// </remarks>
		const double repeatValue = 0.008;

		/// <summary>
		/// 最小スケール
		/// </summary>
		const double SCALE_MIN = 0.01;


		static ILog LOG = LogManager.GetLogger(typeof(ContentPreviewViewModel));

		/// <summary>
		/// 
		/// </summary>
		int _CurrentImagePosition;

		/// <summary>
		/// 
		/// </summary>
		Point _dragOffset;

		/// <summary>
		/// 画像フィットモード
		/// </summary>
		bool _EnabledImageFitMode = false;

		/// <summary>
		/// 表示する画像データ
		/// </summary>
		BitmapSource _ImageBitmap = null;

		/// <summary>
		/// 画像を表示するキャンバスサイズ
		/// </summary>
		Size _ImageCanvasSize = new Size();

		double _ImageOffsetX = 0.0;

		double _ImageOffsetY = 0.0;

		double _ImageScaleX = 1.0;

		double _ImageScaleY = 1.0;

		double _ImageScrollViwerVerticalOffset;

		/// <summary>
		/// 縮小・拡大モード
		/// </summary>
		Stretch _ImageStretch = Stretch.None;

		/// <summary>
		/// 
		/// </summary>
		Transform _ImageTransform = null;

		bool _IsInitialized = false;

		/// <summary>
		///  キャンバス内でのドラッグ操作中かどうかを示すフラグです。
		/// </summary>
		bool IsImageCanvasDragging = false;

		double oldImageOffsetX = 0.0;

		double oldImageOffsetY = 0.0;

		#endregion フィールド

		#region プロパティ

		/// <summary>
		/// 画像フィットモードを取得します
		/// </summary>
		/// <remarks>
		/// このフラグがTrueの場合、キャンバスのサイズに画像がフィットするように拡大（または、縮小）します。
		/// </remarks>
		public bool EnabledImageFitMode
		{
			get
			{ return _EnabledImageFitMode; }
			private set
			{
				if (_EnabledImageFitMode == value)
					return;
				_EnabledImageFitMode = value;
				RaisePropertyChanged();
			}
		}


		/// <summary>
		/// 表示する画像を取得します
		/// </summary>
		/// <remarks>
		/// LoadImage()で画像を読み込むまでは、このプロパティはNULLを返します。
		/// </remarks>
		public BitmapSource ImageBitmap
		{
			get { return _ImageBitmap; }
			internal set
			{
				_ImageBitmap = value;
				RaisePropertyChanged();
			}
		}

		public double ImageOffsetX
		{
			get { return _ImageOffsetX; }
			set
			{
				if (Equals(_ImageOffsetX, value)) return;
				_ImageOffsetX = value;
				RaisePropertyChanged();
			}
		}

		public double ImageOffsetY
		{
			get { return _ImageOffsetY; }
			set
			{
				if (Equals(_ImageOffsetY, value)) return;
				_ImageOffsetY = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// 表示する画像のスケール(X軸)
		/// </summary>
		public double ImageScaleX
		{
			get { return _ImageScaleX; }
			set
			{
				if (Equals(_ImageScaleX, value)) return;
				_ImageScaleX = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// 表示する画像のスケール(Y軸)
		/// </summary>
		public double ImageScaleY
		{
			get { return _ImageScaleY; }
			set
			{
				if (Equals(_ImageScaleY, value)) return;
				_ImageScaleY = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// 画像を表示しているScrollViewerの縦方向のスクロール位置です。
		/// スクロールの制御にはコードビハインドが働いているので、このプロパティを使ってむやみにスクロール位置を変更しないように。
		/// </summary>
		public double ImageScrollViwerVerticalOffset
		{
			get
			{ return _ImageScrollViwerVerticalOffset; }
			set
			{
				if (_ImageScrollViwerVerticalOffset == value)
					return;
				_ImageScrollViwerVerticalOffset = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Stretch ImageStretch
		{
			get { return _ImageStretch; }
			set
			{
				if (Equals(_ImageStretch, value)) return;
				_ImageStretch = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// 表示している画像のスケーリング方法を取得、または設定します。
		/// </summary>
		public Transform ImageTransform
		{
			get { return _ImageTransform; }
			set
			{
				if (Equals(_ImageTransform, value)) return;
				_ImageTransform = value;
				RaisePropertyChanged();
			}
		}

		public string Title { get { return _Title; } }

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// [デバッグ用]
		/// </summary>
		public void __DebugImageLoad()
		{
			MemoryStream data = new MemoryStream(File.ReadAllBytes(@"C:\016_47743597_p7_master1200_72dpi.jpg"));
			//MemoryStream data = new MemoryStream(File.ReadAllBytes(@"C:\016_47743597_p7_master1200_200dpi.jpg"));
			//MemoryStream data = new MemoryStream(File.ReadAllBytes(@"C:\016_47743597_p7_master1200.jpg"));
			var x = BitmapFrame.Create(data);
			WriteableBitmap wbmp = new WriteableBitmap(x);

			this.ImageBitmap = wbmp;

			// 画像サイズ調整
			if (this.EnabledImageFitMode)
			{
				FittingViewSize(this._ImageCanvasSize.Width, this._ImageCanvasSize.Height);
			}
			else
			{
				this.ImageStretch = Stretch.None;

				UpdateImageTransform();
			}
		}



		/// <summary>
		/// スケーリングを使用し、リサイズした場合の画像サイズを計算します
		/// </summary>
		/// <returns>リサイズ後の画像サイズ</returns>
		public Size CurrentScaledImageSize()
		{
			return new Size(
				this.ImageBitmap.PixelWidth * this.ImageScaleX,
				this.ImageBitmap.PixelHeight * this.ImageScaleY
				);
		}

		/// <summary>
		/// 画像のマウスドラッグを終了する
		/// </summary>
		/// <param name="e"></param>
		public async void FinishImageDrag(MouseButtonEventArgs e)
		{
			var imageControl = e.Source as Image;
			imageControl.ReleaseMouseCapture();
			IsImageCanvasDragging = false;
		}


		/// <summary>
		/// 画像のマウスドラッグ中に、画像の位置をドラッグに合わせて変更する
		/// </summary>
		/// <param name="e"></param>
		public async void ImageDragging(MouseEventArgs e)
		{
			// キャンバスのドラッグ中ならば、
			// 現在のマウス座標との差分を取って、Imageコントロールを表示する位置をScrollViewerに設定する。

			if (IsImageCanvasDragging)
			{
				var imageControl = e.Source as Image;

				var @scroll = imageControl.Parent.FindAncestor<ScrollViewer>();
				Point pt = Mouse.GetPosition(@scroll);
				double diffX = pt.X - _dragOffset.X;
				double diffY = pt.Y - _dragOffset.Y;


				// ImageOffsetX/ImageOffsetYはMVVMで
				// ScrollViewerのOffset値とバインドしている。
				this.ImageOffsetX = oldImageOffsetX - diffX;
				this.ImageOffsetY = oldImageOffsetY - diffY;
			}
		}

		/// <summary>
		/// 指定した領域内に画像が表示されるように画像の縮小スケールを設定します
		/// </summary>
		/// 
		/// <param name="width">キャンバスの幅。画像を表示したい領域の横幅。</param>
		/// <param name="height">キャンバスの高さ。画像を表示したい領域の縦幅。</param>
		void FittingViewSize(double width, double height)
		{
			double v = 1.0;
			if (ImageBitmap == null) return;

			double len = 0;

			if (ImageBitmap.PixelHeight >= ImageBitmap.PixelWidth)
			{
				// 縦幅が大きい
				if (height < ImageBitmap.PixelHeight)
				{
					v = height / ImageBitmap.PixelHeight;
				}

				double cheight = v * ImageBitmap.PixelHeight;

				while (cheight > height - 20.0)
				{
					v -= 0.002; // 少しずつ小さくしていく
					cheight = v * ImageBitmap.PixelHeight;
				}

				len = cheight;
			}
			else
			{
				// 横幅が大きい
				if (width < ImageBitmap.PixelWidth)
				{
					v = width / ImageBitmap.PixelWidth;
				}

				double cwidth = v * ImageBitmap.PixelWidth;

				// 縮小時のサイズがキャンバスサイズ(width)に収まるまで
				// 縮小スケール値を減少させていく。(「width-20」はImageコントロールに設定しているMarginの値)
				while (cwidth > width - 40.0)
				{
					v -= 0.002; // 少しずつ小さくしていく
					cwidth = v * ImageBitmap.PixelWidth;
				}

				len = cwidth;
			}

			ImageScaleX = ImageScaleY = v;

			// *** DEBUG PRINT ***
			//string msg = string.Format("画像ピクセル数(DPI変換後) {0}x{1} (DPI: {2}x{3}) LEN={4}", ImageBitmap.PixelWidth, ImageBitmap.PixelHeight, ImageBitmap.DpiX , ImageBitmap.DpiY, len);
			//LOG.DebugFormat(msg);
			//var scaledSize = CurrentScaledImageSize();
			//LOG.DebugFormat("スケーリング済みサイズ {0}x{1} (CanvasSize {2}x{3}) [{4}x{4}]", scaledSize.Width, scaledSize.Height, width, height, v);

			UpdateImageTransform();
		}


		/// <summary>
		/// 
		/// </summary>
		void UpdateImageTransform()
		{
			// WPFスケール環境での伸縮率を計算します(ScaleTransformはWPF空間)(ImageDPI / DeviceDPI)
			var dpi_x_scale = (ImageBitmap.DpiX / ApplicationContext.CurrentDpi.X);
			var dpi_y_scale = (ImageBitmap.DpiY / ApplicationContext.CurrentDpi.Y);

			var scaleTransform = new ScaleTransform(this.ImageScaleX * dpi_x_scale, this.ImageScaleY * dpi_y_scale);
			scaleTransform.CenterX = this.ImageBitmap.PixelWidth / 2.0;
			scaleTransform.CenterY = this.ImageBitmap.PixelHeight / 2.0;
			this.ImageTransform = scaleTransform;
		}

		public async void OnGridSizeChanged(SizeChangedEventArgs e)
		{
			// リサイズ後のコンテナのサイズを取得
			double lastHeight = e.NewSize.Height;
			double lastWidth = e.NewSize.Width;

			//LOG.InfoFormat("サイズ変更イベント2 Height={0} Width={1}", lastHeight, lastWidth);

			this._ImageCanvasSize = new Size(lastWidth, lastHeight);

			if (ImageBitmap != null && this.EnabledImageFitMode) // 画像が読み込み済みの場合は、画像を表示領域にフィットさせる。
			{
				FittingViewSize(lastWidth, lastHeight);
			}
		}


		public async void OnGridLoaded(RoutedEventArgs e)
		{
			var gridControl = e.Source as Grid;
			//LOG.InfoFormat("ImageGridLoadedCommandコマンドの呼び出し");
			//LOG.InfoFormat("Gridのサイズ {0} {1}", gridControl.ActualWidth, gridControl.ActualHeight);

			// コンテナが設置された際のコンテナのサイズをActualWidth/ActualHeightで取得可能。
			this._ImageCanvasSize = new Size(gridControl.ActualWidth, gridControl.ActualHeight);
			//this.EnabledImageFitMode = true; // ←変数は、画面出力前に外部から設定してください。

			if (ImageBitmap != null && this.EnabledImageFitMode) // 画像が読み込み済みの場合は、画像を表示領域にフィットさせる。
			{
				FittingViewSize(gridControl.ActualWidth, gridControl.ActualHeight);
			}
		}

		/// <summary>
		/// 画像表示キャンバスに合わせて画像の読み込みを行います
		/// </summary>
		/// <param name="e"></param>
		public async void InitializeLoad(RoutedEventArgs e)
		{
			var imageControl = e.Source as Image;
			dynamic prop = imageControl.Parent;
			var grid = prop.Parent as Grid;
			if (grid != null)
			{
				this._ImageCanvasSize.Height = grid.ActualHeight;
				this._ImageCanvasSize.Width = grid.ActualWidth;
			}

			// 画面表示時、画像ファイルの読み込みを行う場合の処理
			//if (this.PreviewImageArtifact != null)
			//	await LoadImageAsync(this.PreviewImageArtifact.Id); // コンポーネント表示時に表示される画像の読み込み
			__DebugImageLoad();

			_IsInitialized = true;
		}

		public override void OnActiveViewModel(string perspectiveName)
		{
			LOG.DebugFormat("Execute {0}", this.CurrentPerspectiveName);
		}

		/// <summary>
		///     キャンバスのMouseLeftButtonDownイベントのハンドラ
		/// </summary>
		/// <param name="e"></param>
		public async void OnCanvasMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			// ドラッグ開始
			var imageControl = e.Source as Image;

			IsImageCanvasDragging = true;

			// ドラッグ開始時のImageコントロールのオフセット値を保持
			oldImageOffsetX = ImageOffsetX;
			oldImageOffsetY = ImageOffsetY;

			var @scroll = imageControl.Parent.FindAncestor<ScrollViewer>();
			Point pt = Mouse.GetPosition(@scroll);
			_dragOffset = pt;
			imageControl.CaptureMouse();
		}

		/// <summary>
		///     キャンバスのMouseLeftButtonUpイベントのハンドラ
		/// </summary>
		/// <param name="e"></param>
		public async void OnCanvasMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			// ドラッグ終了
			var imageControl = e.Source as Image;
			imageControl.ReleaseMouseCapture();
			IsImageCanvasDragging = false;
		}
		/// <summary>
		///     キャンバスのMouseMoveイベントのハンドラ
		/// </summary>
		/// <param name="e"></param>
		public async void OnCanvasMouseMove(MouseEventArgs e)
		{
			// キャンバスのドラッグ中ならば、
			// 現在のマウス座標との差分を取って、Imageコントロールを表示する位置をScrollViewerに設定する。

			if (IsImageCanvasDragging)
			{
				var imageControl = e.Source as Image;

				var @scroll = imageControl.Parent.FindAncestor<ScrollViewer>();
				Point pt = Mouse.GetPosition(@scroll);
				double diffX = pt.X - _dragOffset.X;
				double diffY = pt.Y - _dragOffset.Y;


				// ImageOffsetX/ImageOffsetYはMVVMで
				// ScrollViewerのOffset値とバインドしている。
				this.ImageOffsetX = oldImageOffsetX - diffX;
				this.ImageOffsetY = oldImageOffsetY - diffY;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public async void OnCanvasSizeChanged(SizeChangedEventArgs e)
		{
			var imageControl = e.Source as Image;
			dynamic prop = imageControl.Parent;
			var grid = prop.Parent as Grid;
			if (grid != null)
			{
				this._ImageCanvasSize.Height = grid.ActualHeight;
				this._ImageCanvasSize.Width = grid.ActualWidth;
			}
		}

		public override void OnDeActiveViewModel(string perspectiveName)
		{
			LOG.DebugFormat("Execute {0}", this.CurrentPerspectiveName);
		}

		#endregion メソッド

	}
}
