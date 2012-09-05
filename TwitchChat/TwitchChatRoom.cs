using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TwitchChat {
	public class TwitchChatRoom {
		public event EventHandler Connected;
		public event EventHandler Disconnected;
		public event EventHandler UserJoined;
		public event EventHandler UserLeft;
		public event EventHandler<IRCMessageEventArgs> Message;

		public string Password { get; set; }
		public string Username { get; set; }
		public string Channel { get; set; }

		TcpClientDataSource _client;

		public void OnConnected(EventArgs e) {
			if (Connected != null) Connected(this, e);
		}

		public void OnDisconnected(EventArgs e) {
			if (Disconnected != null) Disconnected(this, e);
		}

		public void OnUserJoined(EventArgs e) {
			if (UserJoined != null) UserJoined(this, e);
		}

		public void OnUserLeft(EventArgs e) {
			if (UserLeft != null) UserLeft(this, e);
		}

		public TwitchChatRoom(string streamChannel, string username, string password) {
			_client = new TcpClientDataSource(streamChannel + ".jtvirc.com", 6667);
			Username = username;
			Password = password;
			Channel = streamChannel;

			_client.DataReceived += ClientOnDataReceived;
			_client.Connected += ClientConnected;
			_client.Disconnected += ClientDisconnected;

			_client.Start();
		}

		StringBuilder _buff = new StringBuilder();
		private void ClientOnDataReceived(DataSource dataSource, DataReceivedEventArgs dataReceivedEventArgs) {
			foreach (var b in dataReceivedEventArgs.NewData) {
				if (b == '\r' || b == '\n') {
					if (_buff.Length > 0)
						Dispatch(_buff.ToString());
					_buff.Clear();
				}
				else
					_buff.Append((char)b);
			}
		}

		void ClientConnected(object sender, EventArgs args) {
			Write("PASS " + Password);
			Write("NICK " + Username);
			Write("JOIN #" + Channel);
		}

		void ClientDisconnected(object sender, EventArgs args) {
			OnDisconnected(args);
		}

		private void Write(string line) {
			Logger.Info(">> {0}", line);
			_client.WriteLine(line);
		}

		private void Dispatch(string line) {
			Logger.Info("<< {0}", line);
			string[] tokens = line.Split(' ');
			if (tokens[1] == "PRIVMSG") {
				DateTime when = DateTime.Now;
				string user = tokens[0].Substring(1, tokens[0].IndexOf('!') - 1);
				string msg = line.Substring(line.IndexOf(':', 1) + 1);
				OnMessage(when, user, msg);
			}
			else if (tokens[1] == "JOIN") {
				OnConnected(EventArgs.Empty);
			}
		}

		private void OnMessage(DateTime when, string user, string msg) {
			if (Message != null)
				Message(this, new IRCMessageEventArgs(when, user, msg));
		}


		public void Dispose() {
			_client.Dispose();
		}
	}
}