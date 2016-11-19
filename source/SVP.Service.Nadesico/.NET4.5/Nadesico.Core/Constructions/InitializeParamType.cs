using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Core.Constructions
{
	/// <summary>
	/// 初期化処理区分
	/// </summary>
	[Flags]
	public enum InitializeParamType
	{
		/// <summary>
		/// アプリケーションディレクトリの初期化
		/// </summary>
		DIRECTORY,
		/// <summary>
		/// データベースの初期化
		/// </summary>
		DATABASE
	}
}
