using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakura.Contrib.Infrastructures
{
	/// <summary>
	/// データソースを持つコレクションクラスのインターフェース
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal interface IDataSource<T>
		where T : ICollection
	{

		#region プロパティ

		T Items { get; }

		#endregion プロパティ
	}
}
