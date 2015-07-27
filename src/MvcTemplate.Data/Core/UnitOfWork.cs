using AutoMapper;
using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace MvcTemplate.Data.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private IAuditLogger Logger { get; set; }
        private DbContext Context { get; set; }
        private Boolean Disposed { get; set; }

        public UnitOfWork(DbContext context, IAuditLogger logger = null)
        {
            Context = context;
            Logger = logger;
        }

        public ISelect<TModel> Select<TModel>() where TModel : BaseModel
        {
            return new Select<TModel>(Context.Set<TModel>());
        }

        public TModel To<TModel>(BaseView view) where TModel : BaseModel
        {
            return Mapper.Map<TModel>(view);
        }
        public TView To<TView>(BaseModel model) where TView : BaseView
        {
            return Mapper.Map<TView>(model);
        }

        public TModel Get<TModel>(String id) where TModel : BaseModel
        {
            return Context.Set<TModel>().SingleOrDefault(model => model.Id == id);
        }
        public TView GetAs<TModel, TView>(String id)
            where TModel : BaseModel
            where TView : BaseView
        {
            return To<TView>(Get<TModel>(id));
        }

        public void Insert<TModel>(TModel model) where TModel : BaseModel
        {
            Context.Set<TModel>().Add(model);
        }
        public void Update<TModel>(TModel model) where TModel : BaseModel
        {
            TModel attachedModel = Context.Set<TModel>().Local.SingleOrDefault(localModel => localModel.Id == model.Id);
            if (attachedModel == null)
                attachedModel = Context.Set<TModel>().Attach(model);
            else
                Context.Entry(attachedModel).CurrentValues.SetValues(model);

            DbEntityEntry<TModel> entry = Context.Entry(attachedModel);
            entry.State = EntityState.Modified;
            entry.Property(property => property.CreationDate).IsModified = false;
        }
        public void Delete<TModel>(TModel model) where TModel : BaseModel
        {
            Context.Set<TModel>().Remove(model);
        }
        public void Delete<TModel>(String id) where TModel : BaseModel
        {
            Delete(Context.Set<TModel>().Find(id));
        }

        public void Rollback()
        {
            Context.Dispose();
            Context = Activator.CreateInstance(Context.GetType()) as DbContext;
        }
        public void Commit()
        {
            if (Logger != null)
                Logger.Log(Context.ChangeTracker.Entries<BaseModel>());

            Context.SaveChanges();

            if (Logger != null)
                Logger.Save();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            if (Logger != null) Logger.Dispose();
            Context.Dispose();

            Disposed = true;
        }
    }
}
