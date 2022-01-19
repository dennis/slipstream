#nullable enable

namespace Slipstream.Shared.Helpers.StrongParameters.Validators
{
    internal interface IValidator
    {
        bool Required { get; }

        void Validate(object v);
    }
}