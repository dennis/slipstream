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
            TwitchChannelTextBox.Text = Properties.Settings.Default.TwitchChannel;
            TwitchTokenTextBox.Text = Properties.Settings.Default.TwitchToken;
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
            settings.Save();

            Close();
        }

        private void TwitchTokenGeneratorLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://twitchapps.com/tmi/");
        }
    }
}
