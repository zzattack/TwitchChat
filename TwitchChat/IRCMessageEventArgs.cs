using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitchChat {
	public class IRCMessageEventArgs : EventArgs {
		public readonly DateTime When;
		public readonly string User;
		public readonly string Message;
		public IRCMessageEventArgs(DateTime when, string user, string message) {
			this.When = when;
			this.User = user;
			this.Message = message;
		}

	}
}
