using Datalist;
using MvcTemplate.Components.Datalists;
using MvcTemplate.Controllers;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using NSubstitute;
using System;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class DatalistControllerTests : IDisposable
    {
        private DatalistController controller;
        private AbstractDatalist datalist;
        private IUnitOfWork unitOfWork;
        private DatalistFilter filter;

        public DatalistControllerTests()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            controller = Substitute.ForPartsOf<DatalistController>(unitOfWork);

            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            datalist = Substitute.For<AbstractDatalist>();
            filter = new DatalistFilter();
        }
        public void Dispose()
        {
            HttpContext.Current = null;
        }

        #region Method: GetData(AbstractDatalist datalist, DatalistFilter filter)

        [Fact]
        public void GetData_SetsDatalistCurrentFilter()
        {
            controller.GetData(datalist, filter);

            DatalistFilter actual = datalist.CurrentFilter;
            DatalistFilter expected = filter;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetData_ReturnsPublicJsonData()
        {
            datalist.GetData().Returns(new DatalistData());

            JsonResult actual = controller.GetData(datalist, filter);
            DatalistData expectedData = datalist.GetData();

            Assert.Equal(JsonRequestBehavior.AllowGet, actual.JsonRequestBehavior);
            Assert.Same(expectedData, actual.Data);
        }

        #endregion

        #region Method: Role(DatalistFilter filter)

        [Fact]
        public void Role_GetsRolesData()
        {
            controller.When(sub => sub.GetData(Arg.Any<BaseDatalist<Role, RoleView>>(), filter)).DoNotCallBase();
            controller.GetData(Arg.Any<BaseDatalist<Role, RoleView>>(), filter).Returns(new JsonResult());

            JsonResult expected = controller.GetData(null, filter);
            JsonResult actual = controller.Role(filter);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Fact]
        public void Dispose_DisposesUnitOfWork()
        {
            controller.Dispose();

            unitOfWork.Received().Dispose();
        }

        [Fact]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            controller.Dispose();
            controller.Dispose();
        }

        #endregion
    }
}
