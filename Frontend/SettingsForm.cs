using System;
using System.Windows.Forms;

namespace Slipstream.Frontend
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            var settings = Properties.Settings.Default;

            TwitchUsernameTextBox.Text = settings.TwitchUsername;
            TwitchChannelTextBox.Text = settings.TwitchChannel;
            TwitchTokenTextBox.Text = settings.TwitchToken;
            TwitchLogCheckBox.Checked = settings.TwitchLog;

            TXRXHostPortTextBox.Text = settings.TxrxIpPort;
        }

        private void DiscardButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            var settings = Properties.Settings.Default;

            settings.TwitchChannel = TwitchChannelTextBox.Text;
            settings.TwitchUsername = TwitchUsernameTextBox.Text;
            settings.TwitchToken = TwitchTokenTextBox.Text;
            settings.TwitchLog = TwitchLogCheckBox.Checked;
            settings.TxrxIpPort = TXRXHostPortTextBox.Text;

            settings.Save();

            DialogResult = DialogResult.OK;

            Close();
        }

        private void TwitchTokenGeneratorLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://twitchapps.com/tmi/");
        }
    }
}
