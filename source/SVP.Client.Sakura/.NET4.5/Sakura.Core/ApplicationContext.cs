using Sakura.Core.Constractures;
using Sakura.Core.Infrastructures;
using Sakura.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

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
		public static IContentRepository ContentRepository { get { return Instance.ContentRepository; } }

		/// <summary>
		/// 
		/// </summary>
		public static Dpi CurrentDpi { get { return Instance.CurrentDpi; } }

		/// <summary>
		/// 
		/// </summary>
		public static Window MainWindow { get { return Instance.MainWindow; } }

		/// <summary>
		/// 
		/// </summary>
		public static IUxManager Ux { get { return Instance.Ux; } }

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

		public static void MainWindowSourceInitialize(HwndSource hwnd) { Instance.MainWindowSourceInitialize(hwnd); }
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
