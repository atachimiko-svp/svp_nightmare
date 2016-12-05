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

		BitmapSource Thumbnail { get; }

		#endregion プロパティ
	}
}
