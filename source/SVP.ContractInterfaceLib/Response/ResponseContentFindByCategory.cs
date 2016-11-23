using SVP.CIL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVP.CIL.Response
{
	public class ResponseContentFindByCategory : ResponseBase
	{
		#region プロパティ

		public IList<Content> Datas { get; set; }

		#endregion プロパティ
	}
}
