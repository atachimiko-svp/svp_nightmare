using Akalib.Entity;
using Nadesico.Core.Structures;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Activity.Logic
{
	public static class VfsLogicUtils
	{

		#region メソッド

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public static string GenerateACLHash()
		{
			return Guid.NewGuid().ToString("N");
		}

		public static AclFileStructure ReadACLFile(FileInfo aclFillePath)
		{
			using (var file = File.OpenRead(aclFillePath.FullName))
			{
				return Serializer.Deserialize<AclFileStructure>(file);
			}
		}

		/// <summary>
		/// エンティティを更新または登録を行います
		/// </summary>
		/// <param name="context">データベースコンテキスト</param>
		/// <param name="entity">対象のエンティティ</param>
		public static void UpdateOrCreate(DbContext context, IEntity<long> entity)
		{
			var dbset = context.Set(entity.GetType());
			if (entity.Id == 0L) dbset.Add(entity);
			else context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
		}

		#endregion メソッド
	}
}
