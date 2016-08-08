using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Visitors.Domain;
using Visitors.WebUI.Admin.Models;
using Visitors.WebUI.Models;

namespace Visitors.WebUI.Admin.Controllers
{
  public class ClassVisitorController : AdminBaseController
  {
    public ClassVisitorController(IVisitorRepository visitorDb)
      : base(visitorDb)
    { }

    // GET: /Admin/Visit/
    public ActionResult Index()
    {
      ListClassVisitorsViewModel viewModel = GetClassVisitorLists(WebUIHelper.CurrentAcademicYearBegin.Year);

      if (TempData[Constants.TempDataKeys.Message] != null)
      {
        ViewBag.Message = TempData[Constants.TempDataKeys.Message];
      }

      return View(viewModel);
    }


    // GET: /Admin/ClassVisitor/ChangeYear
    [HttpPost]
    public ActionResult ChangeYear(int year)
    {
      ListClassVisitorsViewModel viewModel = GetClassVisitorLists(year);
      return View("Index", viewModel);
    }


    // GET: /Admin/Visit/Details/5
    public ActionResult Details(Guid id)
    {
      ClassVisitorViewModel viewModel = WebUIHelper.GetClassVisitorViewModel(id, VisitorsDb);

      if (viewModel != null)
      {
        if (Request.IsAjaxRequest())
        {
          return PartialView("_ClassVisitorDetails", viewModel);
        }

        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find a class visit request with that id. id:{0}", id));
    }


    // GET: /Admin/Visit/Edit/5
    public ActionResult Edit(Guid id)
    {
      ClassVisitorViewModel viewModel = WebUIHelper.GetClassVisitorViewModel(id, VisitorsDb);

      if (viewModel != null)
      {
        // add a list of ambassadors
        if (VisitorsDb.Query<Ambassador>().Any(a => a.Active))
        {
          viewModel.Ambassadors = VisitorsDb.Query<Ambassador>().Where(a => a.Active).AsEnumerable().Select(a => new SelectListItem { Value = a.Id.ToString(CultureInfo.CurrentCulture), Text = a.FullName });
        }

        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find a class visit request with that id. id:{0}", id));
    }


    // POST: /Admin/Visit/Edit/5
    [HttpPost]
    public ActionResult Edit(Guid id, ClassVisitorViewModel viewModel)
    {
      if (VisitorsDb.Query<ClassVisitor>().Any(v => v.Id == id))
      {
        ClassVisitor visitor = VisitorsDb.Query<ClassVisitor>().First(v => v.Id == id);

        bool ambassadorCancelled = ((visitor.Ambassador != null) && (!viewModel.AmbassadorId.HasValue));

        visitor.PreferredTimeSlotId = viewModel.PreferedTimeSlotId;
        visitor.AlternateTimeSlotId = viewModel.AlternateTimeSlotId;
        visitor.AmbassadorId = viewModel.AmbassadorId;

        VisitorsDb.Update(visitor);
        VisitorsDb.Save();

        WebUIHelper.UpdateSeatsRemaining(viewModel.PreferedTimeSlotId, VisitorsDb);

        if (ambassadorCancelled)
        {
          string adminDetailsURL = GetAdminDetailsURL(visitor);
          WebUIHelper.SendAmbassadorCancelledMessage(visitor, adminDetailsURL);
        }
      }

      return RedirectToAction("Index");
    }


    // GET: /Admin/Visit/Delete/5
    public ActionResult Delete(Guid id)
    {
      ClassVisitorViewModel viewModel = WebUIHelper.GetClassVisitorViewModel(id, VisitorsDb);

      if (viewModel != null)
      {
        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find a class visit request with that id. id:{0}", id));
    }


    // POST: /Admin/Visit/Delete/5
    [HttpPost]
    public ActionResult Delete(Guid id, ClassVisitorViewModel viewModel)
    {
      if (VisitorsDb.Query<ClassVisitor>().Any(v => v.Id == id))
      {
        ClassVisitor visitor = VisitorsDb.Query<ClassVisitor>().First(v => v.Id == id);
        TempData[Constants.TempDataKeys.Message] = String.Format("The class visit for {0} has been cancelled.", visitor.FullName);

        WebUIHelper.SendCancellationMessage(visitor);

        VisitorsDb.Remove(visitor);
        VisitorsDb.Save();

        WebUIHelper.UpdateSeatsRemaining(visitor.PreferredTimeSlotId, VisitorsDb);
      }
      else
      {
        throw new Elmah.ApplicationException(string.Format("We could not find a class visit request with that id. id:{0}", id));
      }

      return RedirectToAction("Index");
    }


    private ListClassVisitorsViewModel GetClassVisitorLists(int year)
    {
      DateTime academicYearBegin = WebUIHelper.GetAcademicYearBegin(year);
      DateTime academicYearEnd = WebUIHelper.GetAcademicYearEnd(year);

      ListClassVisitorsViewModel viewModel = new ListClassVisitorsViewModel
        {
          Year = year,
          BeginDate = academicYearBegin,
          EndDate = academicYearEnd
        };

      if (VisitorsDb.Query<ClassVisitor>().Any())
      {
        viewModel.UpcomingVisits = VisitorsDb.UpcomingVisitors.Select(v => new ClassVisitorViewModel
        {
          FirstName = v.FirstName,
          LastName = v.LastName,
          Email = v.Email,
          Id = v.Id,
          Address1 = v.Address1,
          Address2 = v.Address2,
          PreferedTimeSlot = v.PreferredTimeSlot,
          City = v.City,
          AlternateTimeSlot = v.AlternateTimeSlot,
          YearInSchool = v.YearInSchool,
          State = v.State,
          Phone = v.Phone,
          PostalCode = v.PostalCode,
          Ambassador = v.Ambassador,
          Country = v.Country
        }).OrderBy(v => v.PreferedTimeSlot.StartDateTime).ToList();
      }


      if (VisitorsDb.Query<ClassVisitor>().Any())
      {
        var pastVisits = VisitorsDb.Query<ClassVisitor>().Where(v => (v.PreferredTimeSlot.StartDateTime > academicYearBegin) &&
          (v.PreferredTimeSlot.StartDateTime < academicYearEnd));

        if (pastVisits.Any())
        {
          viewModel.PastVisits = pastVisits.Select(v => new ClassVisitorViewModel
          {
            FirstName = v.FirstName,
            LastName = v.LastName,
            Email = v.Email,
            Id = v.Id,
            Address1 = v.Address1,
            Address2 = v.Address2,
            PreferedTimeSlot = v.PreferredTimeSlot,
            City = v.City,
            AlternateTimeSlot = v.AlternateTimeSlot,
            YearInSchool = v.YearInSchool,
            State = v.State,
            Phone = v.Phone,
            PostalCode = v.PostalCode,
            Ambassador = v.Ambassador,
            Country = v.Country
          }).OrderByDescending(v => v.PreferedTimeSlot.StartDateTime).ToList();
        }
      }

      return viewModel;
    }


    private string GetAdminDetailsURL(ClassVisitor visitor)
    {
      UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);

      string adminDetailsURL = string.Format("{0}://{1}{2}",
        ControllerContext.RequestContext.HttpContext.Request.Url.Scheme,
        ControllerContext.RequestContext.HttpContext.Request.Url.Authority,
        urlHelper.Action("Details", "ClassVisitor", new { id = visitor.Id, Area = "Admin" }));

      return adminDetailsURL;
    }
  }
}
