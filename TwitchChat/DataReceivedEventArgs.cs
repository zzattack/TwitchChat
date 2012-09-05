using System;

namespace TwitchChat {
	public class DataReceivedEventArgs : EventArgs {
		public readonly byte[] NewData;

		public DataReceivedEventArgs(byte[] data) {
			NewData = data;
		}

	}
}
