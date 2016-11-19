using Nadesico.Core;
using Nadesico.Gateway;
using Nadesico.Model;
using Nadesico.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

		static void RunContentTB1()
		{
			System.Threading.Thread.CurrentThread.Name = "Nadesico API";
			application = new Application();

			var buildAssemblyType = new BuildAssemblyParameter();
			buildAssemblyType.Params["ApplicationDirectoryPath"] = @"\Nadesico_InvokeServiceConsole";

			ApplicationContext.SetupApplicationContextImpl(ApplicationContextImpl.CreateInstance(application, buildAssemblyType));
			((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).InitializeApplication();

			using(var dbc = new AppDbContext())
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

		#endregion メソッド

	}
}
