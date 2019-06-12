namespace TestWebApi.Groups
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pulse.Core.Dto.Entity;
    using Pulse.Core.Services;
    using Pulse.FakeData.FakeData;
    using Pulse.WebApi.Api;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading.Tasks;

    [TestClass]
    public class GroupsApiTest : BaseTest<GroupsController, IGroupService, Pulse.Domain.Group, GroupDto>
    {
        [TestMethod]
        public void GetByNameReturnsStatusOK()
        {
            var groupDto = GetDto(typeof(GroupDto).FullName, DtoRetriveMethods.FakeOne);

            _mockService.Setup(x => x.FindNameAsync(groupDto.Name)).Returns(Task.FromResult(groupDto));
            var controller = InitController(new[] { _mockService.Object });

            var result = Execute<GroupDto>(() => controller.GetByName(groupDto.Name));

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var requestUrl = result.Response.RequestMessage.RequestUri.ToString();
            Assert.AreEqual(_url, requestUrl);
            Assert.AreEqual(API_PREFIX, GetPrefix(requestUrl));
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(groupDto.Name, result.Items.Name);
        }

        [TestMethod]
        public void SearchGroupReturnsStatusOK()
        {
            var groupDto = GetDto(typeof(GroupDto).FullName, DtoRetriveMethods.FakeOne);
            var total = 1;
            var items = new List<GroupDto> { groupDto };
            var pageResultDto = new PageResultDto<GroupDto>(total, items);
            _mockService.Setup(x => x.SearchAsync(It.IsAny<Expression<Func<Pulse.Domain.Group, bool>>>(), 0, 10))
                .Returns(Task.FromResult(pageResultDto));
            var controller = InitController(new[] { _mockService.Object });

            var result = Execute<PageResultDto<GroupDto>>(() => controller.SearchGroup("name"));

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
