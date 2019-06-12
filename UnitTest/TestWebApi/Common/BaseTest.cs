namespace TestWebApi
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Pulse.Core.Mapper.WebApi;
    using System.Net.Http;
    using System.Web.Http;
    using Pulse.Common.Helpers;
    using System.Threading.Tasks;
    using System.Web.Http.Routing;
    using Pulse.Common.ResolverFactories;
    using Moq;
    using System.Configuration;
    using Pulse.Core.Dto.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using Pulse.WebApi.Api;
    using Pulse.Domain;
    using Pulse.Core.Services;
    using System.Net;
    using Pulse.FakeData.FakeData;

    [TestClass]
    public abstract class BaseTest<TController, TService, TEntity, TDto>
        where TController: BaseApiController<TEntity, TDto, TService>
        where TService: class, IServiceBase<TEntity, TDto>
        where TEntity: class, IEntity
        where TDto : class, IDto
    {
        protected const string API_PREFIX = "api";

        private readonly HttpRouteValueDictionary _httpRouteValueDictionary;
        protected readonly Mock<TService> _mockService;
        protected readonly string _url;
        private readonly IDictionary<string, IDictionary<DtoRetriveMethods, Func<object>>> _fakeDataMethodDic;

        protected BaseTest()
        {
            _mockService = new Mock<TService>();
            _httpRouteValueDictionary = InitHttpRouteValueDictionary();
            _url = GetUrl();
            AutoMapperConfiguration.Config();
            _fakeDataMethodDic = new FakeDataDto().FakeDataMethodDic;
        }

        [TestMethod]
        public void GetReturnsStatusOk()
        {
            var dtoToReturn = GetDto(typeof(TDto).FullName, DtoRetriveMethods.FakeOne);
            _mockService.Setup(x => x.FindByIdAsync(dtoToReturn.Id)).Returns(Task.FromResult(dtoToReturn));
            var controller = InitController(new[] { _mockService.Object });

            var result = Execute<CountryDto>(() => controller.Get(dtoToReturn.Id));

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(_url, result.Response.RequestMessage.RequestUri.ToString());
            Assert.AreEqual(API_PREFIX, GetPrefix(result.Response.RequestMessage.RequestUri.ToString()));
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(dtoToReturn.Id, result.Items.Id);
        }

        [TestMethod]
        public void GetReturnsStatusNotFound()
        {
            TDto nullDto = null;
            _mockService.Setup(x => x.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(nullDto));
            var controller = InitController(new[] { _mockService.Object });

            var result = Execute<CountryDto>(() => controller.Get(1));

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(_url, result.Response.RequestMessage.RequestUri.ToString());
            Assert.AreEqual(API_PREFIX, GetPrefix(result.Response.RequestMessage.RequestUri.ToString()));
        }

        [TestMethod]
        public virtual void GetAllReturnsStatusOK()
        {
            var controller = InitController(new[] { _mockService.Object });

            var result = Execute<IEnumerable<TDto>>(() => controller.GetAll());

            _mockService.Verify(x => x.FindAllAsync(null), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var requestUrl = result.Response.RequestMessage.RequestUri.ToString();
            Assert.AreEqual(_url, requestUrl);
            Assert.AreEqual(API_PREFIX, GetPrefix(requestUrl));
        }

        [TestMethod]
        public void PostReturnsStatusOK()
        {
            var dtoToCreate = GetDto(typeof(TDto).FullName, DtoRetriveMethods.FakeOne);
            _mockService.Setup(x => x.CreateAsync(dtoToCreate)).Returns(Task.FromResult(dtoToCreate));
            var controller = InitController(new[] { _mockService.Object });

            var result = Execute<GroupDto>(() => controller.Post(dtoToCreate));

            _mockService.Verify(x => x.CreateAsync(dtoToCreate), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var requestUrl = result.Response.RequestMessage.RequestUri.ToString();
            Assert.AreEqual(_url, requestUrl);
            Assert.AreEqual(API_PREFIX, GetPrefix(requestUrl));
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(dtoToCreate.Id, result.Items.Id);
        }

        [TestMethod]
        public void PostReturnsBadRequest()
        {
            var invalidDto = GetDto(typeof(TDto).FullName, DtoRetriveMethods.FakeInvalidOne);
            var controller = InitController(new[] { _mockService.Object });

            var result = Execute<GroupDto>(() => controller.Post(invalidDto));

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var requestUrl = result.Response.RequestMessage.RequestUri.ToString();
            Assert.AreEqual(_url, requestUrl);
            Assert.AreEqual(API_PREFIX, GetPrefix(requestUrl));
            var err = (IList<IDictionary<string, string>>)result.Error;
            Assert.IsNotNull(err);
            Assert.IsTrue(err.Any());
        }

        [TestMethod]
        public void PutReturnsStatusOK()
        {
            var dtoToUpdate = GetDto(typeof(TDto).FullName, DtoRetriveMethods.FakeOne);
            _mockService.Setup(x => x.UpdateAsync(dtoToUpdate)).Returns(Task.FromResult(dtoToUpdate));
            var controller = InitController(new[] { _mockService.Object });

            var result = Execute<GroupDto>(() => controller.Put(dtoToUpdate));

            _mockService.Verify(x => x.UpdateAsync(dtoToUpdate), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var requestUrl = result.Response.RequestMessage.RequestUri.ToString();
            Assert.AreEqual(_url, requestUrl);
            Assert.AreEqual(API_PREFIX, GetPrefix(requestUrl));
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(dtoToUpdate.Id, result.Items.Id);
        }

        [TestMethod]
        public void PutReturnsBadRequest()
        {
            var invalidDto = GetDto(typeof(TDto).FullName, DtoRetriveMethods.FakeInvalidOne);
            var controller = InitController(new[] { _mockService.Object });

            var result = Execute<GroupDto>(() => controller.Put(invalidDto));

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var requestUrl = result.Response.RequestMessage.RequestUri.ToString();
            Assert.AreEqual(_url, requestUrl);
            Assert.AreEqual(API_PREFIX, GetPrefix(requestUrl));
            var err = (IList<IDictionary<string, string>>)result.Error;
            Assert.IsNotNull(err);
            Assert.IsTrue(err.Any());
        }

        [TestMethod]
        public void DeleteReturnsStatusOK()
        {
            var dtoToDelete = GetDto(typeof(TDto).FullName, DtoRetriveMethods.FakeOne);
            var controller = InitController(new[] { _mockService.Object });

            var result = Execute<string>(() => controller.Delete(dtoToDelete));

            _mockService.Verify(x => x.DeleteAsync(dtoToDelete.Id), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var url = result.Response.RequestMessage.RequestUri.ToString();
            Assert.AreEqual(_url, url);
            Assert.AreEqual(API_PREFIX, GetPrefix(url));
        }

        protected string GetPrefix(string url)
        {
            var assets = url.Split('/');
            if (assets != null && assets.Length > 3)
            {
                return assets[3];
            }

            return string.Empty;
        }

        protected virtual TController InitController(object[] args)
        {
            var controller = ResolverFactory.CreateInstance<ApiController>(
                typeof(TController).AssemblyQualifiedName, args);

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri(_url)
            };

            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: _httpRouteValueDictionary );

            return controller as TController;
        }

        protected virtual TestResult<TDto> Execute<TDto>(Func<Task<IHttpActionResult>> func)
        {
            return new TestResult<TDto>(AsyncHelper.RunSync(func));
        }

        protected virtual TestResult<TDto> GetTestResult<TDto> (IHttpActionResult actionResult)
        {
            return new TestResult<TDto>(actionResult);
        }

        protected void GetDto<TFakeData>(out int key, out TFakeData Dto, IEnumerable<TFakeData> fakeData)
            where TFakeData: class, IDto
        {
            int id = GenerateId();
            key = id;
            Dto = fakeData.Where(x=>x.Id == id).FirstOrDefault();
        }

        protected TDto GetDto(string typeFullName, DtoRetriveMethods creationMethod)
        {
            if (!_fakeDataMethodDic.ContainsKey(typeFullName))
            {
                throw new Exception($"There are no methods to fake data for [{typeFullName}]. Please register for it.");
            }

            if (!_fakeDataMethodDic[typeFullName].ContainsKey(creationMethod))
            {
                throw new Exception($"There is no creation method named [{creationMethod}] for [{typeFullName}]. Please register for it.");
            }

            return _fakeDataMethodDic[typeFullName][creationMethod].Invoke() as TDto;
        }

        private HttpRouteValueDictionary InitHttpRouteValueDictionary()
        {
            var action = typeof(TController).Name.Replace("Controller","");
            return new HttpRouteValueDictionary { { "controller", action.ToLower() } };
        }

        private string GetUrl()
        {
            var serverUri = ConfigurationManager.AppSettings["WEBAPI_URI"];

            if (string.IsNullOrEmpty(serverUri)) throw new Exception("WEBAPI_URI is not empty please set it in app.config");

            RoutePrefixAttribute attr = (RoutePrefixAttribute)Attribute.GetCustomAttribute(typeof(TController), typeof(RoutePrefixAttribute));

            return string.Format("{0}{1}", serverUri, attr.Prefix);
        }

        protected int GenerateId(int start = 1, int end = 10)
        {
            Random rnd = new Random();
            return rnd.Next(start, end);
        }
    }
}
