using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwitchChatG19.Properties;

namespace TwitchChatG19 {
	internal partial class SettingsDialog : Form {
		private SettingsDialog() {
			InitializeComponent();
		}

		public SettingsDialog(Settings settings)
			: this() {
			grid.SelectedObject = settings;
		}

	}
}
