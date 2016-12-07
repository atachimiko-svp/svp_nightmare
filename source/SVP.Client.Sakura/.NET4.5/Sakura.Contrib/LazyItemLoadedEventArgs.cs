using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Contrib
{
	public class LazyItemLoadedEventArgs : EventArgs
	{
		public bool ForceLoadFlag { get; set; }
	}
}
