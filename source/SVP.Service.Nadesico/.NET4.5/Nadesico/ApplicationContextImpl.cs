using Nadesico.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nadesico.Core.Constructions;
using System.Diagnostics;
using log4net;
using System.Windows;
using Akalib;
using System.IO;
using System.Data.SQLite;
using Akalib.Entity;
using System.Text.RegularExpressions;
using Nadesico.Gateway;
using Nadesico.Model;
using Nadesico.Model.Repository;

namespace Nadesico
{
	public class ApplicationContextImpl : IApplicationContext
	{

		#region フィールド

		private static ApplicationContextImpl instance;

		private static ILog LOG = LogManager.GetLogger(typeof(ApplicationContextImpl));

		/// <summary>
		/// アプリケーションが使用するディレクトリ
		/// </summary>
		private readonly string _ApplicationDirectoryPath;

		/// <summary>
		/// Disposeが実行済みの場合Trueを設定します
		/// </summary>
		private bool _alreadyDisposed = false;

		/// <summary>
		/// .NETアプリケーションのコンテキスト
		/// </summary>
		private Application _Application;

		/// <summary>
		/// アプリケーションの処理計測を行うためのタイマー
		/// </summary>
		private Stopwatch _Stopwatch = new Stopwatch();

		#endregion フィールド

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="application"></param>
		/// <param name="applicationConfig"></param>
		private ApplicationContextImpl(Application application, IBuildAssemblyParameter applicationConfig)
		{
			this._Stopwatch = new Stopwatch();
			this._Stopwatch.Start();

			_ApplicationDirectoryPath = "";
			_Application = application;

			IBuildAssemblyParameter buildParam;
			if (applicationConfig != null) buildParam = applicationConfig;
			else buildParam = new BuildAssemblyParameter();

			string personalDirectoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			_ApplicationDirectoryPath = personalDirectoryPath + buildParam.Params["ApplicationDirectoryPath"];

			AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.WindowsPrincipal);

			this.ApplicationFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

			CreateSettingSQLite();
		}

		#endregion コンストラクタ

		#region プロパティ

		/// <summary>
		/// アプリケーションが使用するディレクトrいを取得します。
		/// </summary>
		public string ApplicationDirectoryPath
		{
			get { return _ApplicationDirectoryPath; }
		}

		/// <summary>
		/// 
		/// </summary>
		public System.Diagnostics.FileVersionInfo ApplicationFileVersionInfo
		{
			get;
			private set;
		}

		/// <summary>
		/// アプリケーション設定ファイルのパスを取得します。
		/// </summary>
		public string ApplicationSettingFilePath
		{
			get { return ConfigDirectoryPath + @"\nadesico.conf"; }
		}

		/// <summary>
		/// アプリケーションの設定情報の保存に使用するディレクトリを取得します。
		/// </summary>
		public string ConfigDirectoryPath
		{
			get { return ApplicationDirectoryPath + @"\config"; }
		}

		/// <summary>
		/// アプリケーションが使用するデータベースファイルを保存するディレクトリを取得します。
		/// </summary>
		public string DatabaseDirectoryPath
		{
			get { return ApplicationDirectoryPath + @"\db"; }
		}

		/// <summary>
		/// アプリケーションが一時ファイルを保存するディレクトリを取得します。
		/// </summary>
		public string TemporaryDirectoryPath
		{
			get { return ApplicationDirectoryPath + @"\temporary"; }
		}

		#endregion プロパティ


		#region メソッド

		/// <summary>
		/// アプリケーションコンテキストクラスを新規作成します。
		/// このメソッドはアプリケーション内で１度だけ呼び出してください。
		/// </summary>
		/// <param name="application">WPFフレームワークのアプリケーションクラス</param>
		/// <returns></returns>
		public static IApplicationContext CreateInstance(Application application, IBuildAssemblyParameter applicationConfig = null)
		{
			if (ApplicationContextImpl.instance != null) throw new ApplicationException("アプリケーションコンテキストはすでにインスタンス化しています");
			ApplicationContextImpl.instance = new ApplicationContextImpl(application, applicationConfig);
			return ApplicationContextImpl.instance;
		}

		public static void Dispose()
		{
			ApplicationContextImpl.instance.Dispose(true);
			GC.SuppressFinalize(ApplicationContextImpl.instance);

			ApplicationContextImpl.instance = null;
		}

		public static IApplicationContext GetInstance()
		{
			return ApplicationContextImpl.instance;
		}

		/// <summary>
		/// アプリケーションの初期化を行います
		/// </summary>
		/// <param name="param">実行する初期化シーケンスの種類</param>
		public void Initialize(InitializeParamType param)
		{
			switch (param)
			{
				case InitializeParamType.DIRECTORY:
					InitializeDirectory();
					break;

				case InitializeParamType.DATABASE:
					InitializeDatabase();
					break;
			}
		}

		/// <summary>
		/// アプリケーション起動処理を実行します
		/// </summary>
		public void InitializeApplication()
		{
			Watch("InitializeApplicationの実行を開始します");

			RemoveTemporaryFiles();
			Initialize(InitializeParamType.DIRECTORY);
			Initialize(InitializeParamType.DATABASE);
		}


		/// <summary>
		/// アプリケーション全体での経過時間をロギングする
		/// </summary>
		/// <param name="message"></param>
		public void Watch(string message)
		{
			LOG.InfoFormat("[{0}] {1}", _Stopwatch.Elapsed, message);
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
		/// SQLiteを使用するための設定を読み込みます
		/// </summary>
		void CreateSettingSQLite()
		{
			SQLiteConnectionStringBuilder builder_AppDb = new SQLiteConnectionStringBuilder();
			builder_AppDb.DataSource = Path.Combine(DatabaseDirectoryPath, "nadesico.db");
			AppDbContext.SDbConnection = builder_AppDb;
		}

		/// <summary>
		/// データベースに関する初期化処理
		/// </summary>
		private void InitializeDatabase()
		{
			ServerPld apMetadata = null;
			bool isMigrate = false;

			const string appdb_structure_version_key = "APPDB_VER";

			// 構造の初期化
			using (var @dbc = new AppDbContext())
			{
				bool isInitializeDatabase = false;
				var @repo = new ServerPldRepository(@dbc);
				try
				{
					apMetadata = @repo.FindBy(p => p.Key == appdb_structure_version_key).FirstOrDefault();
					if (apMetadata == null) isInitializeDatabase = true;
				}
				catch (Exception)
				{
					isInitializeDatabase = true;
				}

				if (isInitializeDatabase)
				{
					// データベースにテーブルなどの構造を初期化する
					string sqltext = "";
					System.Reflection.Assembly assm = System.Reflection.Assembly.GetExecutingAssembly();

					using (var stream = assm.GetManifestResourceStream("Nadesico.Assets.Sql.Nadesico.Initialize_sql.txt"))
					{
						using (StreamReader reader = new StreamReader(stream))
						{
							sqltext = reader.ReadToEnd();
						}
					}
					@dbc.Database.ExecuteSqlCommand(sqltext);
					@dbc.SaveChanges();

					apMetadata = @repo.FindBy(p => p.Key == appdb_structure_version_key).FirstOrDefault();
				}

				if (apMetadata == null)
				{
					apMetadata = new ServerPld { Key = appdb_structure_version_key, Value = "1.0.0" };
					@repo.Add(apMetadata);

					@repo.Save();
				}

				string currentVersion = apMetadata.Value;
				string nextVersion = currentVersion;
				do
				{
					currentVersion = nextVersion;
					nextVersion = UpgradeFromResource("Nadesico", currentVersion, @dbc);
					if (nextVersion != currentVersion) isMigrate = true;
				} while (nextVersion != currentVersion);

				if (isMigrate)
				{
					apMetadata.Value = nextVersion;

					@repo.Save();
				}

				@dbc.SaveChanges();
			}
		}

		/// <summary>
		/// 必要なディレクトリを作成する初期化処理
		/// </summary>
		private void InitializeDirectory()
		{
			// アプリケーションが使用する各種ディレクトリの作成
			System.IO.Directory.CreateDirectory(ApplicationDirectoryPath);
			System.IO.Directory.CreateDirectory(DatabaseDirectoryPath);
			System.IO.Directory.CreateDirectory(TemporaryDirectoryPath);
			System.IO.Directory.CreateDirectory(ConfigDirectoryPath);
		}

		/// <summary>
		/// テンポラリディレクトリ内のファイルを削除する
		/// </summary>
		private void RemoveTemporaryFiles()
		{
			var dir = new DirectoryInfo(this.TemporaryDirectoryPath);
			if (!dir.Exists) return;

			foreach (var file in dir.GetFiles())
			{
				// ファイルの削除に失敗しても処理は継続
				try
				{
					file.IsReadOnly = false;
					file.Delete();
				}
				catch (Exception)
				{
				}
			}
		}


		/// <summary>
		///
		/// </summary>
		/// <param name="resourcePath"></param>
		/// <param name="dbc">データベース</param>
		private void UpgradeDatabase(string resourcePath, AtDbContext dbc)
		{
			string sqltext = "";
			System.Reflection.Assembly assm = System.Reflection.Assembly.GetExecutingAssembly();

			using (var stream = assm.GetManifestResourceStream(resourcePath))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					sqltext = reader.ReadToEnd();
				}
			}

			dbc.Database.ExecuteSqlCommand(sqltext);
		}


		/// <summary>
		/// 現在のバージョンからマイグレーションするファイルがリソースファイルにあるか探します。
		/// リソースファイルがある場合はそのファイルに含まれるSQLを実行し、ファイル名からマイグレーション後のバージョンを取得します。
		/// </summary>
		/// <param name="version">現在のバージョン。アップグレード元のバージョン。</param>
		/// <returns>次のバージョン番号。マイグレーションを実施しなかった場合は、versionの値がそのまま帰ります。</returns>
		private string UpgradeFromResource(string dbselect, string version, AtDbContext @dbc)
		{
			System.Reflection.Assembly assm = System.Reflection.Assembly.GetExecutingAssembly();

			string currentVersion = version;
			var mss = assm.GetManifestResourceNames();

			// この方法で読み込みができるリソースファイルの種類は「埋め込みリソース」を設定したもののみです。
			var r = new Regex(string.Format("Nadesico.Assets.Sql.{0}.{1}", dbselect, "upgrade - " + currentVersion + "-(.+)\\.txt"));
			foreach (var rf in assm.GetManifestResourceNames())
			{
				var matcher = r.Match(rf);
				if (matcher.Success && matcher.Groups.Count > 1)
				{
					UpgradeDatabase(rf, @dbc);
					currentVersion = matcher.Groups[1].Value; // 正規表現にマッチした箇所が、マイグレート後のバージョンになります。
				}
			}

			return currentVersion;
		}

		#endregion メソッド
	}
}
