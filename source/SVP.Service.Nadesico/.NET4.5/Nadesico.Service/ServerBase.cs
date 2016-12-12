using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Nadesico.Service
{
	public abstract class ServerBase
	{


		#region フィールド

		public readonly IPEndPoint Listen;
		public int AcceptCount;
		public int CloseByInvalidStream;
		public int CloseByPeerCount;
		public int CloseCount;
		public int ReadCount;
		public int WriteCount;
		protected const int backlog = 1000;
		protected const int bufferSize = 1000;
		protected const int headerSize = 4;
		protected const char terminate = '\n';

		#endregion フィールド


		#region コンストラクタ

		public ServerBase(IPEndPoint endpoint)
		{
			Listen = endpoint;
		}

		#endregion コンストラクタ


		#region メソッド

		abstract public void Run();

		public override string ToString()
		{
			return string.Format("accept({0}) close({1}) peer({2}) + invalid({3}) read({4}) write({5}) : {6}",
				AcceptCount,
				CloseCount,
				CloseByPeerCount,
				CloseByInvalidStream,
				ReadCount,
				WriteCount,
				GetType().Name
			);
		}

		protected void setSocketOption(Socket sock)
		{
			sock.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
		}

		#endregion メソッド

	}
}
