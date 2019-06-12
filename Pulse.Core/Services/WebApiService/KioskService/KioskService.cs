namespace Pulse.Core.Services
{
    using System.Threading.Tasks;
    using Domain;
    using Dto.Entity;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System;
    using Domain.Enum;
    using System.Linq.Expressions;

    public class KioskService : ServiceBase<Kiosk, KioskDto>, IKioskService
    {
        public KioskService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repository = unitOfWork.Kiosks;
        }

        public async Task<IEnumerable<KioskDto>> FindByCountryIdAsync(int countryId)
        {
            return await FindAllAsync(k => k.CountryId == countryId);
        }

        public async Task<KioskDto> FindByMachineIdAsync(string machineId)
        {
            return EntityToDto(await FindAll(k => k.MachineId.Equals(machineId)).FirstOrDefaultAsync());
        }

        public override async Task<KioskDto> UpdateAsync(KioskDto model)
        {
            var entity = await _repository.FindByAsync(model.Id);

            if (entity == null) throw new Exception("Not found entity object with id: " + model.Id);

            model.DefaultValue = model.DefaultValue == null ? entity.DefaultValue : model.DefaultValue;

            entity = DtoToEntity(model, entity);
            
            _repository.Update(entity);

            await _unitOfWork.CommitAsync();

            return EntityToDto(entity);
        }

        public async Task UpdateConnectionIdAsync(string machineId, string connectionId)
        {
            var entity = await FindAll(x => x.MachineId.Equals(machineId)).FirstOrDefaultAsync();

            if (entity == null) throw new Exception("KioskService ==> UpdateConnectionIdAsync: not found with machineId: " + machineId + " clientId: "+ ClientId);

            entity.ConnectionId = connectionId;

            _unitOfWork.Kiosks.Update(entity);

            await _unitOfWork.CommitAsync();
        }

        public async Task<KioskDto> UpdateStatusByMachineIdAsync(string machineId, KioskStatus kioskStatus)
        {
            var entity = await FindAll(k => k.MachineId.Equals(machineId)).FirstOrDefaultAsync();

            if (entity == null) throw new Exception("Not found entity object with machineId: " + machineId);

            entity.Status = kioskStatus;

            _repository.Update(entity);

            await _unitOfWork.CommitAsync();

            return EntityToDto(entity);
        }

    }
}
