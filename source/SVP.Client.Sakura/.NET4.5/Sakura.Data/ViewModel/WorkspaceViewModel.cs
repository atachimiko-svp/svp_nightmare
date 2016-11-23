using log4net;
using Sakura.Core.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Data.ViewModel
{
	public class WorkspaceViewModel : Livet.ViewModel, IWorkspaceViewModel
	{
		static ILog LOG = LogManager.GetLogger(typeof(WorkspaceViewModel));
	}
}
