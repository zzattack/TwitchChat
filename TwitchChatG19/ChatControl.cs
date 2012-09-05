using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwitchChat;

namespace TwitchChatG19 {
	internal class ChatMessage {
		public string Sender { get; set; }
		public string Message { get; set; }
	}

	class ChatControl : Control {

		public ChatControl() {
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		Queue<ChatMessage> messages = new Queue<ChatMessage>();
		internal void AddMessage(ChatMessage msg) {
			lock (messages) {
				messages.Enqueue(msg);
				while (messages.Count > 10) messages.Dequeue();
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent) {
		}

		StringFormat _leftAlign = new StringFormat { Alignment = StringAlignment.Near, Trimming = StringTrimming.Character };
		StringFormat _rightAlign = new StringFormat { Alignment = StringAlignment.Far, Trimming = StringTrimming.Character };

		protected override void OnPaint(PaintEventArgs e) {
			using (SolidBrush sb = new SolidBrush(ForeColor)) {
				lock (messages) {
					int y = 0;
					int yRemaining = Height;
					int messagesWritten = 0;

					foreach (ChatMessage msg in messages) {
						var strSize = e.Graphics.MeasureString(msg.Message, Font, new SizeF(Width - 90, 999999), _rightAlign);
						if (strSize.Height > yRemaining) {
							// the remaining messages dont fit - clear them
							while (messages.Count > messagesWritten) messages.Dequeue();
							break;
						}
						else {
							e.Graphics.DrawString(msg.Sender, Font, sb, new RectangleF(0, y, 80, strSize.Height), _rightAlign);
							e.Graphics.DrawString(msg.Message, Font, sb, new RectangleF(90, y, Width - 90f, strSize.Height), _leftAlign);
							y += (int)strSize.Height + 2;
							yRemaining -= (int)strSize.Height + 2;
							messagesWritten++;
						}
					}

				}
			}
		}


	}
}
