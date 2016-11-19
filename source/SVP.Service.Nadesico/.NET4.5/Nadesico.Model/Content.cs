using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Model
{
	/// <summary>
	/// 
	/// </summary>
	[Table("svp_Content")]
	public class Content : ModelBase
	{


		#region プロパティ

		public bool ArchiveFlag { get; set; }
		public DateTime? AttachCategoryDate { get; set; }
		public string Caption { get; set; }
		public string Comment { get; set; }
		public string ContentHash { get; set; }
		public DateTime? CreateDate { get; set; }
		public string IdentifyKey { get; set; }
		public DateTime? LastUpDate { get; set; }
		public bool ReadableFlag { get; set; }
		public int Starrating { get; set; }
		public string ThumbnailHash { get; set; }
		public string Title { get; set; }
		public bool UnsetStarratingFlag { get; set; }

		#endregion プロパティ
	}
}
