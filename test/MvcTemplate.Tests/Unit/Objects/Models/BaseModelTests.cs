using MvcTemplate.Objects;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Objects
{
    public class BaseModelTests
    {
        private BaseModel model;

        public BaseModelTests()
        {
            model = Substitute.For<BaseModel>();
        }

        #region Property: Id

        [Fact]
        public void Id_AlwaysGetsNotNull()
        {
            model.Id = null;

            Assert.NotNull(model.Id);
        }

        [Fact]
        public void Id_AlwaysGetsUniqueValue()
        {
            String expected = model.Id;
            model.Id = null;
            String actual = model.Id;

            Assert.NotEqual(expected, actual);
        }

        [Fact]
        public void Id_AlwaysGetsSameValue()
        {
            String expected = model.Id;
            String actual = model.Id;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Property: CreationDate

        [Fact]
        public void CreationDate_AlwaysGetsSameValue()
        {
            DateTime expected = model.CreationDate;
            DateTime actual = model.CreationDate;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
