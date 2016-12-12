using Sakura.Core.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Data.Infrastructures
{
	public interface IPerspectiveP2Arg
	{
		IContentLazyItem Content { get; set; }
	}
}
