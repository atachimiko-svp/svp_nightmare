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

		private IList<Label> _ChildLabels;

		Label _ParentLabel;

		#endregion フィールド


		#region コンストラクタ

		public Label()
		{
			ChildLabels = new ObservableSynchronizedCollection<Label>();
		}

		#endregion コンストラクタ


		#region プロパティ

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
