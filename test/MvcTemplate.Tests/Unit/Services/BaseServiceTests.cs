using MvcTemplate.Data.Core;
using MvcTemplate.Services;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Services
{
    public class BaseServiceTests : IDisposable
    {
        private IUnitOfWork unitOfWork;
        private BaseService service;

        public BaseServiceTests()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            service = Substitute.ForPartsOf<BaseService>(unitOfWork);
        }
        public void Dispose()
        {
            service.Dispose();
        }

        #region Method: Dispose()

        [Fact]
        public void Dispose_DisposesUnitOfWork()
        {
            service.Dispose();

            unitOfWork.Received().Dispose();
        }

        [Fact]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            service.Dispose();
            service.Dispose();
        }

        #endregion
    }
}
