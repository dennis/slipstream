using System.Collections.Generic;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public partial class LuaContext
    {
        private class ParameterResult
        {
            private readonly Dictionary<string, dynamic> Values = new Dictionary<string, dynamic>();

            internal void Add(string key, dynamic value)
            {
                Values.Add(key, value);
            }

            internal Dictionary<string, dynamic> AsDictionary()
            {
                return Values;
            }

            internal bool ContainsKey(string k)
            {
                return Values.ContainsKey(k);
            }

            internal T Extract<T>(string key)
            {
                var raw = Get<T>(key);

                Values.Remove(key);

                return raw;
            }

            internal T ExtractOrDefault<T>(string key, T @default)
            {
                var raw = GetOrDefault(key, @default);

                if (Values.ContainsKey(key))
                    Values.Remove(key);

                return raw;
            }

            internal T Get<T>(string key)
            {
                return (T)Values[key];
            }

            internal T GetOrDefault<T>(string key, T @default)
            {
                if (Values.TryGetValue(key, out dynamic raw))
                {
                    return (T)raw;
                }
                else
                {
                    return @default;
                }
            }
        }
    }
}