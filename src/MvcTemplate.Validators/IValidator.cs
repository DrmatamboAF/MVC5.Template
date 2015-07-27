using MvcTemplate.Components.Alerts;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Validators
{
    public interface IValidator : IDisposable
    {
        ModelStateDictionary ModelState { get; set; }
        AlertsContainer Alerts { get; set; }
    }
}
