namespace Slipstream.Components
{
    internal interface ILuaGlueFactory
    {
        ILuaGlue CreateLuaGlue(IComponentPluginCreationContext ctx);
    }
}