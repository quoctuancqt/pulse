namespace Pulse.FakeData
{
    using Core.Dto.Entity;
    using Domain;
    using FakeData;
    using FizzWare.NBuilder;
    using System.Collections.Generic;
    using System.Linq;
    public class FakeCountries : IDtoFaker<CountryDto>
    {
        //http://www.jerriepelser.com/blog/creating-test-data-with-nbuilder-and-faker/
        public static IEnumerable<CountryDto> FakeDataCountriesDto(int record = 10, bool hasKiosks = true, bool hasValue = true)
        {
            var result = Builder<CountryDto>.CreateListOfSize(record)
                .All()
                .With(c => c.Name = (hasValue ? Faker.Name.First() : null))
                .With(c => c.Kiosks = (hasKiosks ? FakeKiosks.FakeDataKioskDto() : null))
                .Build();


            return result;
        }

        public static IEnumerable<Country> FakeDataCountries(int record = 10, bool hasKiosks = true, bool hasValue = true)
        {
            IEnumerable<Country> result = Builder<Country>.CreateListOfSize(record)
                .All()
                .With(c => c.Name = (hasValue ? Faker.Name.First() : null))
                .With(c => c.Kiosks = (hasKiosks ? FakeKiosks.FakeDataKiosk().ToList() : null))
                .Build();
            return result;
        }

        public CountryDto CreateDto()
        {
            var countryDto = new CountryDto
            {
                Code = "test code",
                Id = 1,
                Name = "test country"
            };

            return countryDto;
        }

        public CountryDto CreateInvalidDto()
        {
            var countryDto = new CountryDto
            {
                Code = "test code",
                Id = 1
            };

            return countryDto;
        }
    }
}
