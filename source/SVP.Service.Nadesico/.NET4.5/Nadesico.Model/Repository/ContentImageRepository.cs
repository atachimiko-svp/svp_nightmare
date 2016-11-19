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
	public sealed class ContentImageRepository : GenericRepository<ContentImage>
	{

		#region コンストラクタ

		public ContentImageRepository(DbContext context) : base(context)
		{

		}

		#endregion コンストラクタ


		#region メソッド

		/// <summary>
		/// ContentImageの読込
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ContentImage Load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		#endregion メソッド


	}
}
