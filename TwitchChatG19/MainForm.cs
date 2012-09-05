using System;
using System.Media;
using System.Windows.Forms;
using LgLcd;
using TwitchChat;
using TwitchChatG19.Properties;

namespace TwitchChatG19 {
	public sealed partial class MainForm : WinFormsApplet {
		Hooks _hooks;
		Theme _currentTheme;
		FormThemer _themer = new FormThemer();
		TwitchChatRoom _chatRoom;

		public MainForm() {
			InitializeComponent();
			Device.SetAsLCDForegroundApp(true);
			if (!IsHandleCreated) CreateHandle();
			_hooks = new Hooks(Device);
			Device.Menu += Device_Menu;
			Device.Ok += DeviceOnOk;

			_currentTheme = _themer.GetTheme(Settings.Default.ThemeIdx);
			ApplyTheme();

			EnterChatroom();
		}

		private void DeviceOnOk(object sender, EventArgs args) {
			if (InvokeRequired) {
				Invoke(new EventHandler<EventArgs>(DeviceOnOk), sender, args);
			}
			else {
				Settings.Default.ThemeIdx = (Settings.Default.ThemeIdx + 1)%_themer.NumThemes;
				_currentTheme = _themer.GetTheme(Settings.Default.ThemeIdx);
				ApplyTheme();
			}
		}

		private void EnterChatroom() {
			_chatRoom = new TwitchChatRoom(Settings.Default.Channel, Settings.Default.Username, Settings.Default.Password);
			_chatRoom.Connected += ChatRoomOnConnected;
			_chatRoom.Disconnected += ChatRoomOnDisconnected;
			_chatRoom.Message += ChatRoomOnMessage;
		}


		private void ChatRoomOnConnected(object sender, EventArgs args) {
			if (InvokeRequired) {
				Invoke(new EventHandler(ChatRoomOnConnected), sender, args);
			}
			else {
				lblLog.Text = string.Format("#{0} - connected", Settings.Default.Channel);
				UpdateLcdScreen(this, EventArgs.Empty);
			}
		}

		private void ChatRoomOnDisconnected(object sender, EventArgs args) {
			if (InvokeRequired) {
				Invoke(new EventHandler(ChatRoomOnDisconnected), sender, args);
			}
			else {
				lblLog.Text = string.Format("#{0} - disconnected", Settings.Default.Channel);
				UpdateLcdScreen(this, EventArgs.Empty);
			}
		}

		int _lastBeepTick = 0;
		private void ChatRoomOnMessage(object sender, IRCMessageEventArgs msgArgs) {
			if (InvokeRequired) {
				Invoke(new EventHandler<IRCMessageEventArgs>(ChatRoomOnMessage), sender, msgArgs);
			}
			else {
				chat.AddMessage(new ChatMessage { Sender = msgArgs.User, Message = msgArgs.Message });
				UpdateLcdScreen(this, EventArgs.Empty);

				if ((Environment.TickCount - _lastBeepTick) > 5 * 1000) {
					using (var snd = new SoundPlayer(Properties.Resources.beep)) {
						snd.Play();
					}
					_lastBeepTick = Environment.TickCount;
				}
			}
		}

		private void ApplyTheme() {
			BackgroundImage = _currentTheme.Background;
			foreach (Control c in Controls) {
				if (c is Label || c is ChatControl) {
					c.ForeColor = _currentTheme.ForeColor;
					c.BackColor = _currentTheme.BackColor;
				}
			}
			UpdateLcdScreen(this, EventArgs.Empty);
		}


		void Device_Menu(object sender, EventArgs e) {
			var dlg = new SettingsDialog(Settings.Default);
			if (dlg.ShowDialog() == DialogResult.OK) {
				Settings.Default.Save();
				// if settings changed, enter new chatroom
				if ((_chatRoom != null) && (_chatRoom.Channel != Settings.Default.Channel ||
											_chatRoom.Username != Settings.Default.Username ||
											_chatRoom.Password != Settings.Default.Password)) {
					_chatRoom.Dispose();
					EnterChatroom();
				}
			}
		}

		public override string AppletName {
			get { return "TwitchChat"; }
		}

		public override event EventHandler UpdateLcdScreen;
	}
}
