namespace Slipstream.Components
{
    internal interface ILuaGlue
    {
        void SetupLua(NLua.Lua lua);

        void Loop();
    }
}