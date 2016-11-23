using Sakura.Core.Constractures;
using Sakura.Core.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sakura.Core
{
	public static class ApplicationContext
	{

		#region フィールド

		static IApplicationContext Instance;

		#endregion フィールド

		#region プロパティ

		/// <summary>
		/// アプリケーションコンフィグ情報を取得します
		/// </summary>
		public static AppConfigSetting AppConfigInfo { get { return Instance.AppConfigInfo; } }


		/// <summary>
		/// 
		/// </summary>
		public static Window MainWindow { get { return Instance.MainWindow; } }

		/// <summary>
		/// 
		/// </summary>
		public static IWorkspaceViewModel Workspace { get { return Instance.Workspace; } }

		#endregion プロパティ


		#region メソッド

		public static void InitializeApplication()
		{
			Instance.InitializeApplication();
		}

		public static void SaveApplicationSettingFile()
		{
			Instance.SaveApplicationSettingFile();
		}

		public static void SetupApplicationContextImpl(IApplicationContext applicationContextImpl)
		{
			ApplicationContext.Instance = applicationContextImpl;
		}

		#endregion メソッド
	}
}
