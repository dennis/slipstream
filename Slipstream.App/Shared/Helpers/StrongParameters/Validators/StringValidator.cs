#nullable enable

namespace Slipstream.Shared.Helpers.StrongParameters.Validators
{
    internal class StringValidator : IValidator
    {
        public bool Required { get; set; }

        public void Validate(object v)
        {
            if (!(v is string))
            {
                throw new StrongParametersException($"Expected 'string', got {v.GetType()}");
            }
        }
    }
}