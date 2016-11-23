using Sakura.Core.Constractures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
		/// 
		/// </summary>
		Window MainWindow { get; }

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
		/// アプリケーションコンフィグ情報をファイルに保存します
		/// </summary>
		void SaveApplicationSettingFile();

		#endregion メソッド

	}
}
