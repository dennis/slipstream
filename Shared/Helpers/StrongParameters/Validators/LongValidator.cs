#nullable enable

namespace Slipstream.Shared.Helpers.StrongParameters.Validators
{
    internal class LongValidator : IValidator
    {
        public bool Required { get; set; }

        public void Validate(object v)
        {
            if (!(v is long))
            {
                throw new StrongParametersException($"Expected 'long', got {v.GetType()}");
            }
        }
    }
}