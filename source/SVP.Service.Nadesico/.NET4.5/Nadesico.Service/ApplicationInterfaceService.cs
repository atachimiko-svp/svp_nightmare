using AutoMapper;
using log4net;
using SVP.CIL.Service;
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

		static ILog LOG = LogManager.GetLogger(typeof(ApplicationInterfaceService));
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
			LOG.Debug("Execute API Login");
		}

		public void Logout()
		{
			LOG.Debug("Execute API Logout");
		}

		#endregion メソッド

	}
}
