using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVP.CIL.Domain
{
	public class Workspace
	{


		#region プロパティ

		public long Id { get; set; }
		public string Name { get; set; }
		public string PhysicalSpacePath { get; set; }
		public string VirtualSpacePath { get; set; }

		#endregion プロパティ

	}
}
