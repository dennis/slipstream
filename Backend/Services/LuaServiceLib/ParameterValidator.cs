using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public partial class LuaContext
    {
        private class ParameterValidator
        {
            private bool AllowUnknownArguments;
            private readonly Dictionary<string, Type> Schema = new Dictionary<string, Type>();
            private readonly List<string> RequriedKeys = new List<string>();

            public static ParameterValidator Create()
            {
                return new ParameterValidator();
            }

            internal ParameterValidator Permit(string key, Type type)
            {
                if (type == typeof(string) || type == typeof(long) || type == typeof(float) || type == typeof(bool))
                {
                    Schema.Add(key, type);
                    return this;
                }

                throw new ParameterException($"Unsupported type = '{type}'");
            }

            internal ParameterValidator AllowAnythingElse()
            {
                AllowUnknownArguments = true;

                return this;
            }

            internal ParameterValidator Required(string key, Type type)
            {
                Permit(key, type);
                RequriedKeys.Add(key);
                return this;
            }

            internal ParameterResult Parse(LuaTable source)
            {
                var result = new ParameterResult();

                foreach (var k in source.Keys)
                {
                    if (k.GetType() != typeof(string))
                    {
                        throw new ParameterException($"Argument-name is expected to be a string {k}");
                    }
                    else
                    {
                        var value = source[(string)k];

                        if (Schema.ContainsKey((string)k))
                        {
                            var type = Schema[(string)k];

                            if (type == value.GetType())
                            {
                                result.Add((string)k, value);
                            }
                            else
                            {
                                throw new ParameterException($"Wrong type for argument '{k}'. Expected '{type}', got '{source[k].GetType()}'");
                            }
                        }
                        else
                        {
                            if (AllowUnknownArguments)
                            {
                                result.Add((string)k, value);
                            }
                            else
                            {
                                throw new ParameterException($"Unknown argument '{k}'");
                            }
                        }
                    }
                }

                foreach (var k in RequriedKeys)
                {
                    if (!result.ContainsKey(k))
                    {
                        throw new ParameterException($"Missing required argument '{k}'");
                    }
                }

                return result;
            }
        }
    }
}