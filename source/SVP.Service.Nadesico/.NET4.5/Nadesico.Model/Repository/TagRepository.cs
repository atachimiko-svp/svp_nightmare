using Akalib.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Model.Repository
{
	public class TagRepository : GenericRepository<Tag>
	{


		#region コンストラクタ

		public TagRepository(DbContext context)
			: base(context)
		{
		}

		#endregion コンストラクタ

		#region メソッド

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IQueryable<Tag> FindFloatTag()
		{
			return _dbset.Where(x => x.ParentTag == null);
		}

		/// <summary>
		/// Tagの読み込み
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Tag Load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		#endregion メソッド
	}
}
