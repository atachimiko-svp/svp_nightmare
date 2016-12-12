using Nadesico.Core;
using Nadesico.Gateway;
using Nadesico.Model.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nadesico.Service
{
	public class ContentTransportAwaitServer : ServerBase
	{

		

		#region コンストラクタ

		public ContentTransportAwaitServer(IPEndPoint endpoint) : base(endpoint)
		{

		}

		#endregion コンストラクタ


		#region メソッド

		public override void Run()
		{
			run(Listen).Wait();
		}

		/// <summary>
		/// 画像ファイルのバイナリを転送する
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		async Task handleNetworkStream(NetworkStream stream, string sessionKey)
		{
			if (!ApplicationContext.DataTransportSessionKeyMap.ContainsKey(sessionKey)) return;

			var contentId = ApplicationContext.DataTransportSessionKeyMap[sessionKey];

			using (var dbc = new AppDbContext())
			{
				var repo = new ContentRepository(dbc);
				var content = repo.Load(contentId);
				var filePath = Path.Combine(content.FileMappingInfo.Workspace.PhysicalSpacePath, content.FileMappingInfo.MappingFilePath);

				var thumbnail = LoadImageBytes(filePath);

				// ネットワークソケットから、データ列を読み込む
				var buffer = new byte[bufferSize];
				await stream.WriteAsync(thumbnail, 0, thumbnail.Length).ConfigureAwait(false);
			}
				
		}

		async void handleTcpClient(TcpClient client)
		{
			setSocketOption(client.Client);
			try
			{
				using (var stream = client.GetStream())
				{
					var buffer = new byte[bufferSize];

					var length = await stream.ReadAsync(buffer, 0, bufferSize).ConfigureAwait(false);
					string sessionKey = System.Text.Encoding.ASCII.GetString(buffer).TrimEnd('\0');

					await handleNetworkStream(stream, sessionKey).ConfigureAwait(false);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				Interlocked.Increment(ref CloseCount);
				client.Close();
			}
		}

		/// <summary>
		/// 任意のファイルのデータをバイナリ列で取得する
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		byte[] LoadImageBytes(string filePath)
		{
			// TODO: チェックはしていないが、画像ファイル限定とする
			using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			using (BinaryReader br = new BinaryReader(fs))
			{
				byte[] imageBytes = br.ReadBytes((int)fs.Length);
				return imageBytes;
			}
		}

		async Task run(IPEndPoint end)
		{
			var listener = new TcpListener(end);
			listener.Start(backlog);
			while (true)
			{
				var client = await listener.AcceptTcpClientAsync().ConfigureAwait(false);
				Interlocked.Increment(ref AcceptCount);
				handleTcpClient(client);
			}
		}

		#endregion メソッド

	}
}
