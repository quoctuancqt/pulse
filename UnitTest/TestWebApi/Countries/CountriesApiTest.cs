namespace TestWebApi
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Pulse.Core.Dto.Entity;
    using Pulse.Core.Services;
    using Pulse.WebApi.Api;
    using Pulse.Domain;
    using Pulse.FakeData.FakeData;
    using System.Threading.Tasks;
    using System.Net;
    using Moq;
    using System.Linq.Expressions;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class CountriesApiTest : BaseTest<CountriesController, ICountryService, Country, CountryDto>
    {
        //https://www.asp.net/web-api/overview/testing-and-debugging/unit-testing-controllers-in-web-api
        //http://stackoverflow.com/questions/24674088/how-do-i-unit-test-a-post-web-api-call-with-a-token

        [TestMethod]
        public void GetByNameReturnsStatusOK()
        {
            var countryDto = GetDto(typeof(CountryDto).FullName, DtoRetriveMethods.FakeOne);

            _mockService.Setup(x => x.FindByNameAsync(countryDto.Name)).Returns(Task.FromResult(countryDto));
            var controller = InitController(new[] { _mockService.Object });

            var result = Execute<CountryDto>(() => controller.GetByName(countryDto.Name));

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var requestUrl = result.Response.RequestMessage.RequestUri.ToString();
            Assert.AreEqual(_url, requestUrl);
            Assert.AreEqual(API_PREFIX, GetPrefix(requestUrl));
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(countryDto.Name, result.Items.Name);
        }

        [TestMethod]
        public void SearchGroupReturnsStatusOK()
        {
            var countryDto = GetDto(typeof(CountryDto).FullName, DtoRetriveMethods.FakeOne);
            var total = 1;
            var items = new List<CountryDto> { countryDto };
            var pageResultDto = new PageResultDto<CountryDto>(total, items);
            _mockService.Setup(x => x.SearchAsync(It.IsAny<Expression<Func<Country, bool>>>(), 0, 10))
                .Returns(Task.FromResult(pageResultDto));
            var controller = InitController(new[] { _mockService.Object });

            var result = Execute<PageResultDto<CountryDto>>(() => controller.SearchCountry("name"));

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var requestUrl = result.Response.RequestMessage.RequestUri.ToString();
            Assert.AreEqual(_url, requestUrl);
            Assert.AreEqual(API_PREFIX, GetPrefix(requestUrl));
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(result.Items.TotalRecord, pageResultDto.TotalRecord);
            Assert.AreEqual(result.Items.ToTalPage, pageResultDto.ToTalPage);
        }
    }
}
