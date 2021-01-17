using Newtonsoft.Json.Linq;
using NLua;
using System.Collections.Generic;

#nullable enable

namespace Slipstream.Shared.Helpers.StrongParameters
{
    public class Parameters : Dictionary<dynamic, dynamic>
    {
        public Parameters()
        {
        }

        public Parameters(IDictionary<dynamic, dynamic> dictionary) : base(dictionary)
        {
        }

        internal static Parameters From(LuaTable luaTable)
        {
            var result = new Parameters();

            foreach (var k in luaTable.Keys)
            {
                var v = luaTable[k];

                if (v is LuaTable t)
                {
                    result.Add(k, From(t));
                }
                else
                {
                    result.Add(k, luaTable[k]);
                }
            }

            return result;
        }

        internal static Parameters From(JToken dictionary)
        {
            var result = ParseJsonObject(dictionary);

            return new Parameters(result);
        }

        private static Dictionary<dynamic, dynamic> ParseJsonObject(JToken dictionary)
        {
            var result = new Dictionary<dynamic, dynamic>();

            if (dictionary.Type != JTokenType.Object)
            {
                throw new StrongParametersException($"Unexpected type of JToken. Expected Object got {dictionary.Type}");
            }

            foreach (var pair in dictionary.Children())
            {
                switch (pair.Type)
                {
                    case JTokenType.Property:
                        string name = ((JProperty)pair).Name;
                        foreach (var i in ((JProperty)pair).AsJEnumerable())
                        {
                            switch (i.Type)
                            {
                                case JTokenType.String:
                                    result.Add(name, (string)pair!);
                                    break;

                                case JTokenType.Integer:
                                    result.Add(name, (long)pair);
                                    break;

                                case JTokenType.Object:
                                    result.Add(name, ParseJsonObject(i));
                                    break;

                                case JTokenType.Boolean:
                                    result.Add(name, (bool)pair);
                                    break;

                                case JTokenType.Float:
                                    result.Add(name, (float)pair);
                                    break;

                                default:
                                    throw new System.Exception($"Unhandled type: {i.Type}");
                            }
                        }
                        break;

                    default:
                        throw new System.Exception($"Unhandled type: {pair.Type}");
                }
            }

            return result;
        }

        internal T Extract<T>(string name)
        {
            T value = (T)this[name];

            Remove(name);

            return value;
        }

        internal T ExtractOrDefault<T>(string name, T defaultValue)
        {
            if (TryGetValue(name, out dynamic _))
            {
                return Extract<T>(name);
            }
            else
            {
                return defaultValue;
            }
        }
    }
}