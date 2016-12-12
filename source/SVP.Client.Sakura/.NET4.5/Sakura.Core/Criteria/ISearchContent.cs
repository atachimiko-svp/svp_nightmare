using SVP.CIL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Core.Criteria
{
	public interface ISearchContent
	{
		Task ExecFindByCategory(Category category);
	}
}
