using Akalib.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Model.Repository
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class ContentRepository : GenericRepository<Content>
	{


		#region コンストラクタ

		public ContentRepository(DbContext context) : base(context)
		{

		}

		#endregion コンストラクタ

		#region メソッド

		/// <summary>
		/// 
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public IQueryable<Content> FindByCategory(Category category)
		{
			return _dbset.Where(x => x.Category.Id == category.Id);
		}

		/// <summary>
		/// Contentの読込
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Content Load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		#endregion メソッド
	}
}
