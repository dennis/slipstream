namespace Slipstream.Shared
{
    public interface ILuaLibraryRepository
    {
        public ILuaLibrary Get(string name);
    }
}