using Datalist;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MvcTemplate.Components.Datalists
{
    public class BaseDatalist<TModel, TView> : GenericDatalist<TView>
        where TModel : BaseModel
        where TView : BaseView
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        public BaseDatalist(UrlHelper url)
        {
            String view = typeof(TView).Name.Replace("View", "");
            DialogTitle = ResourceProvider.GetDatalistTitle(view);
            DatalistUrl = url.Action(view, Prefix, new { area = "" });
        }
        public BaseDatalist(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        protected override String GetColumnHeader(PropertyInfo property)
        {
            DatalistColumnAttribute column = property.GetCustomAttribute<DatalistColumnAttribute>(false);
            if (column != null && column.Relation != null)
                return GetColumnHeader(property.PropertyType.GetProperty(column.Relation));

            return ResourceProvider.GetPropertyTitle(typeof(TView), property.Name) ?? "";
        }
        protected override String GetColumnCssClass(PropertyInfo property)
        {
            Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (type.IsEnum) return "text-left";

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "text-right";
                case TypeCode.DateTime:
                    return "text-center";
                default:
                    return "text-left";
            }
        }

        protected override IQueryable<TView> GetModels()
        {
            return UnitOfWork.Select<TModel>().To<TView>();
        }

        protected override IQueryable<TView> FilterById(IQueryable<TView> models)
        {
            return models.Where(model => model.Id == CurrentFilter.Id);
        }
    }
}
