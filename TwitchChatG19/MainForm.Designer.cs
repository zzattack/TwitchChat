namespace TwitchChatG19 {
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.lblLog = new System.Windows.Forms.Label();
			this.chat = new TwitchChatG19.ChatControl();
			this.SuspendLayout();
			// 
			// lblLog
			// 
			this.lblLog.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLog.Location = new System.Drawing.Point(0, 0);
			this.lblLog.Name = "lblLog";
			this.lblLog.Size = new System.Drawing.Size(320, 22);
			this.lblLog.TabIndex = 1;
			this.lblLog.Text = "#zzattack - connected";
			this.lblLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chat
			// 
			this.chat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chat.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chat.Location = new System.Drawing.Point(0, 22);
			this.chat.Name = "chat";
			this.chat.Size = new System.Drawing.Size(320, 218);
			this.chat.TabIndex = 2;
			this.chat.Text = "chatControl1";
			// 
			// MainForm
			// 
			this.Controls.Add(this.chat);
			this.Controls.Add(this.lblLog);
			this.Name = "MainForm";
			this.Size = new System.Drawing.Size(320, 240);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblLog;
		private ChatControl chat;
	}
}

