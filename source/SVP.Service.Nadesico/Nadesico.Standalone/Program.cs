using AutoMapper;
using Nadesico.Core;
using Nadesico.Gateway;
using Nadesico.Model;
using Nadesico.Model.Repository;
using Nadesico.Service;
using SVP.CIL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WcfClientProxyGenerator;

namespace Nadesico.Standalone
{
	class Program
	{


		#region フィールド

		private static Application application;

		#endregion フィールド


		#region メソッド

		static void Main(string[] args)
		{
			Console.WriteLine("アプリケーションを開始します");
			Console.WriteLine("実行モードを入力してください：");
			Console.WriteLine("   vfs    : 仮想空間ファイル監視機能モード");
			Console.WriteLine("   cat1   : カテゴリ機能モード1");
			Console.WriteLine("   ctm1   : コンテンツ機能モード1");
			Console.WriteLine("   srv1   : サーバ起動1");
			Console.WriteLine("   cli1   : クライアント起動1");
			Console.WriteLine("   e      : 終了");

			Console.Write("> ");
			string mode = Console.ReadLine();

			switch (mode)
			{
				case "vfs":
					RunFileWatcherTB();
					break;
				case "cat1":
					RunCategoryTB1();
					break;
				case "ctm1":
					RunContentTB1();
					break;
				case "srv1":
					RunServiceTB1();
					break;
				case "cli1":
					RunClientTB1();
					break;
				case "e":
					break;
				default:
					break;
			}

			Console.WriteLine("アプリケーションを終了します");
			Console.ReadLine();
		}

		static void RunCategoryTB1()
		{
			System.Threading.Thread.CurrentThread.Name = "Nadesico API";
			application = new Application();

			var buildAssemblyType = new BuildAssemblyParameter();
			buildAssemblyType.Params["ApplicationDirectoryPath"] = @"\Nadesico_InvokeServiceConsole";

			ApplicationContext.SetupApplicationContextImpl(ApplicationContextImpl.CreateInstance(application, buildAssemblyType));
			((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).InitializeApplication();

			long id_child;
			using (var dbc = new AppDbContext())
			{
				var repoCategory = new CategoryRepository(dbc);
				var catRoot = repoCategory.Load(1L);

				var catNew = new Category
				{
					Name = "新しいカテゴリ",
					Memo = "新しいメモ",
					Comment = "コメント",
					CreateDate = DateTime.Now,
					LastUpDate = DateTime.Now,
					SortType = CategorySortType.MANUAL,
				};
				repoCategory.Add(catNew);
				dbc.SaveChanges();
				id_child = catNew.Id;

				catRoot.ChildCategories.Add(catNew);
				dbc.SaveChanges();

				Console.WriteLine("ROOTカテゴリの子階層({0})に、新しいカテゴリを追加しました。", id_child);
			}

			// 保存した子階層カテゴリを参照
			using (var dbc = new AppDbContext())
			{
				var repoCategory = new CategoryRepository(dbc);
				var catRoot = repoCategory.Load(1L);

				Console.WriteLine("ROOTカテゴリの子階層カテゴリのID({0})を確認", catRoot.ChildCategories[0].Id);
			}

			// 子階層カテゴリから、親階層カテゴリ(ID=1)の参照
			using (var dbc = new AppDbContext())
			{
				var repoCategory = new CategoryRepository(dbc);
				var catChild = repoCategory.Load(id_child);

				Console.WriteLine("子階層カテゴリの親カテゴリ({0})を確認", catChild.ParentCategory.Id);
			}
		}

		private static void RunClientTB1()
		{
			bool bContinue = true;
			string command = "";

			IApplicationInterfaceService proxy = WcfClientProxy.Create<IApplicationInterfaceService>(c =>
				{
					c.SetEndpoint("netNamedPipeBinding_SvpApi");
				});

			while (bContinue)
			{
				Console.WriteLine("コマンドを入力してください");
				Console.Write("> ");
				command = Console.ReadLine();

				switch (command)
				{
					case "login":
						proxy.Login();
						Console.WriteLine("Loginコマンドを実行しました");
						break;
					case "cc1":
						var newCategory = new SVP.CIL.Domain.Category
						{
							Name = "新規カテゴリ",
							Comment = "コメントです"
						};
						var rsp_cc1 = proxy.CategoryCrud(new SVP.CIL.Request.RequestCategoryCrud
						{
							Crud = SVP.CIL.Request.CrudType.CREATE,
							Target = newCategory
						});
						Console.WriteLine("カテゴリ({0})を作成しました。", rsp_cc1.Data.Id);
						break;
					case "cd1":
						Console.Write("削除ID:");
						string deleteId = Console.ReadLine();
						var rsp_cd1 = proxy.CategoryCrud(new SVP.CIL.Request.RequestCategoryCrud
						{
							Crud = SVP.CIL.Request.CrudType.DELETE,
							Target = new SVP.CIL.Domain.Category { Id = long.Parse(deleteId) }
						});
						if(rsp_cd1.Success)
							Console.WriteLine("削除しました");
						else
							Console.WriteLine("削除できませんでした");
						break;
					case "cu1":
						var rsp_cu1_1 = proxy.CategoryCrud(new SVP.CIL.Request.RequestCategoryCrud
						{
							Crud = SVP.CIL.Request.CrudType.READ,
							Target = new SVP.CIL.Domain.Category { Id = 2L }
						});

						var updatedDomainCategory = rsp_cu1_1.Data;
						Console.Write("新しいタイトル名:");
						string newTitle = Console.ReadLine();
						updatedDomainCategory.Name = newTitle;
						var rsp_cu1 = proxy.CategoryCrud(new SVP.CIL.Request.RequestCategoryCrud
						{
							Crud = SVP.CIL.Request.CrudType.UPDATE,
							Target = updatedDomainCategory
						});
						break;
					case "clist1":
						var rsp_clist1 = proxy.CategoryLoadList(new SVP.CIL.Request.RequestCategoryLoadList
						{
							ParentTarget = new SVP.CIL.Domain.Category { Id = 10L }
						});

						if (rsp_clist1.Success)
						{
							foreach (var item in rsp_clist1.Datas)
							{
								Console.WriteLine("ID:{0} Name:{1}", item.Id, item.Name);
							}
						}
						else
						{
							Console.WriteLine("API実行に失敗しました");
						}
						break;
					case "e":
						bContinue = false;
						break;
				}
			}
		}

		static void RunContentTB1()
		{
			System.Threading.Thread.CurrentThread.Name = "Nadesico API";
			application = new Application();

			var buildAssemblyType = new BuildAssemblyParameter();
			buildAssemblyType.Params["ApplicationDirectoryPath"] = @"\Nadesico_InvokeServiceConsole";

			ApplicationContext.SetupApplicationContextImpl(ApplicationContextImpl.CreateInstance(application, buildAssemblyType));
			((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).InitializeApplication();

			using (var dbc = new AppDbContext())
			{
				var content = new ContentImage();
				content.IdentifyKey = "aaaa";
				content.Title = "コンテントテストA";
				content.ArchiveFlag = true;
				content.ReadableFlag = true;
				content.Starrating = 3;
				content.UnsetStarratingFlag = false;

				var repoContent = new ContentImageRepository(dbc);
				repoContent.Add(content);

				dbc.SaveChanges();
			}

			Console.WriteLine("永続化が完了しました。");
		}

		static void RunFileWatcherTB()
		{
			// Serviceサーバーを起動して、クライアントにWSDLなどを公開するためのコンソールプログラムです。
			System.Threading.Thread.CurrentThread.Name = "Nadesico API";
			application = new Application();

			var buildAssemblyType = new BuildAssemblyParameter();
			buildAssemblyType.Params["ApplicationDirectoryPath"] = @"\Nadesico_InvokeServiceConsole";

			ApplicationContext.SetupApplicationContextImpl(ApplicationContextImpl.CreateInstance(application, buildAssemblyType));
			((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).InitializeApplication();

			var invoker = new FileWatcherTBInvoker();
			invoker.Run();

			bool isEnd = false;

			do
			{
				var command = Console.ReadLine();
				switch (command)
				{
					case "d":
						invoker.OutputDump();
						break;
					default:
						isEnd = true;
						break;
				}

			} while (!isEnd);

		}

		static void RunServiceTB1()
		{
			// Serviceサーバーを起動して、クライアントにWSDLなどを公開するためのコンソールプログラムです。
			System.Threading.Thread.CurrentThread.Name = "Nadesico API";
			application = new Application();

			var buildAssemblyType = new BuildAssemblyParameter();
			buildAssemblyType.Params["ApplicationDirectoryPath"] = @"\Nadesico_InvokeServiceConsole";

			ApplicationContext.SetupApplicationContextImpl(ApplicationContextImpl.CreateInstance(application, buildAssemblyType));
			((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).InitializeApplication();

			using (ServiceHost host = new ServiceHost(typeof(ApplicationInterfaceService)))
			{
				// ① エンドポイントの手動構成設定 (C/B/A を指定)
				//   バインディングの構成設定を行いたい場合には、Binding インスタンスのプロパティを設定する
				var binder = new NetNamedPipeBinding();
				binder.MaxBufferSize = 1073741824;
				binder.MaxBufferPoolSize = 1073741824;
				binder.MaxReceivedMessageSize = 1073741824;
				binder.ReaderQuotas.MaxArrayLength = 1073741824;

				host.AddServiceEndpoint(typeof(IApplicationInterfaceService), binder,
					"net.pipe://localhost/Nadesico.Server/Application");
				// ② ビヘイビアの手動構成設定
				//   すでにいくつかのビヘイビアは既定で追加されているため、取り払ってから再設定する
				host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
				host.Description.Behaviors.Remove(typeof(ServiceMetadataBehavior));
				host.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });
				host.Description.Behaviors.Add(new ServiceMetadataBehavior
				{
					HttpGetEnabled = true,
					HttpGetUrl = new Uri("http://localhost:8001/Nadesico.Server/Application/mex")
				});

				// ホストのオープン
				host.Open();
				Console.WriteLine("WCF サービスを起動しました。");
				Console.ReadLine();
				host.Close();
			}
		}

		#endregion メソッド

	}
}
