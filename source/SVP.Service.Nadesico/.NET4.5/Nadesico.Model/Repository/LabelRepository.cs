using Akalib.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Model.Repository
{
	public class LabelRepository : GenericRepository<Label>
	{


		#region コンストラクタ

		public LabelRepository(DbContext context)
			: base(context)
		{
		}

		#endregion コンストラクタ

		#region メソッド

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IQueryable<Label> FindFloatLabel()
		{
			return _dbset.Where(x => x.ParentLabel == null);
		}

		/// <summary>
		/// Labelの読み込み
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Label Load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		#endregion メソッド
	}
}
