using Sakura.Core.Constractures;
using Sakura.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Sakura.Core.Infrastructures
{
	public interface IApplicationContext
	{

		#region プロパティ

		/// <summary>
		/// アプリケーションコンフィグ情報を取得します
		/// </summary>
		AppConfigSetting AppConfigInfo { get; }

		/// <summary>
		/// アプリケーションが使用するディレクトリ情報を取得します。
		/// </summary>
		DirectoryInfo ApplicationDirectory { get; }

		/// <summary>
		/// アプリケーションのコンフィグ情報を格納するディレクトリ情報を取得します。
		/// </summary>
		DirectoryInfo ConfigDirectory { get; }

		/// <summary>
		/// アプリケーションのメインウィンドウがあるデバイスのDPI
		/// </summary>
		Dpi CurrentDpi { get; }

		/// <summary>
		/// 
		/// </summary>
		Window MainWindow { get; }

		/// <summary>
		/// 
		/// </summary>
		IUxManager Ux { get; }

		/// <summary>
		/// 
		/// </summary>
		IContentRepository ContentRepository { get; }

		/// <summary>
		/// 
		/// </summary>
		IWorkspaceViewModel Workspace { get; }

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// アプリケーションを終了します
		/// </summary>
		void ApplicationExit();

		/// <summary>
		/// アプリケーションの初期化
		/// </summary>
		void InitializeApplication();

		/// <summary>
		/// 
		/// </summary>
		void MainWindowSourceInitialize(HwndSource hwnd);

		/// <summary>
		/// アプリケーションコンフィグ情報をファイルに保存します
		/// </summary>
		void SaveApplicationSettingFile();

		#endregion メソッド
	}
}
