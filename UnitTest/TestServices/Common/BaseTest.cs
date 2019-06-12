namespace TestServices
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pulse.Core.Mapper.WebApi;
    using Pulse.Core.Repository.Entity;
    using Pulse.Core.Services;
    using Pulse.Domain;
    using Pulse.FakeData.FakeDB;

    [TestClass]
    public abstract class BaseTest<TEntity>
        where TEntity: class, IEntity
    {
         
        protected readonly Mock<IUnitOfWork> _mockUnitOfWork;
        protected readonly Mock<IRepository<TEntity>> _mockRepository;
        protected readonly FakeDbSet<IEntity> _dbSet;

        public BaseTest()
        {
            AutoMapperConfiguration.Config();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IRepository<TEntity>>();
            _dbSet = new FakeDbSet<IEntity>();
            //_mockUnitOfWork.Setup(x => x.Countries).Returns(_mockRepository.Object);
            //mockService.Setup(x => x.Countries.FindAll(null)).Returns(dbSet);
        }
      
    }
}
