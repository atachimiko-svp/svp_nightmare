using AutoMapper;
using log4net;
using Nadesico.Model;
using SVP.CIL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SVP.CIL.Request;
using SVP.CIL.Response;
using Nadesico.Gateway;
using Nadesico.Model.Repository;
using System.Data.Entity.Validation;
using System.Collections;

namespace Nadesico.Service
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
	public class ApplicationInterfaceService : IApplicationInterfaceService
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(ApplicationInterfaceService));
		static IMapper Mapper;
		static MapperConfiguration MapperConfig;

		#endregion フィールド


		#region コンストラクタ

		public ApplicationInterfaceService()
		{
			// マッピングするクラスの紐付け設定
			MapperConfig = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Category, SVP.CIL.Domain.Category>().ReverseMap();
				cfg.CreateMap<Workspace, SVP.CIL.Domain.Workspace>().ReverseMap();
			});

			Mapper = MapperConfig.CreateMapper();
		}

		#endregion コンストラクタ


		#region メソッド

		public ResponseCategoryCrud CategoryCrud(RequestCategoryCrud reqparam)
		{
			var resp = new ResponseCategoryCrud();
			try
			{
				using (var dbc = new AppDbContext())
				{
					switch (reqparam.Crud)
					{
						case CrudType.CREATE:
							resp.Data = CategoryCreate(dbc, reqparam.Target);
							resp.Success = true;
							break;
						case CrudType.DELETE:
							resp.Success = CategoryDelete(dbc, reqparam.Target);
							break;
						case CrudType.READ:
							resp.Data = CategoryRead(dbc,reqparam.Target);
							resp.Success = true;
							break;
						case CrudType.UPDATE:
							resp.Data = CategoryUpdate(dbc, reqparam.Target);
							resp.Success = true;
							break;
					}
				}
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (DbEntityValidationResult entityErr in dbEx.EntityValidationErrors)
				{
					foreach (DbValidationError error in entityErr.ValidationErrors)
					{
						Console.WriteLine("Error Property Name {0} : Error Message: {1}",
											error.PropertyName, error.ErrorMessage);
						resp.Success = false;
					}
				}
			}


			return resp;
		}

		public ResponseCategoryLoadList CategoryLoadList(RequestCategoryLoadList reqparam)
		{
			var resp = new ResponseCategoryLoadList();

			try
			{
				using (var dbc = new AppDbContext())
				{
					var repo = new CategoryRepository(dbc);
					var category = repo.Load(reqparam.ParentTarget.Id);
					resp.Datas = new List<SVP.CIL.Domain.Category>();

					foreach (var c in category.ChildCategories)
					{
						var domainCategory = Mapper.Map<SVP.CIL.Domain.Category>(c);
						resp.Datas.Add(domainCategory);
					}

					resp.Success = true;
				}
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (DbEntityValidationResult entityErr in dbEx.EntityValidationErrors)
				{
					foreach (DbValidationError error in entityErr.ValidationErrors)
					{
						Console.WriteLine("Error Property Name {0} : Error Message: {1}",
											error.PropertyName, error.ErrorMessage);
						resp.Success = false;
					}
				}
			}

			return resp;
		}

		public void Login()
		{
			LOG.Debug("Execute API Login");
		}

		public void Logout()
		{
			LOG.Debug("Execute API Logout");
		}

		public ResponseWorkspaceCrud WorkspaceCrud(RequestWorkspaceCrud reqparam)
		{
			var resp = new ResponseWorkspaceCrud();
			try
			{
				using (var dbc = new AppDbContext())
				{
					switch (reqparam.Crud)
					{
						case CrudType.CREATE:
							resp.Data = WorkspaceCreate(dbc, reqparam.Target);
							resp.Success = true;
							break;
						case CrudType.DELETE:
							resp.Success = WorkspaceDelete(dbc, reqparam.Target);
							break;
						case CrudType.READ:
							resp.Data = WorkspaceRead(dbc, reqparam.Target);
							resp.Success = true;
							break;
						case CrudType.UPDATE:
							resp.Data = WorkspaceUpdate(dbc, reqparam.Target);
							resp.Success = true;
							break;
					}
				}
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (DbEntityValidationResult entityErr in dbEx.EntityValidationErrors)
				{
					foreach (DbValidationError error in entityErr.ValidationErrors)
					{
						Console.WriteLine("Error Property Name {0} : Error Message: {1}",
											error.PropertyName, error.ErrorMessage);
						resp.Success = false;
					}
				}
			}


			return resp;
		}

		public ResponseWorkspaceLoadList WorkspaceLoadList(RequestWorkspaceLoadList reqparam)
		{
			var resp = new ResponseWorkspaceLoadList();

			try
			{
				using (var dbc = new AppDbContext())
				{
					var repo = new WorkspaceRepository(dbc);

					resp.Datas = new List<SVP.CIL.Domain.Workspace>();

					foreach (var c in repo.GetAll())
					{
						var domainWorkspace = Mapper.Map<SVP.CIL.Domain.Workspace>(c);
						resp.Datas.Add(domainWorkspace);
					}

					resp.Success = true;
				}
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (DbEntityValidationResult entityErr in dbEx.EntityValidationErrors)
				{
					foreach (DbValidationError error in entityErr.ValidationErrors)
					{
						Console.WriteLine("Error Property Name {0} : Error Message: {1}",
											error.PropertyName, error.ErrorMessage);
						resp.Success = false;
					}
				}
			}

			return resp;
		}

		private SVP.CIL.Domain.Category CategoryCreate(AppDbContext dbc, SVP.CIL.Domain.Category target)
		{
			var category = Mapper.Map<Category>(target);
			var repo = new CategoryRepository(dbc);
			repo.Add(category);
			dbc.SaveChanges();
			var domainCategory = Mapper.Map<SVP.CIL.Domain.Category>(category);
			return domainCategory;
		}

		private bool CategoryDelete(AppDbContext dbc, SVP.CIL.Domain.Category target)
		{
			var repo = new CategoryRepository(dbc);
			var category = repo.Load(target.Id);
			repo.Delete(category);
			dbc.SaveChanges();
			return true;
		}

		private SVP.CIL.Domain.Category CategoryRead(AppDbContext dbc, SVP.CIL.Domain.Category target)
		{
			var repo = new CategoryRepository(dbc);
			var category = repo.Load(target.Id);
			var domainCategory = Mapper.Map<SVP.CIL.Domain.Category>(category);
			return domainCategory;
		}

		private SVP.CIL.Domain.Category CategoryUpdate(AppDbContext dbc, SVP.CIL.Domain.Category target)
		{
			var repo = new CategoryRepository(dbc);
			var category = repo.Load(target.Id);
			Mapper.Map<SVP.CIL.Domain.Category, Category>(target, category);
			repo.Save();
			dbc.SaveChanges();
			var domainCategory = Mapper.Map<SVP.CIL.Domain.Category>(category);
			return domainCategory;
		}

		private SVP.CIL.Domain.Workspace WorkspaceCreate(AppDbContext dbc, SVP.CIL.Domain.Workspace target)
		{
			var workspace = Mapper.Map<Workspace>(target);
			var repo = new WorkspaceRepository(dbc);
			repo.Add(workspace);
			dbc.SaveChanges();
			var domainWorkspace = Mapper.Map<SVP.CIL.Domain.Workspace>(workspace);
			return domainWorkspace;
		}

		private bool WorkspaceDelete(AppDbContext dbc, SVP.CIL.Domain.Workspace target)
		{
			var repo = new WorkspaceRepository(dbc);
			var workspace = repo.Load(target.Id);
			repo.Delete(workspace);
			dbc.SaveChanges();
			return true;
		}

		private SVP.CIL.Domain.Workspace WorkspaceRead(AppDbContext dbc, SVP.CIL.Domain.Workspace target)
		{
			var repo = new WorkspaceRepository(dbc);
			var workspace = repo.Load(target.Id);
			var domainWorkspace = Mapper.Map<SVP.CIL.Domain.Workspace>(workspace);
			return domainWorkspace;
		}

		private SVP.CIL.Domain.Workspace WorkspaceUpdate(AppDbContext dbc, SVP.CIL.Domain.Workspace target)
		{
			var repo = new WorkspaceRepository(dbc);
			var workspace = repo.Load(target.Id);
			Mapper.Map<SVP.CIL.Domain.Workspace, Workspace>(target, workspace);
			repo.Save();
			dbc.SaveChanges();
			var domainWorkspace = Mapper.Map<SVP.CIL.Domain.Workspace>(workspace);
			return domainWorkspace;
		}

		#endregion メソッド

	}
}
