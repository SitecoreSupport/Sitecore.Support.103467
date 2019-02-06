using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Forms.Mvc.Attributes;
using Sitecore.Forms.Mvc.Controllers.Filters;
using Sitecore.Forms.Mvc.Controllers.ModelBinders;
using Sitecore.Forms.Mvc.Interfaces;
using Sitecore.Forms.Mvc.Models;
using Sitecore.Forms.Mvc.ViewModels;
using Sitecore.Mvc.Controllers;
using Sitecore.WFFM.Abstractions.Dependencies;
using Sitecore.WFFM.Abstractions.Shared;
using System.IO;
using System.Web.Mvc;
using Sitecore.Forms.Mvc.Controllers;

namespace Sitecore.Support.Forms.Mvc.Controllers
{
  [ModelBinder(typeof(FormModelBinder))]
  public class FormController : Sitecore.Forms.Mvc.Controllers.FormController
  {
   

    public FormController() : this((IRepository<FormModel>)Factory.CreateObject(Sitecore.Forms.Mvc.Constants.FormRepository, true), (IAutoMapper<IFormModel, FormViewModel>)Factory.CreateObject(Sitecore.Forms.Mvc.Constants.FormAutoMapper, true), (IFormProcessor<FormModel>)Factory.CreateObject(Sitecore.Forms.Mvc.Constants.FormProcessor, true), DependenciesManager.AnalyticsTracker)
    {
    }

    public FormController(IRepository<FormModel> repository, IAutoMapper<IFormModel, FormViewModel> mapper, IFormProcessor<FormModel> processor, IAnalyticsTracker analyticsTracker):
      base(repository,mapper,processor,analyticsTracker)
    {
      
    }   
    
    [HttpHead]
    public ActionResult Index(string none)
    {
      return base.Index();
    }
  }
}