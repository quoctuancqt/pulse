namespace Pulse.Core.Dto.Entity
{
    using Common.Helpers;
    using Common.ResolverFactories;
    using FluentValidation;
    using FluentValidation.Validators;
    using Services;

    public class CountryDtoValidator: AbstractValidator<CountryDto>
    {
        public CountryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required !")
                .Length(2, 100).WithMessage("The length of Name cannot exceed 100 characters.")
                .SetValidator(new UniqueCountryValidator());
        }

        public class UniqueCountryValidator : PropertyValidator
        {
            public UniqueCountryValidator()
                : base("Country Name is already used.")
            {

            }

            protected override bool IsValid(PropertyValidatorContext context)
            {
                if (context.Instance.GetValue<int>("Id") == 0)
                {
                    var countryService = ResolverFactory.GetService<ICountryService>();
                    string countryName = context.PropertyValue as string;

                    var countryDto = AsyncHelper.RunSync(() => countryService.FindByNameAsync(countryName));

                    return countryDto == null;
                }

                return true;
            }
        }
    }
}
