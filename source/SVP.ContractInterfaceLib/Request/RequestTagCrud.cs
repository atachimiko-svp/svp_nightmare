using SVP.CIL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVP.CIL.Request
{
	public class RequestTagCrud
	{

		#region プロパティ

		public CrudType Crud { get; set; }

		public Tag Target { get; set; }

		#endregion プロパティ
	}
}
