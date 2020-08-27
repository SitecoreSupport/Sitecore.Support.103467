using Sitecore.Configuration;
using Sitecore.Forms.Mvc.Controllers.ModelBinders;
using Sitecore.Forms.Mvc.Interfaces;
using Sitecore.Forms.Mvc.Models;
using Sitecore.Forms.Mvc.ViewModels;
using Sitecore.WFFM.Abstractions;
using Sitecore.WFFM.Abstractions.Shared;
using System.Web.Mvc;

namespace Sitecore.Support.Forms.Mvc.Controllers
{
    [ModelBinder(typeof(FormModelBinder))]
    public class FormController : Sitecore.Forms.Mvc.Controllers.FormController
  {
    public FormController()
      : this((IRepository<FormModel>)Factory.CreateObject(Sitecore.Forms.Mvc.Constants.FormRepository, true), (IAutoMapper<FormModel, FormViewModel>)Factory.CreateObject(Sitecore.Forms.Mvc.Constants.FormAutoMapper, assert: true), (IFormProcessor<FormModel>)Factory.CreateObject(Sitecore.Forms.Mvc.Constants.FormProcessor, assert: true), DependenciesManager.AnalyticsTracker)
    {
    }

    public FormController(IRepository<FormModel> repository, IAutoMapper<FormModel, FormViewModel> mapper, IFormProcessor<FormModel> processor, IAnalyticsTracker analyticsTracker)
      : base(repository, mapper, processor, analyticsTracker)
    {
    }

    [HttpHead]
    public ActionResult Index(string none)
    {
      return this.Index();
    }
	}
}