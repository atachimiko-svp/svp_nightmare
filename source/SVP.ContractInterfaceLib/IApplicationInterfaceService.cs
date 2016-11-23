using SVP.CIL.Request;
using SVP.CIL.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SVP.CIL.Service
{
	/// <summary>
	/// アプリケーション通信サービス
	/// </summary>
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface IApplicationInterfaceService
	{


		#region メソッド

		[OperationContract]
		ResponseCategoryCrud CategoryCrud(RequestCategoryCrud reqparam);

		
		[OperationContract]
		ResponseCategoryLoadList CategoryLoadList(RequestCategoryLoadList reqparam);


		[OperationContract]
		ResponseContentFindByCategory ContentFindByCategory(RequestContentFindByCategory reqparam);

		[OperationContract(IsInitiating = true, IsTerminating = false)]
		void Login();

		[OperationContract(IsInitiating = false, IsTerminating = true)]
		void Logout();

		[OperationContract]
		ResponseWorkspaceCrud WorkspaceCrud(RequestWorkspaceCrud reqparam);

		[OperationContract]
		ResponseWorkspaceLoadList WorkspaceLoadList(RequestWorkspaceLoadList reqparam);

		#endregion メソッド

	}
}
