using Sakura.Core.Constractures;
using Sakura.Core.Infrastructures;
using Sakura.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sakura
{
	public class ApplicationContextImpl : IApplicationContext
	{


		#region フィールド

		static ApplicationContextImpl instance;

		readonly FileInfo _ApplicationConfigInfoFile;

		readonly DirectoryInfo _ApplicationDirectory;

		/// <summary>
		/// Disposeが実行済みの場合Trueを設定します
		/// </summary>
		bool _alreadyDisposed = false;


		/// <summary>
		/// WPFアプリケーションのコンテキスト
		/// </summary>
		Application _Application;

		/// <summary>
		/// 
		/// </summary>
		Window _MainWindow;

		/// <summary>
		/// 
		/// </summary>
		WorkspaceViewModel _Workspace;

		#endregion フィールド


		#region コンストラクタ

		private ApplicationContextImpl(Application application)
		{
			this.AppConfigInfo = new AppConfigSetting();
			this.AppConfigInfo.Reset();

			_Application = application;

			// アプリケーションが使用するディレクトリ
			string personalDirectoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#if DEBUG
			_ApplicationDirectory = new DirectoryInfo(Path.Combine(personalDirectoryPath, @"Sakura_dev"));
#else
			_ApplicationDirectory = new DirectoryInfo(Path.Combine(personalDirectoryPath,@"Sakura"));
#endif
			_ApplicationConfigInfoFile = new FileInfo(Path.Combine(ConfigDirectory.FullName, @"sakura.conf"));

			// アプリケーションが動作するドメイン
			AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
		}

		#endregion コンストラクタ


		#region Destructors

		~ApplicationContextImpl()
		{
			// ファイナライザでは、アンマネージドリソースのみを破棄するようにDispose()を呼び出す。
			Dispose(false);
		}

		#endregion Destructors


		#region プロパティ

		public AppConfigSetting AppConfigInfo
		{
			get;
			private set;
		}

		public DirectoryInfo ApplicationDirectory
		{
			get
			{
				return _ApplicationDirectory;
			}
		}

		public DirectoryInfo ConfigDirectory
		{
			get
			{
				return new DirectoryInfo(Path.Combine(ApplicationDirectory.FullName, @"config"));
			}
		}
		public Window MainWindow
		{
			get
			{
				return _MainWindow;
			}
			set { _MainWindow = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public IWorkspaceViewModel Workspace
		{
			get { return _Workspace; }
			set { _Workspace = (WorkspaceViewModel)value; }
		}

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// アプリケーションコンテキストクラスを新規作成します。
		/// このメソッドはアプリケーション内で１度だけ呼び出してください。
		/// </summary>
		/// <param name="application">WPFフレームワークのアプリケーションクラス</param>
		/// <returns></returns>
		public static IApplicationContext CreateInstance(Application application)
		{

			if (ApplicationContextImpl.instance != null) throw new ApplicationException("アプリケーションコンテキストはすでにインスタンス化しています");
			ApplicationContextImpl.instance = new ApplicationContextImpl(application);

			return ApplicationContextImpl.instance;
		}

		public static void Dispose()
		{
			ApplicationContextImpl.instance.Dispose(true);
			GC.SuppressFinalize(ApplicationContextImpl.instance);
		}

		public static IApplicationContext GetInstance()
		{
			return ApplicationContextImpl.instance;
		}

		public void ApplicationExit()
		{
			_Application.Shutdown(1);
		}

		public void InitializeApplication()
		{
			// アプリケーションが使用する各種ディレクトリの作成
			System.IO.Directory.CreateDirectory(ApplicationDirectory.FullName);
			System.IO.Directory.CreateDirectory(ConfigDirectory.FullName);

			LoadApplicationSettingFile();
		}

		/// <summary>
		/// アプリケーション設定を保存します
		/// </summary>
		public void SaveApplicationSettingFile()
		{
			if (this.AppConfigInfo == null) return;

			if (!File.Exists(_ApplicationConfigInfoFile.FullName))
			{
				File.Create(_ApplicationConfigInfoFile.FullName).Close();
			}

			using (var sw = new StreamWriter(_ApplicationConfigInfoFile.FullName))
			{
				this.AppConfigInfo.Save(sw);
			}
		}

		/// <summary>
		/// アプリケーション終了時に行う処理を記述します
		/// </summary>
		public void ShutdownProcess()
		{
			SaveApplicationSettingFile();
		}

		protected virtual void Dispose(bool isDisposing)
		{

			if (_alreadyDisposed)
				return;

			if (isDisposing)
			{
			}

			_alreadyDisposed = true;
		}
		/// <summary>
		/// アプリケーション設定を読込ます。
		/// </summary>
		void LoadApplicationSettingFile()
		{
			if (File.Exists(_ApplicationConfigInfoFile.FullName))
			{

				using (StreamReader sr = new StreamReader(_ApplicationConfigInfoFile.FullName, Encoding.GetEncoding("utf-8")))
				{
					AppConfigInfo.Load(sr);
				}
			}
			else
			{
				// ファイルが存在しない場合、デフォルト設定で設定情報を作成し、ファイルに出力する。
				AppConfigInfo.Reset();

				File.Create(_ApplicationConfigInfoFile.FullName).Close();
				using (var sw = new StreamWriter(_ApplicationConfigInfoFile.FullName))
				{
					AppConfigInfo.Save(sw);
				}
			}
		}

		#endregion メソッド

	}
}
