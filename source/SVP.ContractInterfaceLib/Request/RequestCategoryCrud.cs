using SVP.CIL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVP.CIL.Request
{
	public class RequestCategoryCrud
	{

		#region プロパティ

		public CrudType Crud { get; set; }

		public Category Target { get; set; }

		#endregion プロパティ
	}
}
