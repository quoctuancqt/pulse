namespace Pulse.Core.Dto.Entity
{
    using Common.Helpers;
    using Common.ResolverFactories;
    using FluentValidation;
    using FluentValidation.Validators;
    using Services;

    public class ClientDtoValidator : AbstractValidator<ClientDto>
    {
        public ClientDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 100).WithMessage("The length of Name cannot exceed 100 characters.")
                .SetValidator(new UniqueClientDtoValidator());

            RuleFor(x => x.TokenLifeTime).Must((x) => x >= 20)
                .WithMessage("The tokenLifeTime minimum value is 20 minutes");

            RuleFor(x => x.RefreshTokenLifeTime).Must((x) => x >= 7)
                .WithMessage("The tokenLifeTime minimum is 7 Days");
        }

        public class UniqueClientDtoValidator : PropertyValidator
        {
            public UniqueClientDtoValidator()
                : base("Client Name is already used.")
            {

            }

            protected override bool IsValid(PropertyValidatorContext context)
            {
                var clientService = ResolverFactory.GetService<IClientService>();

                if (context.Instance.GetValue<int>("Id") == 0)
                {
                    string clientName = context.PropertyValue as string;

                    var clientDto = AsyncHelper.RunSync(() => clientService.FindByNameAsync(clientName));

                    return clientDto == null;
                }

                return true;
            }
        }

    }
}
