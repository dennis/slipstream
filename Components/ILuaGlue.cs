namespace Slipstream.Components
{
    public interface ILuaGlue
    {
        void SetupLua(NLua.Lua lua);

        void Loop();
    }
}