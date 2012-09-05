namespace TwitchChatG19 {
	partial class SettingsDialog {
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
			this.grid = new System.Windows.Forms.PropertyGrid();
			this.btmOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.Location = new System.Drawing.Point(12, 12);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(320, 231);
			this.grid.TabIndex = 0;
			// 
			// btmOk
			// 
			this.btmOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btmOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btmOk.Location = new System.Drawing.Point(257, 253);
			this.btmOk.Name = "btmOk";
			this.btmOk.Size = new System.Drawing.Size(75, 23);
			this.btmOk.TabIndex = 1;
			this.btmOk.Text = "Ok";
			this.btmOk.UseVisualStyleBackColor = true;
			// 
			// SettingsDialog
			// 
			this.AcceptButton = this.btmOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(344, 288);
			this.Controls.Add(this.btmOk);
			this.Controls.Add(this.grid);
			this.Name = "SettingsDialog";
			this.Text = "SettingsDialog";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PropertyGrid grid;
		private System.Windows.Forms.Button btmOk;
	}
}