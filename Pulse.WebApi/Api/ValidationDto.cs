namespace Pulse.WebApi.Api
{
    using FluentValidation.Results;
    using System.Collections.Generic;

    internal class ValidationDto 
    {
        public bool IsValid { get; private set; }

        public IList<IDictionary<string, string>> Errors { get; private set; }

        public ValidationDto()
        {
            IsValid = true;
        }

        public ValidationDto(ValidationResult validationResult)
        {
            IsValid = validationResult.IsValid;
            if (!validationResult.IsValid)
            {
                Errors = GetErrors(validationResult.Errors);
            }
        }

        private IList<IDictionary<string, string>> GetErrors(IList<ValidationFailure> Errors)
        {
            IList<IDictionary<string, string>> obj = new List<IDictionary<string, string>>();
            foreach (ValidationFailure error in Errors)
            {
                IDictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(error.PropertyName, error.ErrorMessage);
                obj.Add(dic);
            }
            return obj;
        }
    }
}
