namespace Pulse.Core.Services
{
    using Dto.Entity;
    using Domain;
    using System.Threading.Tasks;
    using System;

    public class HistoryService : ServiceBase<History, HistoryDto>, IHistoryService
    {

        private readonly IUserProfileService _userProfileService;

        public HistoryService(IUnitOfWork unitOfWork, IUserProfileService userProfileService) : base(unitOfWork)
        {
            _repository = unitOfWork.Histories;
            _userProfileService = userProfileService;
        }

        public async override Task<HistoryDto> CreateAsync(HistoryDto model)
        {
            if (!string.IsNullOrEmpty(CurrentUserId))
            {
                var userProfile = await _userProfileService.FindByUserIdAsync(CurrentUserId);
                model.FullName = userProfile.FullName;
            }

            model.UserId = (string.IsNullOrEmpty(model.UserId) ? CurrentUserId: model.UserId);
            
            model.CreatedDate = DateTime.Now;

            model.ClientId = string.IsNullOrEmpty(ClientId) ? model.ClientId : ClientId;

            _repository.Add(DtoToEntity(model));

            await _unitOfWork.CommitAsync();

            return model;
        }

        public override Task<HistoryDto> UpdateAsync(HistoryDto model)
        {
            throw new NotImplementedException();
        }
    }
}
