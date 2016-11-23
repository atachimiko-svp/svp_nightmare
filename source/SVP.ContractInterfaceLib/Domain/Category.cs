using Nadesico.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVP.CIL.Domain
{
	public class Category
	{

		#region プロパティ

		public string Comment { get; set; }
		public DateTime? CreateDate { get; set; }
		public long Id { get; set; }
		public DateTime? LastUpDate { get; set; }
		public string Memo { get; set; }
		public string Name { get; set; }
		public OrderType OrderType { get; set; }
		public CategorySortType SortType { get; set; }

		#endregion プロパティ
	}
}
