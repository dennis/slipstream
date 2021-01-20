using System;
using System.Collections.Generic;

#nullable enable

namespace Slipstream.Shared.Helpers.StrongParameters.Validators
{
    internal class DictionaryValidator : IValidator
    {
        private readonly Dictionary<string, IValidator> Schema = new Dictionary<string, IValidator>();
        private bool AllowExtras = false;

        public bool Required { get; set; }

        public void Validate(object input)
        {
            if (!(input is Dictionary<dynamic, dynamic> dictionary))
                throw new StrongParametersException($"1 Wrong input type: {input.GetType()}");

            var seenParameters = new List<string>();

            foreach (var k in dictionary.Keys)
            {
                var value = dictionary[k];

                if (k.GetType() != typeof(String))
                {
                    throw new StrongParametersException("Expected dictionary (key/values), got array");
                }

                var key = (string)k; // LuaTable keys are always strings for dictionaries

                if (Schema.TryGetValue(key, out IValidator validator))
                {
                    try
                    {
                        validator.Validate(value);
                    }
                    catch (StrongParametersException e)
                    {
                        throw new StrongParametersException($"{key} {e.Message}");
                    }

                    seenParameters.Add((string)key);
                }
                else
                {
                    if (!AllowExtras)
                    {
                        throw new StrongParametersException($"{key} Unexpected argument");
                    }
                }
            }

            // Check if we are missing some parameters
            foreach (var validator in Schema)
            {
                if (validator.Value.Required && !seenParameters.Contains(validator.Key))
                {
                    throw new StrongParametersException($"Missing required argument '{validator.Key}'");
                }
            }
        }

        public DictionaryValidator PermitArray(string name, Action<ArrayValidator> func)
        {
            var validator = new ArrayValidator();

            func(validator);

            Schema.Add(name, validator);
            return this;
        }

        public DictionaryValidator RequireArray(string name, Action<ArrayValidator> func)
        {
            var validator = new ArrayValidator { Required = true };

            func(validator);

            Schema.Add(name, validator);
            return this;
        }

        public DictionaryValidator PermitString(string name)
        {
            Schema.Add(name, new StringValidator());
            return this;
        }

        public DictionaryValidator RequireString(string name)
        {
            Schema.Add(name, new StringValidator { Required = true });
            return this;
        }

        public DictionaryValidator PermitDictionary(string name, Action<DictionaryValidator> func)
        {
            var validator = new DictionaryValidator();

            func(validator);

            Schema.Add(name, validator);

            return this;
        }

        public DictionaryValidator RequireBool(string name)
        {
            Schema.Add(name, new BooleanValidator { Required = true });
            return this;
        }

        public DictionaryValidator PermitBool(string name)
        {
            Schema.Add(name, new BooleanValidator());
            return this;
        }

        public DictionaryValidator RequireFloat(string name)
        {
            Schema.Add(name, new FloatValidator { Required = true });
            return this;
        }

        public DictionaryValidator PermitFloat(string name)
        {
            Schema.Add(name, new FloatValidator());
            return this;
        }

        internal DictionaryValidator RequireLong(string name)
        {
            Schema.Add(name, new LongValidator { Required = true });
            return this;
        }

        public DictionaryValidator PermitLong(string name)
        {
            Schema.Add(name, new LongValidator());
            return this;
        }

        public DictionaryValidator AllowAnythingElse()
        {
            AllowExtras = true;
            return this;
        }
    }
}