namespace TestServices.Countries
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pulse.Common.Helpers;
    using Pulse.Core.Dto.Entity;
    using Pulse.Core.Mapper.WebApi;
    using Pulse.Core.Repository.Entity;
    using Pulse.Core.Services;
    using Pulse.Domain;
    using Pulse.FakeData;
    using Pulse.FakeData.FakeDB;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    [TestClass]
    public class CountriesServiceTest
    {
        private object _syncObject = new object();

        [TestMethod]
        public void TestFindAllAsync()
        {
            AutoMapperConfiguration.Config();

            var dbSet = new FakeDbSet<Country>();
            dbSet.AddRange(FakeCountries.FakeDataCountries());

            var countryRepository = new Mock<IRepository<Country>>();
            var mockService = new Mock<IUnitOfWork>();

            mockService.Setup(x => x.Countries).Returns(countryRepository.Object);
            mockService.Setup(x => x.Countries.FindAll(null)).Returns(dbSet);

            //var countryService = new CountryService(mockService.Object);
            //var subsites = countryService.FindAllAsync().Result;
        }

        [TestMethod]
        public void TestCreateAsync()
        {
            AutoMapperConfiguration.Config();

            var countryRepository = new Mock<IRepository<Country>>();
            var mockService = new Mock<IUnitOfWork>();

            var countryDto = FakeCountries.FakeDataCountriesDto(hasKiosks:false).FirstOrDefault();

            mockService.Setup(x => x.Countries).Returns(countryRepository.Object);
            mockService.Setup(x => x.CommitAsync()).Returns(Task.FromResult(0)).Verifiable();
            //var countryService = new CountryService(mockService.Object);
            //var subsites = countryService.CreateAsync(countryDto).Result;
            //mockService.Verify(x => x.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public void TestUpdateAsync()
        {
            AutoMapperConfiguration.Config();

            var countryRepository = new Mock<IRepository<Country>>();
            var mockService = new Mock<IUnitOfWork>();

            var dbSet = new FakeDbSet<Country>();
            dbSet.AddRange(FakeCountries.FakeDataCountries());

            countryRepository.Setup(x => x.FindByAsync(1))
                .Returns(dbSet.FindAsync(new CancellationToken(false), 1));

            var countryDto = FakeCountries.FakeDataCountriesDto(hasKiosks: false).FirstOrDefault();
            mockService.Setup(x => x.Countries).Returns(countryRepository.Object);

            mockService.Setup(x => x.CommitAsync()).Returns(Task.FromResult(0)).Verifiable();

            //var countryService = new CountryService(mockService.Object);

            //var subsites = countryService.UpdateAsync(countryDto).Result;

            //mockService.Verify(x => x.CommitAsync(), Times.Once);
        }

    }
}
