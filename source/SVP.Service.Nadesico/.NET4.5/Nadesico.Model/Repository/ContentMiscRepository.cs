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
	public sealed class ContentMiscRepository : GenericRepository<ContentMisc>
	{

		#region コンストラクタ

		public ContentMiscRepository(DbContext context) : base(context)
		{

		}

		#endregion コンストラクタ

		#region メソッド

		/// <summary>
		/// Contentの読込
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ContentMisc Load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		#endregion メソッド

	}
}
