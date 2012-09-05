using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace TwitchChatG19 {
	static class Program {

		static Mutex _mutex = new Mutex(true, "{12D03875-5765-487D-9C58-AAE26D7992A5}");

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			if (_mutex.WaitOne(TimeSpan.Zero, true)) {
				Application.EnableVisualStyles();
				try {
					new MainForm();
					Application.Run();
				}
				catch {
				}
				finally {
					_mutex.ReleaseMutex();
				}

			}
			else {
				Hooks.BroadcastForegroundRequest();
			}
		}

	}
}
