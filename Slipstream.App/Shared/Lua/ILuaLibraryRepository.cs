using System.Collections.Generic;

namespace Slipstream.Shared.Lua
{
    public interface ILuaLibraryRepository
    {
        public ILuaLibrary? Get(string name);

        public IEnumerable<string> GetAll();
    }
}