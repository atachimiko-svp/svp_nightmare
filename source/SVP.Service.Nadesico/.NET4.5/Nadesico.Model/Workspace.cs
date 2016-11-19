using SVP.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Model
{
	[Table("svp_Workspace")]
	public class Workspace : ModelBase, IWorkspace
	{

		#region プロパティ

		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string PhysicalSpacePath { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string VirtualSpacePath { get; set; }

		#endregion プロパティ

	}
}
