﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Model
{
	/// <summary>
	/// 
	/// </summary>
	[Table("svp_ServerPld")]
	public sealed class ServerPld : ModelBase
	{

		#region プロパティ

		[Required]
		public string Key { get; set; }

		public string Value { get; set; }

		#endregion プロパティ
	}
}
