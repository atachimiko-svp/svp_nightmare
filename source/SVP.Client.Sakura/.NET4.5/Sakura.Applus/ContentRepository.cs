using Sakura.Core.Criteria;
using Sakura.Core.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Sakura.Applus.DataSource;
using System.Net;
using log4net;
using System.Net.Sockets;
using SVP.CIL.Domain;
using Sakura.Applus.Helper;
using SVP.CIL.Request;
using SVP.CIL.Response;

namespace Sakura.Applus
{
	public class ContentRepository : ISearchContent, IContentRepository
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(ContentRepository));

		LazyImageDataSource _DataSource = new LazyImageDataSource();

		IContentLazyItem _SelectedItem;

		#endregion フィールド


		#region コンストラクタ

		public ContentRepository()
		{
		}

		#endregion コンストラクタ


		#region イベント

		public event ChangeSelectedItemEventHandler ChangeSelectedItem;

		#endregion イベント


		#region プロパティ

		public IReadOnlyCollection<IContentLazyItem> Items
		{
			get
			{
				return new List<IContentLazyItem>(_DataSource.Items.Cast<IContentLazyItem>());
			}
		}

		public IContentLazyItem SelectedItem
		{
			get
			{
				return _SelectedItem;
			}
			private set
			{
				var old = _SelectedItem;
				_SelectedItem = value;

				RaiseChangeSelectedItem(old, _SelectedItem);
			}
		}

		#endregion プロパティ


		#region メソッド

		public async Task ExecFindByCategory(Category category)
		{
			var req = new RequestContentFindByCategory();
			req.Category = category;
			req.EnableThumbnailFlag = true;

			var proxy = ApiProxyHelper.CreateAsyncProxy();
			var result = await proxy.CallAsync<ResponseContentFindByCategory>(s => s.ContentFindByCategory(req));

			this._DataSource.Items.Clear();

			foreach (var content in result.Datas)
			{
				this._DataSource.AddItem(new ImageLazyItem(content.Id)
				{
					Title = content.Title
				});
			}
		}

		/// <summary>
		/// コンテントの付帯データを取得します。
		/// </summary>
		/// <remarks>
		///   ①サムネイル画像のバイト列を取得します。
		/// </remarks>
		/// <param name="content"></param>
		/// <returns></returns>
		public async Task<bool> LoadContentData(IContentLazyItem content)
		{
			string sessionId;

			// APIを使用し、バイト列を取得するためのセッションIDをサーバから取得する。
			var req = new RequestSendContentData { ContentId = content.ContentId };
			var proxy = ApiProxyHelper.CreateAsyncProxy();
			var result = await proxy.CallAsync<ResponseSendContentData>(s => s.SendContentData(req));
			sessionId = result.SessionId;

			var end = new IPEndPoint(IPAddress.Loopback, 17001);
			using (var client = new TcpClient())
			{
				client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);

				// サーバに接続
				try
				{
					await client.ConnectAsync(end.Address, end.Port).ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					LOG.Error(ex.Message);
					return false;
				}


				// サーバとの通信
				int sizePos = 0;

				// サーバから受信したデータ列を、一時ファイルへ書き込むためのIOを開く
				using (System.IO.MemoryStream temporaryFileStream = new System.IO.MemoryStream())
				using (var stream = client.GetStream())
				{
					byte[] data_session = Encoding.ASCII.GetBytes(sessionId);
					await stream.WriteAsync(data_session, 0, data_session.Length);
					int readSize = 0;
					do
					{
						var buf = new byte[10000];
						readSize = await stream.ReadAsync(buf, 0, buf.Length).ConfigureAwait(false);
						await temporaryFileStream.WriteAsync(buf, 0, readSize);
						sizePos += readSize;
					} while (readSize > 0);

					content.UpdatePreviewBitmap(temporaryFileStream.ToArray());
				}
			}

			return true;
		}

		public void NextContent()
		{

		}

		public void PrevContent()
		{

		}

		/// <summary>
		/// ChangeSelectedItemイベントを発火する
		/// </summary>
		/// <param name="old"></param>
		/// <param name="newItem"></param>
		void RaiseChangeSelectedItem(IContentLazyItem old, IContentLazyItem newItem)
		{
			if (ChangeSelectedItem != null)
			{
				var args = new ChangeSelectedItemEventArgs { OldItem = old, NewItem = newItem };
				ChangeSelectedItem(this, args);
			}
		}

		#endregion メソッド
	}
}
