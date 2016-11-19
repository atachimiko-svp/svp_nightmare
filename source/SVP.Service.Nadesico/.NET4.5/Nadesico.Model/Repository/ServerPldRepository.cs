using Akalib.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Model.Repository
{
	public sealed class ServerPldRepository : GenericRepository<ServerPld>
	{


		#region コンストラクタ

		public ServerPldRepository(DbContext context)
			: base(context)
		{
		}

		#endregion コンストラクタ


		#region メソッド

		public override IEnumerable<ServerPld> GetAll()
		{
			return _entities.Set<ServerPld>().AsEnumerable();
		}

		public ServerPld load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		#endregion メソッド

	}
}
