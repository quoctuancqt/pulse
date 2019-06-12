namespace Pulse.Core.Dto.Entity
{
    using FluentValidation;
    using Common.Helpers;
    using Common.ResolverFactories;
    using FluentValidation.Validators;
    using Services;
    public class GroupDtoValidator: AbstractValidator<GroupDto>
    {
        public GroupDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .SetValidator(new UniqueGroupValidator());
        }
    }

    public class UniqueGroupValidator : PropertyValidator
    {
        public UniqueGroupValidator()
            : base("Group Name is already used.")
        {
            
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if(context.Instance.GetValue<int>("Id") == 0)
            {
                var groupService = ResolverFactory.GetService<IGroupService>();
                string groupName = context.PropertyValue as string;

                var groupDto = AsyncHelper.RunSync(() => groupService.FindNameAsync(groupName));

                return groupDto == null;
            }

            return true;
        }
    }
}
