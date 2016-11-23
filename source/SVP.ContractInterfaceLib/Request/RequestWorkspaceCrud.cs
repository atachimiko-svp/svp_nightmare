using SVP.CIL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVP.CIL.Request
{
	public class RequestWorkspaceCrud
	{
		#region プロパティ

		public CrudType Crud { get; set; }

		public Workspace Target { get; set; }

		#endregion プロパティ
	}
}
