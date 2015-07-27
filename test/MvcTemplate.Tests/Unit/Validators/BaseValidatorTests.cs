using MvcTemplate.Data.Core;
using MvcTemplate.Validators;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Services
{
    public class BaseValidatorTests : IDisposable
    {
        private BaseValidator validator;
        private IUnitOfWork unitOfWork;

        public BaseValidatorTests()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            validator = Substitute.ForPartsOf<BaseValidator>(unitOfWork);
        }
        public void Dispose()
        {
            validator.Dispose();
        }

        #region Constructor: BaseValidator(IUnitOfWork unitOfWork)

        [Fact]
        public void BaseValidator_CreatesEmptyModelState()
        {
            Assert.Empty(validator.ModelState);
        }

        [Fact]
        public void BaseValidator_CreateEmptyAlertsContainer()
        {
            Assert.Empty(validator.Alerts);
        }

        #endregion

        #region Method: Dispose()

        [Fact]
        public void Dispose_DisposesUnitOfWork()
        {
            validator.Dispose();

            unitOfWork.Received().Dispose();
        }

        [Fact]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            validator.Dispose();
            validator.Dispose();
        }

        #endregion
    }
}
