using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using LgLcd;

namespace TwitchChatG19 {

	/// <summary> Registers a window for some keyboard/broadcast hooks </summary>
	public class Hooks : IDisposable {

		Device _device;
		HookListenerWindow _hookListenerWindow = new HookListenerWindow();

		public Hooks(Device device) {
			_device = device;

			_hookListenerWindow.SetForegroundApplet += delegate {
				_device.SetAsLCDForegroundApp(true);
			};
		}
		
		#region interop crap
		public const int WM_HOTKEY = 0x0312;
		public const int HWND_BROADCAST = 0xFFFF;
		public static readonly int WM_LGLCD_TWITCH_SETFOREGROUND = RegisterWindowMessage("WM_LGLCD_TWITCH_SETFOREGROUND");

		[DllImport("user32")]
		public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

		[DllImport("user32")]
		public static extern int RegisterWindowMessage(string message);

		[DllImport("user32.dll")]
		static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		[DllImport("user32.dll")]
		static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		#endregion

		/// <summary>
		/// Represents the window that is used internally to get the messages.
		/// </summary>
		sealed class HookListenerWindow : NativeWindow, IDisposable {
			public event EventHandler SetForegroundApplet;

			public HookListenerWindow() {
				// create the handle for the window.
				CreateHandle(new CreateParams());
			}

			protected override void WndProc(ref Message m) {
				// check if we got a hot key pressed.
				if (m.Msg == WM_LGLCD_TWITCH_SETFOREGROUND) {
					if (SetForegroundApplet != null)
						SetForegroundApplet(this, EventArgs.Empty);
				}
				else {
					base.WndProc(ref m);
				}
			}

			public void Dispose() {
				DestroyHandle();
			}
		}

		public void Dispose() {
			_hookListenerWindow.Dispose();
		}

		public static void BroadcastForegroundRequest() {
			PostMessage((IntPtr)HWND_BROADCAST, WM_LGLCD_TWITCH_SETFOREGROUND, new IntPtr(0xCDCD), new IntPtr(0xEFEF));
		}
	}
	
}