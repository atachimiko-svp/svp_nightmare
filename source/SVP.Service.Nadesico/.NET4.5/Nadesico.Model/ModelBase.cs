using Akalib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Model
{
	public abstract class ModelBase : IEntity<long>
	{


		#region プロパティ

		public long Id
		{
			get;
			set;
		}

		#endregion プロパティ

	}
}
