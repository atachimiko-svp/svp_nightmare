using Livet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Model
{
	[Table("svp_Tag")]
	public class Tag : ModelBase
	{


		#region フィールド

		private IList<Tag> _ChildTags;

		private Tag _ParentTag;

		#endregion フィールド


		#region コンストラクタ

		public Tag()
		{
			ChildTags = new ObservableSynchronizedCollection<Tag>();
		}

		#endregion コンストラクタ


		#region プロパティ

		public virtual IList<Tag> ChildTags
		{
			get
			{ return _ChildTags; }
			set
			{
				if (_ChildTags == value)
					return;
				_ChildTags = value;
			}
		}

		public string Comment { get; set; }

		public DateTime? CreateDate { get; set; }

		public DateTime? LastUpDate { get; set; }

		public string Name { get; set; }

		public virtual Tag ParentTag
		{
			get { return _ParentTag; }
			set { _ParentTag = value; }
		}

		#endregion プロパティ

	}
}
