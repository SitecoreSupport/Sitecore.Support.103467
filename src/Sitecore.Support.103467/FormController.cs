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
  public class FormController : SitecoreController
  {
    private readonly IAnalyticsTracker analyticsTracker;

    public IRepository<FormModel> FormRepository
    {
      get;
      private set;
    }

    public IAutoMapper<IFormModel, FormViewModel> Mapper
    {
      get;
      private set;
    }

    public IFormProcessor<FormModel> FormProcessor
    {
      get;
      private set;
    }

    public FormController() : this((IRepository<FormModel>)Factory.CreateObject(Sitecore.Forms.Mvc.Constants.FormRepository, true), (IAutoMapper<IFormModel, FormViewModel>)Factory.CreateObject(Sitecore.Forms.Mvc.Constants.FormAutoMapper, true), (IFormProcessor<FormModel>)Factory.CreateObject(Sitecore.Forms.Mvc.Constants.FormProcessor, true), DependenciesManager.AnalyticsTracker)
    {
    }

    public FormController(IRepository<FormModel> repository, IAutoMapper<IFormModel, FormViewModel> mapper, IFormProcessor<FormModel> processor, IAnalyticsTracker analyticsTracker)
    {
      Assert.ArgumentNotNull(repository, "repository");
      Assert.ArgumentNotNull(mapper, "mapper");
      Assert.ArgumentNotNull(processor, "processor");
      Assert.ArgumentNotNull(analyticsTracker, "analyticsTracker");
      this.FormRepository = repository;
      this.Mapper = mapper;
      this.FormProcessor = processor;
      this.analyticsTracker = analyticsTracker;
    }

    [FormErrorHandler, AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
    public override ActionResult Index()
    {
      return this.Form();
    }

    [FormErrorHandler, SubmittedFormHandler, WffmValidateAntiForgeryToken, HttpPost]
    public virtual ActionResult Index([ModelBinder(typeof(FormModelBinder))] FormViewModel formViewModel)
    {
      this.analyticsTracker.InitializeTracker();
      return this.ProcessedForm(formViewModel, "");
    }

    [FormErrorHandler, AllowCrossSiteJson]
    public virtual JsonResult Process([ModelBinder(typeof(FormModelBinder))] FormViewModel formViewModel)
    {
      this.analyticsTracker.InitializeTracker();
      ProcessedFormResult<FormModel, FormViewModel> processedFormResult = this.ProcessedForm(formViewModel, "~/Views/Form/Index.cshtml");
      processedFormResult.ExecuteResult(base.ControllerContext);
      string data;
      using (StringWriter stringWriter = new StringWriter())
      {
        ViewContext viewContext = new ViewContext(base.ControllerContext, processedFormResult.View, base.ViewData, base.TempData, stringWriter);
        processedFormResult.View.Render(viewContext, stringWriter);
        data = stringWriter.GetStringBuilder().ToString();
      }
      base.ControllerContext.HttpContext.Response.Clear();
      return new JsonResult
      {
        Data = data
      };
    }

    public virtual FormResult<FormModel, FormViewModel> Form()
    {
      return new FormResult<FormModel, FormViewModel>(this.FormRepository, this.Mapper)
      {
        ViewData = base.ViewData,
        TempData = base.TempData,
        ViewEngineCollection = base.ViewEngineCollection
      };
    }

    public virtual ProcessedFormResult<FormModel, FormViewModel> ProcessedForm(FormViewModel viewModel, string viewName = "")
    {
      ProcessedFormResult<FormModel, FormViewModel> processedFormResult = new ProcessedFormResult<FormModel, FormViewModel>(this.FormRepository, this.Mapper, this.FormProcessor, viewModel)
      {
        ViewData = base.ViewData,
        TempData = base.TempData,
        ViewEngineCollection = base.ViewEngineCollection
      };
      if (!string.IsNullOrEmpty(viewName))
      {
        processedFormResult.ViewName = viewName;
      }
      return processedFormResult;
    }
  }
}