using MvcTemplate.Objects;
using System;

namespace MvcTemplate.Data.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ISelect<TModel> Select<TModel>() where TModel : BaseModel;

        TModel To<TModel>(BaseView view) where TModel : BaseModel;
        TView To<TView>(BaseModel model) where TView : BaseView;

        TModel Get<TModel>(String id) where TModel : BaseModel;
        TView GetAs<TModel, TView>(String id)
            where TModel : BaseModel
            where TView : BaseView;

        void Insert<TModel>(TModel model) where TModel : BaseModel;
        void Update<TModel>(TModel model) where TModel : BaseModel;
        void Delete<TModel>(TModel model) where TModel : BaseModel;
        void Delete<TModel>(String id) where TModel : BaseModel;

        void Rollback();
        void Commit();
    }
}
