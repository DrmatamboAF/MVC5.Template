using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Table;
using MvcTemplate.Tests.Objects;
using NonFactors.Mvc.Grid;
using NSubstitute;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class MvcGridExtensionsTests : IDisposable
    {
        private IGridColumns<AllTypesView> columns;
        private IGridColumn<AllTypesView> column;
        private IHtmlGrid<AllTypesView> htmlGrid;

        public MvcGridExtensionsTests()
        {
            column = SubstituteColumn<AllTypesView>();
            htmlGrid = SubstituteHtmlGrid<AllTypesView>();
            columns = SubstituteColumns<AllTypesView, DateTime?>(column);
        }
        public void Dispose()
        {
            Authorization.Provider = null;
        }

        #region Extension method: AddActionLink<T>(this IGridColumns<T> columns, String action, String iconClass)

        [Fact]
        public void AddActionLink_OnUnauthorizedActionLinkDoesNotAddColumn()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            columns.Grid.HttpContext = HttpContextFactory.CreateHttpContextBase();

            columns.AddActionLink("Edit", "fa fa-pencil");

            columns.DidNotReceive().Add(Arg.Any<Expression<Func<AllTypesView, String>>>());
            columns.DidNotReceive().Insert(Arg.Any<Int32>(), Arg.Any<Expression<Func<AllTypesView, String>>>());
        }

        [Fact]
        public void AddActionLink_OnUnauthorizedActionLinkReturnsGridColumn()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            columns.Grid.HttpContext = HttpContextFactory.CreateHttpContextBase();

            IGridColumn<AllTypesView> column = columns.AddActionLink("Edit", "fa fa-pencil");

            Assert.IsType<GridColumn<AllTypesView, String>>(column);
            Assert.NotNull(column);
        }

        [Fact]
        public void AddActionLink_RendersAuthorizedActionLink()
        {
            String actionLink = "";
            AllTypesView view = new AllTypesView();
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            columns.Grid.HttpContext = HttpContextFactory.CreateHttpContextBase();
            UrlHelper urlHelper = new UrlHelper(columns.Grid.HttpContext.Request.RequestContext);
            Authorization.Provider.IsAuthorizedFor(Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>(), "Details").Returns(true);

            columns
                .Add(Arg.Any<Expression<Func<AllTypesView, String>>>())
                .Returns(column)
                .AndDoes(info =>
                {
                    actionLink = info.Arg<Expression<Func<AllTypesView, String>>>().Compile().Invoke(view);
                });

            columns.AddActionLink("Details", "fa fa-info");

            String actual = actionLink;
            String expected = String.Format(
                "<a class=\"details-action\" href=\"{0}\">" +
                    "<i class=\"fa fa-info\"></i>" +
                "</a>",
                urlHelper.Action("Details", new { id = view.Id }));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddActionLink_OnNullAuthorizationProviderRendersActionLink()
        {
            String actionLink = "";
            Authorization.Provider = null;
            AllTypesView view = new AllTypesView();
            columns.Grid.HttpContext = HttpContextFactory.CreateHttpContextBase();
            UrlHelper urlHelper = new UrlHelper(columns.Grid.HttpContext.Request.RequestContext);

            columns
                .Add(Arg.Any<Expression<Func<AllTypesView, String>>>()).Returns(column)
                .AndDoes(info => { actionLink = info.Arg<Expression<Func<AllTypesView, String>>>().Compile().Invoke(view); });

            columns.AddActionLink("Details", "fa fa-info");

            String actual = actionLink;
            String expected = String.Format(
                "<a class=\"details-action\" href=\"{0}\">" +
                    "<i class=\"fa fa-info\"></i>" +
                "</a>",
                urlHelper.Action("Details", new { id = view.Id }));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddActionLink_OnModelWihoutKeyPropertyThrows()
        {
            Func<Object, String> renderer = null;
            IGridColumn<Object> column = SubstituteColumn<Object>();
            IGridColumns<Object> columns = SubstituteColumns<Object, String>(column);

            columns
                .Add(Arg.Any<Expression<Func<Object, String>>>())
                .Returns(column)
                .AndDoes(info =>
                {
                    renderer = info.Arg<Expression<Func<Object, String>>>().Compile();
                });

            columns.AddActionLink("Delete", "fa fa-times");

            String actual = Assert.Throws<Exception>(() => renderer.Invoke(new Object())).Message;
            String expected = "Object type does not have a key property.";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddActionLink_SetsCssOnGridColumn()
        {
            columns.AddActionLink("Edit", "fa fa-pencil");

            column.Received().Css("action-cell");
        }

        [Fact]
        public void AddActionLink_DoesNotEncodeGridColumn()
        {
            columns.AddActionLink("Edit", "fa fa-pencil");

            column.Received().Encoded(false);
        }

        #endregion

        #region Extension method: AddDateProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime>> property)

        [Fact]
        public void AddDateProperty_AddsGridColumn()
        {
            Expression<Func<AllTypesView, DateTime?>> expression = (model) => model.DateTimeField;

            columns.AddDateProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddDateProperty_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.DateTimeField);

            columns.AddDateProperty(model => model.DateTimeField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddDateProperty_SetsGridColumnCss()
        {
            columns.AddDateProperty(model => model.DateTimeField);

            column.Received().Css("text-center");
        }

        [Fact]
        public void AddDateProperty_FormatsGridColumn()
        {
            columns.AddDateProperty(model => model.DateTimeField);

            column.Received().Formatted("{0:d}");
        }

        #endregion

        #region Extension method: AddDateProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime?>> property)

        [Fact]
        public void AddDateProperty_Nullable_AddsGridColumn()
        {
            Expression<Func<AllTypesView, DateTime?>> expression = (model) => model.NullableDateTimeField;

            columns.AddDateProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddDateProperty_Nullable_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.NullableDateTimeField);

            columns.AddDateProperty(model => model.NullableDateTimeField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddDateProperty_Nullable_SetsGridColumnCss()
        {
            columns.AddDateProperty(model => model.NullableDateTimeField);

            column.Received().Css("text-center");
        }

        [Fact]
        public void AddDateProperty_Nullable_FormatsGridColumn()
        {
            columns.AddDateProperty(model => model.NullableDateTimeField);

            column.Received().Formatted("{0:d}");
        }

        #endregion

        #region Extension method: AddBooleanProperty<T>(this IGridColumns<T> columns, Expression<Func<T, Boolean>> property)

        [Fact]
        public void AddBooleanProperty_AddsGridColumn()
        {
            Expression<Func<AllTypesView, Boolean>> expression = (model) => model.BooleanField;

            columns.AddBooleanProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddBooleanProperty_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, Boolean>(model => model.BooleanField);

            columns.AddBooleanProperty(model => model.BooleanField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddBooleanProperty_SetsGridColumnCss()
        {
            columns.AddBooleanProperty(model => model.BooleanField);

            column.Received().Css("text-left");
        }

        [Fact]
        public void AddBooleanProperty_RendersBooleanTrueValue()
        {
            Object renderedValue = null;
            AllTypesView view = new AllTypesView { BooleanField = true };

            column
                .When(sub => sub.RenderedAs(Arg.Any<Func<AllTypesView, Object>>()))
                .Do(info => renderedValue = info.Arg<Func<AllTypesView, Object>>()(view));

            columns.AddBooleanProperty(model => model.BooleanField);

            Object expected = TableResources.Yes;
            Object actual = renderedValue;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddBooleanProperty_RendersBooleanFalseValue()
        {
            Object renderedValue = null;
            AllTypesView view = new AllTypesView { BooleanField = false };

            column
                .When(sub => sub.RenderedAs(Arg.Any<Func<AllTypesView, Object>>()))
                .Do(info => renderedValue = info.Arg<Func<AllTypesView, Object>>()(view));

            columns.AddBooleanProperty(model => model.BooleanField);

            Object expected = TableResources.No;
            Object actual = renderedValue;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: AddBooleanProperty<T>(this IGridColumns<T> columns, Expression<Func<T, Boolean?>> property)

        [Fact]
        public void AddBooleanProperty_Nullable_AddsGridColumn()
        {
            Expression<Func<AllTypesView, Boolean?>> expression = (model) => model.NullableBooleanField;

            columns.AddBooleanProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddBooleanProperty_Nullable_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, Boolean?>(model => model.NullableBooleanField);

            columns.AddBooleanProperty(model => model.NullableBooleanField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddBooleanProperty_Nullable_SetsGridColumnCss()
        {
            columns.AddBooleanProperty(model => model.NullableBooleanField);

            column.Received().Css("text-left");
        }

        [Fact]
        public void AddBooleanProperty_Nullable_RendersBooleanTrueValue()
        {
            Object renderedValue = null;
            AllTypesView view = new AllTypesView { NullableBooleanField = true };

            column
                .When(sub => sub.RenderedAs(Arg.Any<Func<AllTypesView, Object>>()))
                .Do(info => renderedValue = info.Arg<Func<AllTypesView, Object>>()(view));

            columns.AddBooleanProperty(model => model.NullableBooleanField);

            Object expected = TableResources.Yes;
            Object actual = renderedValue;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddBooleanProperty_Nullable_RendersBooleanFalseValue()
        {
            Object renderedValue = null;
            AllTypesView view = new AllTypesView { NullableBooleanField = false };

            column
                .When(sub => sub.RenderedAs(Arg.Any<Func<AllTypesView, Object>>()))
                .Do(info => renderedValue = info.Arg<Func<AllTypesView, Object>>()(view));

            columns.AddBooleanProperty(model => model.NullableBooleanField);

            Object expected = TableResources.No;
            Object actual = renderedValue;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddBooleanProperty_Nullable_RendersBooleanNullValue()
        {
            Object renderedValue = null;
            AllTypesView view = new AllTypesView { NullableBooleanField = null };

            column
                .When(sub => sub.RenderedAs(Arg.Any<Func<AllTypesView, Object>>()))
                .Do(info => renderedValue = info.Arg<Func<AllTypesView, Object>>()(view));

            columns.AddBooleanProperty(model => model.NullableBooleanField);

            Object expected = "";
            Object actual = renderedValue;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: AddDateTimeProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime>> property)

        [Fact]
        public void AddDateTimeProperty_AddsGridColumn()
        {
            Expression<Func<AllTypesView, DateTime?>> expression = (model) => model.DateTimeField;

            columns.AddDateTimeProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddDateTimeProperty_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.DateTimeField);

            columns.AddDateTimeProperty(model => model.DateTimeField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddDateTimeProperty_SetsGridColumnCss()
        {
            columns.AddDateTimeProperty(model => model.DateTimeField);

            column.Received().Css("text-center");
        }

        [Fact]
        public void AddDateTimeProperty_FormatsGridColumn()
        {
            columns.AddDateTimeProperty(model => model.DateTimeField);

            column.Received().Formatted("{0:g}");
        }

        #endregion

        #region Extension method: AddDateTimeProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime?>> property)

        [Fact]
        public void AddDateTimeProperty_Nullable_AddsGridColumn()
        {
            Expression<Func<AllTypesView, DateTime?>> expression = (model) => model.NullableDateTimeField;

            columns.AddDateTimeProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddDateTimeProperty_Nullable_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.NullableDateTimeField);

            columns.AddDateTimeProperty(model => model.NullableDateTimeField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddDateTimeProperty_Nullable_SetsGridColumnCss()
        {
            columns.AddDateTimeProperty(model => model.NullableDateTimeField);

            column.Received().Css("text-center");
        }

        [Fact]
        public void AddDateTimeProperty_Nullable_FormatsGridColumn()
        {
            columns.AddDateTimeProperty(model => model.NullableDateTimeField);

            column.Received().Formatted("{0:g}");
        }

        #endregion

        #region Extension method: AddProperty<T, TProperty>(this IGridColumns<T> columns, Expression<Func<T, TProperty>> property)

        [Fact]
        public void AddProperty_AddsGridColumn()
        {
            Expression<Func<AllTypesView, String>> expression = (model) => model.Id;

            columns.AddProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddProperty_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.NullableDateTimeField);

            columns.AddProperty(model => model.NullableDateTimeField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddProperty_SetsCssClassForEnum()
        {
            AssertCssClassFor(model => model.EnumField, "text-left");
        }

        [Fact]
        public void AddProperty_SetsCssClassForSByte()
        {
            AssertCssClassFor(model => model.SByteField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForByte()
        {
            AssertCssClassFor(model => model.ByteField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForInt16()
        {
            AssertCssClassFor(model => model.Int16Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForUInt16()
        {
            AssertCssClassFor(model => model.UInt16Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForInt32()
        {
            AssertCssClassFor(model => model.Int32Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForUInt32()
        {
            AssertCssClassFor(model => model.UInt32Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForInt64()
        {
            AssertCssClassFor(model => model.Int64Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForUInt64()
        {
            AssertCssClassFor(model => model.UInt64Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForSingle()
        {
            AssertCssClassFor(model => model.SingleField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForDouble()
        {
            AssertCssClassFor(model => model.DoubleField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForDecimal()
        {
            AssertCssClassFor(model => model.DecimalField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForDateTime()
        {
            AssertCssClassFor(model => model.DateTimeField, "text-center");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableEnum()
        {
            AssertCssClassFor(model => model.NullableEnumField, "text-left");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableSByte()
        {
            AssertCssClassFor(model => model.NullableSByteField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableByte()
        {
            AssertCssClassFor(model => model.NullableByteField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableInt16()
        {
            AssertCssClassFor(model => model.NullableInt16Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableUInt16()
        {
            AssertCssClassFor(model => model.NullableUInt16Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableInt32()
        {
            AssertCssClassFor(model => model.NullableInt32Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableUInt32()
        {
            AssertCssClassFor(model => model.NullableUInt32Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableInt64()
        {
            AssertCssClassFor(model => model.NullableInt64Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableUInt64()
        {
            AssertCssClassFor(model => model.NullableUInt64Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableSingle()
        {
            AssertCssClassFor(model => model.NullableSingleField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableDouble()
        {
            AssertCssClassFor(model => model.NullableDoubleField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableDecimal()
        {
            AssertCssClassFor(model => model.NullableDecimalField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableDateTime()
        {
            AssertCssClassFor(model => model.NullableDateTimeField, "text-center");
        }

        [Fact]
        public void AddProperty_SetsCssClassForOtherTypes()
        {
            AssertCssClassFor(model => model.StringField, "text-left");
        }

        #endregion

        #region Extension method: ApplyDefaults<T>(this IHtmlGrid<T> grid)

        [Fact]
        public void ApplyDefaults_SetsRowsPerPage()
        {
            IGridPager<AllTypesView> pager = Substitute.For<IGridPager<AllTypesView>>();

            htmlGrid
                .When(sub => sub.Pageable(Arg.Any<Action<IGridPager<AllTypesView>>>()))
                .Do(info => info.Arg<Action<IGridPager<AllTypesView>>>()(pager));

            htmlGrid.ApplyDefaults();

            Int32 expected = pager.RowsPerPage;
            Int32 actual = 16;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ApplyDefaults_SetsNameByReplacingViewToEmpty()
        {
            htmlGrid.ApplyDefaults();

            htmlGrid.Received().Named("AllTypes");
        }

        [Fact]
        public void ApplyDefaults_SetsEmptyText()
        {
            htmlGrid.ApplyDefaults();

            htmlGrid.Received().Empty(TableResources.NoDataFound);
        }

        [Fact]
        public void ApplyDefaults_SetsCss()
        {
            htmlGrid.ApplyDefaults();

            htmlGrid.Received().Css("table-hover");
        }

        [Fact]
        public void ApplyDefaults_EnablesFiltering()
        {
            htmlGrid.ApplyDefaults();

            htmlGrid.Received().Filterable();
        }

        [Fact]
        public void ApplyDefaults_EnablesSorting()
        {
            htmlGrid.ApplyDefaults();

            htmlGrid.Received().Sortable();
        }

        #endregion

        #region Test helpers

        private IHtmlGrid<TModel> SubstituteHtmlGrid<TModel>()
        {
            IHtmlGrid<TModel> grid = Substitute.For<IHtmlGrid<TModel>>();
            grid.Pageable(Arg.Any<Action<IGridPager<TModel>>>()).Returns(grid);
            grid.Empty(Arg.Any<String>()).Returns(grid);
            grid.Named(Arg.Any<String>()).Returns(grid);
            grid.Css(Arg.Any<String>()).Returns(grid);
            grid.Filterable().Returns(grid);
            grid.Sortable().Returns(grid);

            return grid;
        }
        private IGridColumn<TModel> SubstituteColumn<TModel>()
        {
            IGridColumn<TModel> column = Substitute.For<IGridColumn<TModel>>();
            column.RenderedAs(Arg.Any<Func<TModel, Object>>()).Returns(column);
            column.Formatted(Arg.Any<String>()).Returns(column);
            column.Encoded(Arg.Any<Boolean>()).Returns(column);
            column.Titled(Arg.Any<String>()).Returns(column);
            column.Css(Arg.Any<String>()).Returns(column);

            return column;
        }
        private IGridColumns<TModel> SubstituteColumns<TModel, TProperty>(IGridColumn<TModel> column)
        {
            IGridColumns<TModel> columns = Substitute.For<IGridColumns<TModel>>();
            columns.Add(Arg.Any<Expression<Func<TModel, String>>>()).Returns(column);
            columns.Add(Arg.Any<Expression<Func<TModel, Boolean>>>()).Returns(column);
            columns.Add(Arg.Any<Expression<Func<TModel, Boolean?>>>()).Returns(column);
            columns.Add(Arg.Any<Expression<Func<TModel, DateTime>>>()).Returns(column);
            columns.Add(Arg.Any<Expression<Func<TModel, DateTime?>>>()).Returns(column);
            columns.Add(Arg.Any<Expression<Func<TModel, TProperty>>>()).Returns(column);

            return columns;
        }

        private void AssertCssClassFor<TProperty>(Expression<Func<AllTypesView, TProperty>> property, String expected)
        {
            columns = SubstituteColumns<AllTypesView, TProperty>(column);
            columns.AddProperty(property);

            column.Received().Css(expected);
        }

        #endregion
    }
}
