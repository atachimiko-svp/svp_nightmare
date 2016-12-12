using Akalib.Entity;
using log4net;
using Nadesico.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Gateway
{
	public sealed class ThumbnailDbContext : AtDbContext
	{

		#region フィールド

		public static SQLiteConnectionStringBuilder SDbConnection;

		private static ILog LOG = LogManager.GetLogger(typeof(ThumbnailDbContext));

		#endregion フィールド


		#region コンストラクタ

		public ThumbnailDbContext()
		: base(new SQLiteConnection(SDbConnection.ConnectionString), true)
		{
		}

		#endregion コンストラクタ


		#region プロパティ

		public DbSet<ServerPld> ServerPlds { get; set; }
		public DbSet<Thumbnail> Thumbnails { get; set; }

		#endregion プロパティ
	}
}
