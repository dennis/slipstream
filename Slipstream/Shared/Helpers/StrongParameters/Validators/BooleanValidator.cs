#nullable enable

namespace Slipstream.Shared.Helpers.StrongParameters.Validators
{
    internal class BooleanValidator : IValidator
    {
        public bool Required { get; set; }

        public void Validate(object v)
        {
            if (!(v is bool))
            {
                throw new StrongParametersException($"Expected 'bool', got {v.GetType()}");
            }
        }
    }
}