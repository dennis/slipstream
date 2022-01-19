#nullable enable

namespace Slipstream.Shared.Helpers.StrongParameters.Validators
{
    internal class FloatValidator : IValidator
    {
        public bool Required { get; set; }
        public string TypeName => "Float";

        public void Validate(object v)
        {
            if (!(v is double))
            {
                throw new StrongParametersException($"Expected 'double', got {v.GetType()}");
            }
        }
    }
}