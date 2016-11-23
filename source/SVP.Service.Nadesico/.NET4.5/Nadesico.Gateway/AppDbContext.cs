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
using System.Data.Entity.Infrastructure;

namespace Nadesico.Gateway
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class AppDbContext : AtDbContext
	{


		#region フィールド

		public static SQLiteConnectionStringBuilder SDbConnection;

		private static ILog LOG = LogManager.GetLogger(typeof(AppDbContext));

		#endregion フィールド


		#region コンストラクタ

		public AppDbContext()
		: base(new SQLiteConnection(SDbConnection.ConnectionString), true)
		{
		}

		#endregion コンストラクタ


		#region プロパティ

		public DbSet<Category> Categories { get; set; }
		public DbSet<Content> Contents { get; set; }
		public DbSet<FileMappingInfo> FileMappingInfos { get; set; }
		public DbSet<ServerPld> ServerPlds { get; set; }
		public DbSet<Workspace> Workspaces { get; set; }

		#endregion プロパティ


		#region メソッド

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Tag>()
				.HasMany(c => c.Contents)
				.WithMany(p => p.Tags)
				.Map(m =>
				{
					m.MapLeftKey("TagId");
					m.MapRightKey("ContentId");
					m.ToTable("svp_T_Tag2Content");
				});

			modelBuilder.Entity<Label>()
				.HasMany(c => c.Contents)
				.WithMany(p => p.Labels)
				.Map(m =>
				{
					m.MapLeftKey("LavelId");
					m.MapRightKey("ContentId");
					m.ToTable("svp_T_Label2Content");
				});
		}

		protected override bool ShouldValidateEntity(DbEntityEntry entityEntry)
		{
			if (entityEntry.State == EntityState.Deleted)
			{
				if (entityEntry.Entity is Category)
				{
					return true;
				}
			}

			return base.ShouldValidateEntity(entityEntry);
		}

		protected override System.Data.Entity.Validation.DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
		{
			// バリデーションは、エンティティモデル単位で行うこと

			if (entityEntry.Entity is Content)
			{
				var list = new List<System.Data.Entity.Validation.DbValidationError>();
				var entity = entityEntry.Entity as Content;

				// Title
				if (entityEntry.CurrentValues.GetValue<string>("Title") == "")
				{
					list.Add(new System.Data.Entity.Validation.DbValidationError("Title", "Title is required"));
				}

				// Starrating (0～5)
				int iStarrating = entityEntry.CurrentValues.GetValue<int>("Starrating");
				if (iStarrating < 0 || iStarrating > 5)
				{
					list.Add(new System.Data.Entity.Validation.DbValidationError("Starrating", "設定範囲外"));
				}

				return new System.Data.Entity.Validation.DbEntityValidationResult(entityEntry, list);
			}
			else if (entityEntry.Entity is Category)
			{
				var list = new List<System.Data.Entity.Validation.DbValidationError>();
				var entity = entityEntry.Entity as Category;

				if (entityEntry.State == EntityState.Deleted)
				{
					if (entity.Id == 1L)
					{
						list.Add(new System.Data.Entity.Validation.DbValidationError("Category.ID", "ID:1は削除できません"));
					}
				}

				return new System.Data.Entity.Validation.DbEntityValidationResult(entityEntry, list);
			}


				return base.ValidateEntity(entityEntry, items);
		}

		#endregion メソッド

	}
}
