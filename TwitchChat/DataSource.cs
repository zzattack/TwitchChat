using System;
using System.Threading;

namespace TwitchChat {

	public enum ReconnectBehavior {
		Report,
		Reconnect,
		Ignore
	};

	public abstract class DataSource : IDisposable {
		protected AutoResetEvent LineReadyEvent = new AutoResetEvent(false);
		public byte[] LastLine { get; set; }
		public abstract string Source { get; }
		protected AutoResetEvent _areStartStop = new AutoResetEvent(false);
		ReconnectBehavior ReconnectBehavior { get; set; }

		public DataSource() {
			ReconnectBehavior = ReconnectBehavior.Reconnect;
		}

		protected bool _isEnabled;
		public virtual bool IsEnabled {
			get { return _isEnabled; }
			set {
				if (_isEnabled != value) {
					if (value)
						Start();
					else
						Stop();
					_areStartStop.WaitOne();
				}
			}
		}

		public virtual bool Enabled {
			get { return false; }
			set {
				if (value && !Enabled)
					Start();
				else if (Enabled)
					Stop();
			}
		}

		public virtual void Dispose() {
			Stop();
		}

		public event DataReceivedEventHandler DataReceived;
		public abstract void Start();
		public virtual void Stop() { }

		protected virtual void OnDataReceived(byte[] data) {
			if (DataReceived != null)
				DataReceived(this, new DataReceivedEventArgs(data));
		}

		protected void ApplyReconnectBehavior() {
			if (ReconnectBehavior == ReconnectBehavior.Reconnect)
				ScheduleReconnect();
			else if (ReconnectBehavior == ReconnectBehavior.Ignore)
				return;
			else if (ReconnectBehavior == ReconnectBehavior.Report)
				Logger.Error("Connection aborted on {0}", this.Source);
		}

		Timer _t;
		void ScheduleReconnect() {
			Logger.Info("Scheduling reconnect in 3s on {0}", Source);
			_t = new Timer(delegate {
				Start();
			}, null, new TimeSpan(30000000), new TimeSpan(-1));
		}

	}
}
