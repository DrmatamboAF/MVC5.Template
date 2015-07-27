using MvcTemplate.Objects;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Objects
{
    public class BaseViewTests
    {
        private BaseView view;

        public BaseViewTests()
        {
            view = Substitute.For<BaseView>();
        }

        #region Property: Id

        [Fact]
        public void Id_AlwaysGetsNotNull()
        {
            view.Id = null;

            Assert.NotNull(view.Id);
        }

        [Fact]
        public void Id_AlwaysGetsUniqueValue()
        {
            String expected = view.Id;
            view.Id = null;
            String actual = view.Id;

            Assert.NotEqual(expected, actual);
        }

        [Fact]
        public void Id_AlwaysGetsSameValue()
        {
            String expected = view.Id;
            String actual = view.Id;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Property: CreationDate

        [Fact]
        public void CreationDate_AlwaysGetsSameValue()
        {
            DateTime expected = view.CreationDate;
            DateTime actual = view.CreationDate;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
