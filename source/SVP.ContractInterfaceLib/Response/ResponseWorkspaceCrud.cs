using SVP.CIL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVP.CIL.Response
{
	public class ResponseWorkspaceCrud : ResponseBase
	{
		#region プロパティ

		public Workspace Data { get; set; }

		#endregion プロパティ
	}
}
