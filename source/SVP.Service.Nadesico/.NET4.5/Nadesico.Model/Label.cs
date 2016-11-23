using Livet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Model
{
	[Table("svp_Label")]
	public class Label : ModelBase
	{


		#region フィールド

		private IList<Category> _Categories;
		private IList<Label> _ChildLabels;

		private IList<Content> _Contents;
		Label _ParentLabel;

		#endregion フィールド


		#region コンストラクタ

		public Label()
		{
			ChildLabels = new ObservableSynchronizedCollection<Label>();
			Contents = new ObservableSynchronizedCollection<Content>();
			Categories = new ObservableSynchronizedCollection<Category>();
		}

		#endregion コンストラクタ


		#region プロパティ

		public virtual IList<Category> Categories
		{
			get
			{ return _Categories; }
			set
			{
				if (_Categories == value)
					return;
				_Categories = value;
			}
		}

		public virtual IList<Label> ChildLabels
		{
			get
			{ return _ChildLabels; }
			set
			{
				if (_ChildLabels == value)
					return;
				_ChildLabels = value;
			}
		}

		public string Comment { get; set; }

		public virtual IList<Content> Contents
		{
			get
			{ return _Contents; }
			set
			{
				if (_Contents == value)
					return;
				_Contents = value;
			}
		}

		public DateTime? CreateDate { get; set; }

		public DateTime? LastUpDate { get; set; }

		public string Name { get; set; }

		public virtual Label ParentLabel
		{
			get { return _ParentLabel; }
			set { _ParentLabel = value; }
		}

		#endregion プロパティ

	}
}
