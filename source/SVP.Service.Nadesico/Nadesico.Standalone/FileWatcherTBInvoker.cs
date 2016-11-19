using log4net;
using Nadesico.Applus;
using Nadesico.Model;
using SVP.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Standalone
{
	/// <summary>
	/// ファイル更新監視をテストする実行クラス
	/// </summary>
	class FileWatcherTBInvoker
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(FileWatcherTBInvoker));

		VspFileUpdateWatchManager manager = null;

		#endregion フィールド


		#region メソッド

		public void OutputDump()
		{
			LOG.Info("ダンプ出力\n" + manager.DumpUpdateWatchedFile());
		}

		public void Run()
		{
			if (manager==null)
				manager = new VspFileUpdateWatchManager(GenerateTestWorkspace());
			manager.StartWatch();
		}

		/// <summary>
		/// テスト用のワークスペースオブジェクトを作成し、インスタンスを返す
		/// </summary>
		/// <returns></returns>
		private Workspace GenerateTestWorkspace()
		{
			Workspace obj = new Workspace();
			obj.Id = 1L;
			obj.Name = "テストワークスペース";
			obj.VirtualSpacePath = @"C:\DevSVP\VirtualSpace";
			obj.PhysicalSpacePath = @"C:\DevSVP\PhysicalSpace";
			return obj;
		}

		#endregion メソッド

	}
}
