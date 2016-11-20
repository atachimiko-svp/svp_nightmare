using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Service
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
	public class ApplicationInterfaceService : IApplicationInterfaceService
	{


		#region フィールド

		static IMapper Mapper;
		static MapperConfiguration MapperConfig;

		#endregion フィールド


		#region コンストラクタ

		public ApplicationInterfaceService()
		{
			// マッピングするクラスの紐付け設定
			MapperConfig = new MapperConfiguration(cfg =>
			{
				//cfg.CreateMap<Content, SVP.CIL.Domain.Conrent>();
			});

			Mapper = MapperConfig.CreateMapper();
		}

		#endregion コンストラクタ


		#region メソッド

		public void Login()
		{
			throw new NotImplementedException();
		}

		public void Logout()
		{
			throw new NotImplementedException();
		}

		#endregion メソッド

	}
}
