namespace Slipstream.Components.Discord.Lua
{
    public interface IDiscordLuaChannelReference
    {
#pragma warning disable IDE1006 // Naming Styles
        void send_message(string message);
        void send_message_tts(string message);
#pragma warning restore IDE1006 // Naming Styles
    }
}