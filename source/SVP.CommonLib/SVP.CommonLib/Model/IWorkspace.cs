using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SVP.CommonLib.Model
{
	public interface IWorkspace
	{

		#region プロパティ

		/// <summary>
		/// Getterのみ公開
		/// </summary>
		long Id { get; }

		/// <summary>
		/// Getterのみ公開
		/// </summary>
		string Name { get;}

		/// <summary>
		/// Getterのみ公開
		/// </summary>
		string PhysicalSpacePath { get;  }

		/// <summary>
		/// Getterのみ公開
		/// </summary>
		string VirtualSpacePath { get; }

		#endregion プロパティ

	}

	public static class WorkspaceExtension
	{

		#region メソッド

		/// <summary>
		/// 指定した文字列からワークスペースのパス部分を削除した文字列を返します。
		/// </summary>
		/// <param name="this"></param>
		/// <param name="path"></param>
		/// <param name="removeAclExtension">ACLファイル拡張子を除去する</param>
		/// <returns></returns>
		public static string TrimWorekspacePath(this IWorkspace @this, string path, bool removeAclExtension)
		{
			var escaped = Regex.Escape(@this.VirtualSpacePath);
			Regex re = new Regex("^" + escaped + @"[\\]*", RegexOptions.Singleline);
			string key = re.Replace(path, "");

			if (removeAclExtension)
			{
				Regex re2 = new Regex("\\.aclgene$", RegexOptions.Singleline);
				key = re2.Replace(key, "");
			}

			return key;
		}

		#endregion メソッド

	}
}
