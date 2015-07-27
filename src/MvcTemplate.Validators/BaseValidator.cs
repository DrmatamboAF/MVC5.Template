using MvcTemplate.Components.Alerts;
using MvcTemplate.Data.Core;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Validators
{
    public abstract class BaseValidator : IValidator
    {
        protected IUnitOfWork UnitOfWork { get; private set; }
        public ModelStateDictionary ModelState { get; set; }
        public AlertsContainer Alerts { get; set; }
        private Boolean Disposed { get; set; }

        protected BaseValidator(IUnitOfWork unitOfWork)
        {
            ModelState = new ModelStateDictionary();
            Alerts = new AlertsContainer();
            UnitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            UnitOfWork.Dispose();

            Disposed = true;
        }
    }
}
