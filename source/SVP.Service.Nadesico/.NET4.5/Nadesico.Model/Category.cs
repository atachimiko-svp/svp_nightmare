using Livet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Model
{
	[Table("svp_Category")]
	public class Category : ModelBase
	{


		#region フィールド

		private IList<Category> _ChildCategories;

		IList<Label> _Labels;

		Content _LastSelectedContent;

		private Category _ParentCategory;

		#endregion フィールド


		#region コンストラクタ

		public Category()
		{
			ChildCategories = new ObservableSynchronizedCollection<Category>();
			Labels = new ObservableSynchronizedCollection<Label>();
		}

		#endregion コンストラクタ


		#region プロパティ

		public virtual IList<Category> ChildCategories
		{
			get
			{ return _ChildCategories; }
			set
			{
				if (_ChildCategories == value)
					return;
				_ChildCategories = value;
			}
		}

		public string Comment { get; set; }

		public DateTime? CreateDate { get; set; }

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

		public virtual Content LastSelectedContent
		{
			get
			{
				return _LastSelectedContent;
			}
			set
			{
				_LastSelectedContent = value;

			}
		}

		public DateTime? LastUpDate { get; set; }

		public string Memo { get; set; }

		public string Name { get; set; }

		public OrderType OrderType { get; set; }

		public virtual Category ParentCategory
		{
			get { return _ParentCategory; }
			set { _ParentCategory = value; }
		}

		public CategorySortType SortType { get; set; }

		#endregion プロパティ

	}
}
