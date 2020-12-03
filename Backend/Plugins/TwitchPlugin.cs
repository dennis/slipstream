using Slipstream.Shared;
using Slipstream.Shared.Events.Setting;
using Slipstream.Shared.Events.Twitch;
using Slipstream.Shared.Events.Utility;
using System;
using System.Diagnostics;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class TwitchPlugin : IPlugin
    {
        public string Id { get; }
        public string Name => "TwitchPlugin";
        public string DisplayName => Name;
        public bool Enabled { get; set; }
        public string WorkerName => Name;
        public EventHandler EventHandler { get; } = new EventHandler();

        private TwitchClient? Client;
        private readonly IEventBus EventBus;

        private string TwitchUsername;
        private string TwitchToken;

        public TwitchPlugin(string id, IEvent settings, IEventBus eventBus)
        {
            Id = id;
            EventBus = eventBus;

            if (settings is TwitchSettings typedSettings)
            {
                TwitchUsername = typedSettings.TwitchUsername;
                TwitchToken = typedSettings.TwitchToken;
            }                
            else
            { 
                throw new System.Exception($"Unexpected event as Exception {settings}");
            }

            EventHandler.OnSettingTwitchSettings += (s, e) =>
            {
                if(TwitchUsername != e.Event.TwitchUsername || TwitchToken != e.Event.TwitchToken)
                {
                    TwitchUsername = e.Event.TwitchUsername;
                    TwitchToken = e.Event.TwitchToken;

                    Disconnect();
                    Connnect();
                }
            };
            EventHandler.OnTwitchSendMessage += (s, e) =>
            {
                Client?.SendMessage(TwitchUsername, e.Event.Message);
            };
        }

        public void Disable(IEngine engine)
        {
            Disconnect();
        }

        private void Disconnect()
        {
            Client?.Disconnect();
            Client = null;
        }

        public void Enable(IEngine engine)
        {
            if (TwitchUsername.Length == 0 || TwitchToken.Length == 0)
            {
                EventBus.PublishEvent(new WriteToConsole { Message = "Twitch not configured" });
                EventBus.PublishEvent(new Shared.Events.Internal.PluginUnregister() { Id = this.Id });
                return;
            }

            Connnect();
        }

        private void Connnect()
        {
            if (!Enabled)
                return;

            ConnectionCredentials credentials = new ConnectionCredentials(TwitchUsername, TwitchToken, "ws://irc-ws.chat.twitch.tv:80");
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            WebSocketClient customClient = new WebSocketClient(clientOptions);
            Client = new TwitchClient(customClient);
            Client.Initialize(credentials, TwitchUsername);

            Client.OnLog += OnLog;
            Client.OnConnected += OnConnected;
            Client.OnChatCommandReceived += OnChatCommandReceived;
            Client.OnDisconnected += OnDisconnect;
            Client.OnError += Client_OnError;
            Client.OnIncorrectLogin += Client_OnIncorrectLogin;

            Client.Connect();
        }

        private void Client_OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            EventBus.PublishEvent(new WriteToConsole { Message = $"Twitch Error: {e.Exception.Message}" });
            EventBus.PublishEvent(new Shared.Events.Internal.PluginDisable() { Id = this.Id });
        }

        private void Client_OnError(object sender, OnErrorEventArgs e)
        {
            Debug.WriteLine($"Twitch: ERROR: {e.Exception.Message}");
        }

        private void OnDisconnect(object sender, OnDisconnectedEventArgs e)
        {
            EventBus.PublishEvent(new TwitchDisconnected());
        }

        private void OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            var chatMessage = e.Command.ChatMessage;

            if (chatMessage.IsMe)
                return;

            EventBus.PublishEvent(
                new TwitchReceivedCommand
                {
                    From = chatMessage.DisplayName,
                    Message = chatMessage.Message,
                    Broadcaster = chatMessage.IsBroadcaster,
                    Moderator = chatMessage.IsModerator,
                    Subscriber = chatMessage.IsSubscriber,
                    Vip = chatMessage.IsVip
                });
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            EventBus.PublishEvent(new TwitchConnected());
            EventBus.PublishEvent(new WriteToConsole { Message = $"Twitch connected as {TwitchUsername}" });
        }

        private void OnLog(object sender, OnLogArgs e)
        {
            Debug.WriteLine($"Twitch: {e.DateTime} {e.Data}");
        }

        public void Loop()
        {
        }

        public void RegisterPlugin(IEngine engine)
        {
        }

        public void UnregisterPlugin(IEngine engine)
        {
        }
    }
}
