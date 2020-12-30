using System;
using System.Windows.Forms;

namespace Slipstream.Frontend
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            TwitchUsernameTextBox.Text = Properties.Settings.Default.TwitchUsername;
            TwitchTokenTextBox.Text = Properties.Settings.Default.TwitchToken;
        }

        private void DiscardButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            var settings = Properties.Settings.Default;

            settings.TwitchToken = TwitchTokenTextBox.Text;
            settings.TwitchUsername = TwitchUsernameTextBox.Text;
            settings.TwitchChannel = TwitchUsernameTextBox.Text; // TODO: Change me
            settings.Save();

            Close();
        }
    }
}
