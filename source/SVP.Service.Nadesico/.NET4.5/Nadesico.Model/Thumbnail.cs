using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Model
{
	[Table("svp_Thumbnail")]
	public class Thumbnail : ModelBase
	{

		#region フィールド

		private byte[] _BitmapBytes;
		private string _ThumbnailKey;

		#endregion フィールド



		#region プロパティ

		public byte[] BitmapBytes
		{
			get { return _BitmapBytes; }
			set
			{
				_BitmapBytes = value;
			}
		}

		public string ThumbnailKey
		{
			get { return _ThumbnailKey; }
			set
			{
				if (_ThumbnailKey == value) return;
				_ThumbnailKey = value;
			}
		}

		#endregion プロパティ


	}
}
