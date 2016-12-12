using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Sakura.Core.Infrastructures
{
	public interface IContentLazyItem
	{


		#region プロパティ

		long ContentId { get; }

		BitmapSource PreviewBitmap { get; }

		BitmapSource Thumbnail { get; }

		string Title { get; }

		#endregion プロパティ


		#region メソッド

		void ClearPreviewBitmap();

		void UpdatePreviewBitmap(byte[] bitmapbytes);

		#endregion メソッド

	}
}
