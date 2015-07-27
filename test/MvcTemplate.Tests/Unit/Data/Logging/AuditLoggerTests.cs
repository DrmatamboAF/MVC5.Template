using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Xunit;
using Xunit.Extensions;

namespace MvcTemplate.Tests.Unit.Data.Logging
{
    public class AuditLoggerTests : IDisposable
    {
        private DbEntityEntry<BaseModel> entry;
        private TestingContext dataContext;
        private AuditLogger logger;
        private DbContext context;

        public AuditLoggerTests()
        {
            context = new TestingContext();
            dataContext = new TestingContext();
            TestModel model = ObjectFactory.CreateTestModel();
            logger = Substitute.ForPartsOf<AuditLogger>(context);

            entry = dataContext.Entry<BaseModel>(dataContext.Set<TestModel>().Add(model));
            dataContext.Set<TestModel>().RemoveRange(dataContext.Set<TestModel>());
            dataContext.SaveChanges();
        }
        public void Dispose()
        {
            HttpContext.Current = null;
            dataContext.Dispose();
            context.Dispose();
            logger.Dispose();
        }

        #region Constructor: AuditLogger(DbContext context)

        [Fact]
        public void AuditLogger_DisablesChangesDetection()
        {
            TestingContext context = new TestingContext();
            context.Configuration.AutoDetectChangesEnabled = true;

            using (AuditLogger logger = new AuditLogger(context))
                Assert.False(context.Configuration.AutoDetectChangesEnabled);
        }

        #endregion

        #region Constructor: AuditLogger(DbContext context, String accountId)

        [Fact]
        public void AuditLogger_AccountId_DisablesChangesDetection()
        {
            TestingContext context = new TestingContext();
            context.Configuration.AutoDetectChangesEnabled = true;

            using (AuditLogger logger = new AuditLogger(context, "Test"))
                Assert.False(context.Configuration.AutoDetectChangesEnabled);
        }

        #endregion

        #region Method: Log(IEnumerable<DbEntityEntry<BaseModel>> entries)

        [Fact]
        public void Log_LogsAddedEntities()
        {
            entry.State = EntityState.Added;

            Logs(entry);
        }

        [Fact]
        public void Log_LogsModifiedEntities()
        {
            (entry.Entity as TestModel).Text += "Test";
            entry.State = EntityState.Modified;

            Logs(entry);
        }

        [Fact]
        public void Log_DoesNotLogModifiedEntitiesWithoutChanges()
        {
            entry.State = EntityState.Modified;

            logger.Log(new[] { entry });

            logger.DidNotReceiveWithAnyArgs().Log((LoggableEntity)null);
        }

        [Fact]
        public void Log_LogsDeletedEntities()
        {
            entry.State = EntityState.Deleted;

            Logs(entry);
        }

        [Fact]
        public void Log_DoesNotLogUnsupportedEntityStates()
        {
            IEnumerable<EntityState> unsupportedStates = Enum
                .GetValues(typeof(EntityState))
                .Cast<EntityState>()
                .Where(state =>
                    state != EntityState.Added &&
                    state != EntityState.Modified &&
                    state != EntityState.Deleted);

            foreach (EntityState usupportedState in unsupportedStates)
            {
                entry.State = usupportedState;
                logger.Log(new[] { entry });
            }

            logger.DidNotReceiveWithAnyArgs().Log((LoggableEntity)null);
        }

        [Fact]
        public void Log_DoesNotSaveLogs()
        {
            entry.State = EntityState.Added;
            HttpContext.Current = HttpContextFactory.CreateHttpContext();

            logger.Log(new[] { entry });

            Assert.Empty(context.Set<AuditLog>());
        }

        #endregion

        #region Method: Log(LoggableEntity entity)

        [Theory]
        [InlineData("", "", null)]
        [InlineData(null, "", null)]
        [InlineData("", null, null)]
        [InlineData(null, null, null)]
        [InlineData("", "IdentityId", null)]
        [InlineData("AccountId", "", "AccountId")]
        [InlineData("AccountId", null, "AccountId")]
        [InlineData(null, "IdentityId", "IdentityId")]
        [InlineData("AccountId", "IdentityId", "AccountId")]
        public void Log_AddsLogToTheSet(String accountId, String identityName, String expectedAccountId)
        {
            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            HttpContext.Current.User.Identity.Name.Returns(identityName);
            LoggableEntity entity = new LoggableEntity(entry);
            logger = new AuditLogger(context, accountId);

            logger.Log(entity);

            AuditLog actual = context.ChangeTracker.Entries<AuditLog>().First().Entity;
            LoggableEntity expected = entity;

            Assert.Equal(expectedAccountId, actual.AccountId);
            Assert.Equal(expected.ToString(), actual.Changes);
            Assert.Equal(expected.Name, actual.EntityName);
            Assert.Equal(expected.Action, actual.Action);
            Assert.Equal(expected.Id, actual.EntityId);
        }

        [Fact]
        public void Log_DoesNotSaveLog()
        {
            entry.State = EntityState.Added;
            LoggableEntity entity = new LoggableEntity(entry);
            HttpContext.Current = HttpContextFactory.CreateHttpContext();

            logger.Log(entity);

            Assert.Empty(context.Set<AuditLog>());
        }

        #endregion

        #region Method: Save()

        [Fact]
        public void Save_SavesLogs()
        {
            DbContext context = Substitute.For<DbContext>();
            logger = Substitute.ForPartsOf<AuditLogger>(context);

            logger.Save();

            context.Received().SaveChanges();
        }

        #endregion

        #region Method: Dispose()

        [Fact]
        public void Dispose_DisposesContext()
        {
            DbContext context = Substitute.For<DbContext>();
            logger = Substitute.ForPartsOf<AuditLogger>(context);

            logger.Dispose();

            context.Received().Dispose();
        }

        [Fact]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            logger.Dispose();
            logger.Dispose();
        }

        #endregion

        #region Test helpers

        private void Logs(DbEntityEntry<BaseModel> entry)
        {
            LoggableEntity expected = new LoggableEntity(entry);
            logger.When(sub => sub.Log(Arg.Any<LoggableEntity>())).DoNotCallBase();
            logger.When(sub => sub.Log(Arg.Any<LoggableEntity>())).Do(info =>
            {
                LoggableEntity actual = info.Arg<LoggableEntity>();

                Assert.Equal(expected.ToString(), actual.ToString());
                Assert.Equal(expected.Action, actual.Action);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Id, actual.Id);
            });

            logger.Log(new[] { entry });

            logger.ReceivedWithAnyArgs().Log(expected);
        }

        #endregion
    }
}
