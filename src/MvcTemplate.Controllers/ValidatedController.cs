using MvcTemplate.Services;
using MvcTemplate.Validators;
using System;

namespace MvcTemplate.Controllers
{
    public abstract class ValidatedController<TValidator, TService> : ServicedController<TService>
        where TValidator : IValidator
        where TService : IService
    {
        public TValidator Validator { get; private set; }
        private Boolean Disposed { get; set; }

        protected ValidatedController(TValidator validator, TService service)
            : base(service)
        {
            Validator = validator;
            Validator.Alerts = Alerts;
            Validator.ModelState = ModelState;
        }

        protected override void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            Validator.Dispose();
            Disposed = true;

            base.Dispose(disposing);
        }
    }
}
