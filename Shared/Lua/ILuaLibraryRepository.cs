namespace Slipstream.Shared.Lua
{
    public interface ILuaLibraryRepository
    {
        public ILuaLibrary? Get(string name);
    }
}