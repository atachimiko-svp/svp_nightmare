using SVP.CIL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVP.CIL.Response
{
	public class ResponseTagLoadList : ResponseBase
	{
		#region プロパティ

		public IList<Tag> Datas { get; set; }

		#endregion プロパティ
	}
}
