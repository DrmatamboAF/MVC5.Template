using MvcTemplate.Data.Core;
using MvcTemplate.Tests.Data.Mapping;
using MvcTemplate.Tests.Objects;
using System.Data.Entity;

namespace MvcTemplate.Tests.Data
{
    public class TestingContext : Context
    {
        #region Test

        protected DbSet<TestModel> TestModels { get; set; }

        #endregion

        static TestingContext()
        {
            TestObjectMapper.MapObjects();
        }
    }
}
