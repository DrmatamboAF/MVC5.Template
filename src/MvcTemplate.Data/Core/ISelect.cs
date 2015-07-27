using MvcTemplate.Objects;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MvcTemplate.Data.Core
{
    public interface ISelect<TModel> : IQueryable<TModel> where TModel : BaseModel
    {
        ISelect<TModel> Where(Expression<Func<TModel, Boolean>> predicate);

        IQueryable<TView> To<TView>() where TView : BaseView;
    }
}
