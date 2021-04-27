using Autofac;
using System.Collections.Generic;
using Slipstream.Backend;

namespace Slipstream.Shared.Lua
{
    public class LuaLibraryRepository : ILuaLibraryRepository
    {
        private readonly List<ILuaLibrary> LuaLibraries = new List<ILuaLibrary>();

        public LuaLibraryRepository(ILifetimeScope scope)
        {
            foreach (var type in scope.GetImplementingTypes<ILuaLibraryAutoRegistration>())
            {
                LuaLibraries.Add((ILuaLibrary)scope.Resolve(type));

                System.Diagnostics.Debug.WriteLine($"Registering {type}");
            }
        }

        public ILuaLibrary Get(string name)
        {
            return LuaLibraries.Find(a => a.Name == name);
        }
    }
}