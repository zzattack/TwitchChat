using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TwitchChat {
	public class TcpClientDataSource : DataSource {
		TcpClient _tcp;
		NetworkStream _stream;
		byte[] _readBuf = new byte[1024];
		string _host;
		int _port;
		public event EventHandler Connected;
		public event EventHandler Disconnected;

		public TcpClientDataSource(string host, int port = 6498) {
			_host = host;
			_port = port;
		}

		public override string Source {
			get { return string.Format("tcp://{0}:{1}", _host, _port); }
		}

		public override void Start() {
			try {
				if (_stream != null) {
					_stream.Dispose();
					_stream = null;
				}
				if (_tcp != null) {
					if (_tcp.Connected)
						_tcp.Close();
					_tcp = null;
				}

				if (_tcp == null) {
					_tcp = new TcpClient();
					_tcp.BeginConnect(_host, _port, OnConnect, null);
				}
			}
			catch {
				Logger.Error("TCP connection failed");
				ApplyReconnectBehavior();
			}
		}

		public override void Stop() {
			if (_stream != null) {
				_stream.Close();
				_stream.Dispose();
			}
			if (_tcp != null)
				_tcp.Close();
			_tcp = null;
			_stream = null;

			Logger.Info("Stopping TCP DataProvider");

			base.Stop();
		}

		public override void Dispose() {
			Stop();
			base.Dispose();
		}

		public void OnConnect(IAsyncResult iar) {
			try {
				if (_tcp != null) {
					_tcp.EndConnect(iar);
					_stream = _tcp.GetStream();
					_stream.BeginRead(_readBuf, 0, _readBuf.Length, TcpOnDataReceived, null);
					Logger.Success("Connected TCP socket");
					if (Connected != null)
						Connected(this, EventArgs.Empty);
				}
			}
			catch {
				Logger.Error("Couldn't connect TCP socket");
				ApplyReconnectBehavior();
			}
		}

		void TcpOnDataReceived(IAsyncResult r) {
			if (_stream == null)
				return;

			int bytesRead = 0;
			try {
				bytesRead = _stream.EndRead(r);
			}
			catch (IOException) {
				Logger.Error("IOException in TcpOnDataReceived");
			}
			catch (InvalidOperationException) {
				Logger.Error("InvalidOperationException in TcpOnDataReceived");
			}
			catch (NullReferenceException) {
				Logger.Error("NullReferenceException in TcpOnDataReceived");
			}
			if (bytesRead == 0) {
				Logger.Error("No bytes read! Connection aborted?");
				if (_stream != null)
					_stream.Close();
				if (_tcp != null)
					_tcp.Close();

				if (Disconnected != null) 
					Disconnected(this, EventArgs.Empty);
				ApplyReconnectBehavior();
			}
			else {
				byte[] b = new byte[bytesRead];
				Array.Copy(_readBuf, b, bytesRead);
				OnDataReceived(b);
				_stream.BeginRead(_readBuf, 0, _readBuf.Length, TcpOnDataReceived, null);
			}
		}


		internal void WriteLine(string line) {
			var buff = Encoding.Default.GetBytes(line + "\r\n");
			_stream.BeginWrite(buff, 0, buff.Length, WrittenCallback, _stream);
		}

		void WrittenCallback(IAsyncResult iar) {
			var stream = (NetworkStream)iar.AsyncState;
			stream.EndRead(iar);
		}

	}
}
