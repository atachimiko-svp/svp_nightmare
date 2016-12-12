using Livet;
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

		#region フィールド

		private IList<Label> _Labels;

		private IList<Tag> _Tags;

		#endregion フィールド


		#region コンストラクタ

		public Content()
		{
			this.Tags = new ObservableSynchronizedCollection<Tag>();
			this.Labels = new ObservableSynchronizedCollection<Label>();
		}

		#endregion コンストラクタ


		#region プロパティ

		public virtual FileMappingInfo FileMappingInfo { get; set; }

		public bool ArchiveFlag { get; set; }

		public DateTime? AttachCategoryDate { get; set; }

		public string Caption { get; set; }

		public virtual Category Category { get; set; }

		public string Comment { get; set; }

		public string ContentHash { get; set; }

		public DateTime? CreateDate { get; set; }

		public string IdentifyKey { get; set; }

		public virtual IList<Label> Labels
		{
			get
			{ return _Labels; }
			set
			{
				if (_Labels == value)
					return;
				_Labels = value;
			}
		}

		public DateTime? LastUpDate { get; set; }

		public bool ReadableFlag { get; set; }

		public int Starrating { get; set; }

		public virtual IList<Tag> Tags
		{
			get
			{ return _Tags; }
			set
			{
				if (_Tags == value)
					return;
				_Tags = value;
			}
		}
		public string ThumbnailHash { get; set; }
		public string Title { get; set; }
		public bool UnsetStarratingFlag { get; set; }

		#endregion プロパティ

	}
}
