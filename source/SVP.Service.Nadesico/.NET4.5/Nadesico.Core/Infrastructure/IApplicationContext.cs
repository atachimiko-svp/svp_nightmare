﻿using Nadesico.Core.Constructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Core.Infrastructure
{
	public interface IApplicationContext
	{

		#region プロパティ

		/// <summary>
		/// アプリケーションが使用するファイルシステム上のディレクトリのパスを取得します
		/// </summary>
		string ApplicationDirectoryPath { get; }

		/// <summary>
		/// アプリケーションのバージョン情報を取得します
		/// </summary>
		System.Diagnostics.FileVersionInfo ApplicationFileVersionInfo { get; }

		/// <summary>
		/// アプリケーション設定ファイルのパス
		/// </summary>
		string ApplicationSettingFilePath { get; }

		/// <summary>
		/// コンフィグファイルを保存するディレクトリのパスを取得します
		/// </summary>
		string ConfigDirectoryPath { get; }

		/// <summary>
		/// データベースファイルを保存するディレクトリのパスを取得します
		/// </summary>
		string DatabaseDirectoryPath { get; }

		/// <summary>
		/// エンポラリファイルを格納するディレクトリパスを取得します
		/// </summary>
		string TemporaryDirectoryPath { get; }

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// 初期化処理を実行します
		/// </summary>
		/// <param name="param">実行する初期化処理の種類</param>
		void Initialize(InitializeParamType param);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		void Watch(string message);

		#endregion メソッド
	}
}
