namespace Pulse.Core.Dto.Entity
{
    using FluentValidation;
    using Common.Helpers;
    using Common.ResolverFactories;
    using FluentValidation.Validators;
    using Services;
    public class KioskDtoValidator : AbstractValidator<GroupDto>
    {
        public KioskDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .SetValidator(new UniqueKioskValidator());
        }
    }

    public class UniqueKioskValidator : PropertyValidator
    {
        public UniqueKioskValidator()
            : base("Kiosk Name is already used.")
        {
            
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var kioskService = ResolverFactory.GetService<IKioskService>();
            string kioskName = context.PropertyValue as string;

            var kioskDto = AsyncHelper.RunSync(() => kioskService.FindAllAsync(k => k.Name.ToLower().Equals(kioskName.ToLower())));

            return kioskDto == null;
        }
    }
}
