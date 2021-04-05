using Slipstream.Shared;
using System;
using System.Linq;

namespace Slipstream.Components.Internal.LuaGlues
{
    public class RequireLuaGlue : ILuaGlue
    {
        private readonly ILuaLibraryRepository Repository;
        private static readonly Random random = new Random();

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public RequireLuaGlue(ILuaLibraryRepository repository)
        {
            Repository = repository;
        }

        public void SetupLua(NLua.Lua lua)
        {
            var hiddenRequireName = $"require_{RandomString(5)}"; // TODO: is there a better way?

            lua["slipstreamrequire"] = this;
            lua.DoString(@$"
local {hiddenRequireName} = require;

function require(n)
    local m = slipstreamrequire:require(n)

    if not m then
      m = {hiddenRequireName}(n)
    end

    return m
end");
        }

        public void Loop()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public ILuaLibrary require(string name)
        {
            return Repository.Get(name);
        }
    }
}