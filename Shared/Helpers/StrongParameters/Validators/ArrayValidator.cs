using System.Collections.Generic;

#nullable enable

namespace Slipstream.Shared.Helpers.StrongParameters.Validators
{
    internal class ArrayFailValidator : IValidator
    {
        public bool Required => false;

        public void Validate(object v)
        {
            throw new StrongParametersException("Array didn't define what it accepts");
        }
    }

    internal class ArrayValidator : IValidator
    {
        private IValidator Schema = new ArrayFailValidator();

        public bool Required { get; set; }

        public string TypeName => "Array";

        public void Validate(object input)
        {
            if (!(input is Dictionary<dynamic, dynamic>))
                throw new StrongParametersException($"2 Wrong input type: {input.GetType()}");

            var dictionary = (Dictionary<dynamic, dynamic>)input;
            int idx = 0;
            foreach (var k in dictionary.Keys)
            {
                var value = dictionary[k];

                try
                {
                    Schema.Validate(value);
                }
                catch (StrongParametersException e)
                {
                    throw new StrongParametersException($"[{idx}] {e.Message}");
                }

                idx++;
            }

            if (dictionary.Keys.Count == 0 && Required)
            {
                throw new StrongParametersException("Missing elements for array");
            }
        }

        public void PermitBoolean()
        {
            Schema = new BooleanValidator();
        }

        public void PermitFloat()
        {
            Schema = new FloatValidator();
        }

        public void PermitLong()
        {
            Schema = new LongValidator();
        }

        public void PermitString()
        {
            Schema = new StringValidator();
        }

        public void RequireBoolean()
        {
            Schema = new BooleanValidator { Required = true };
        }

        public void RequireFloat()
        {
            Schema = new FloatValidator { Required = true };
        }

        public void RequireLong()
        {
            Schema = new LongValidator { Required = true };
        }

        public void RequireString()
        {
            Schema = new StringValidator { Required = true };
        }
    }
}