using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace MvcTemplate.Data.Logging
{
    public class LoggableEntity
    {
        public String Id { get; private set; }
        public String Name { get; private set; }
        public String Action { get; private set; }

        public IEnumerable<LoggableProperty> Properties { get; private set; }

        public LoggableEntity(DbEntityEntry<BaseModel> entry)
        {
            DbPropertyValues originalValues =
                (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                    ? entry.GetDatabaseValues()
                    : entry.CurrentValues;

            Type entityType = entry.Entity.GetType();
            if (entityType.Namespace == "System.Data.Entity.DynamicProxies") entityType = entityType.BaseType;
            Properties = originalValues.PropertyNames.Select(name => new LoggableProperty(entry.Property(name), originalValues[name]));
            Properties = entry.State == EntityState.Modified ? Properties.Where(property => property.IsModified) : Properties;
            Properties = Properties.ToArray();
            Action = entry.State.ToString();
            Name = entityType.Name;
            Id = entry.Entity.Id;
        }

        public override String ToString()
        {
            StringBuilder changes = new StringBuilder();
            foreach (LoggableProperty property in Properties)
                changes.Append(property + Environment.NewLine);

            return changes.ToString();
        }
    }
}
