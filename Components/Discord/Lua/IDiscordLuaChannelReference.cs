namespace Slipstream.Components.Discord.Lua
{
    public interface IDiscordLuaChannelReference
    {
        void send_message(string message);
        void send_message_tts(string message);
    }
}