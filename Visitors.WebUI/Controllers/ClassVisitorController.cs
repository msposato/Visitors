using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Mvc.Message;
using Visitors.Domain;
using Visitors.WebUI.Models;
using ApplicationException = Elmah.ApplicationException;

namespace Visitors.WebUI.Controllers
{
  public class ClassVisitorController : VisitorsBaseController
  {
    public ClassVisitorController(IVisitorRepository db)
      : base(db)
    {
    }


    // GET: /ClassVisit/Details/5
    public ActionResult Details(Guid id)
    {
      ClassVisitorViewModel viewModel = WebUIHelper.GetClassVisitorViewModel(id, VisitorsDb);

      if (viewModel != null)
      {
        if (TempData[Constants.TempDataKeys.Message] != null)
        {
          ViewBag.Message = TempData[Constants.TempDataKeys.Message];
        }

        return View(viewModel);
      }

      throw new ApplicationException(
        string.Format("Could not find a class visit request with that id. id:{0}", id));
    }


    // GET: /ClassVisit/Create
    public ActionResult Create()
    {
      ClassVisitorViewModel viewModel = new ClassVisitorViewModel();

      // add the academic subjects to the view model
      if (VisitorsDb.Query<AcademicSubject>().Any())
      {
        viewModel.AcademicSubjects =
          VisitorsDb.Query<AcademicSubject>()
                    .Select(a => new CheckBoxItem { Id = a.Id, IsChecked = false, Label = a.Name })
                    .ToList();
      }

      // add the available time slots
      if (VisitorsDb.AvailableTimeSlots.Any())
      {
        viewModel.AvailableTimeSlots =
          VisitorsDb.AvailableTimeSlots.AsEnumerable().OrderBy(t => t.StartDateTime).Select(t => new SelectListItem
          {
            Text = t.Description,
            Value = t.Id.ToString(CultureInfo.CurrentCulture)
          }).ToList();
      }
      else
      {
        ViewBag.Message = "There are no class visits available at this time. Click on the contact link above for more information";
      }

      return View(viewModel);
    }


    // POST: /ClassVisit/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(ClassVisitorViewModel viewModel)
    {
      ClassVisitor visitor = new ClassVisitor
        {
          Id = Guid.NewGuid(),
          FirstName = viewModel.FirstName,
          LastName = viewModel.LastName,
          Email = viewModel.Email,
          Phone = viewModel.Phone,
          PreferredTimeSlotId = viewModel.PreferedTimeSlotId,
          AlternateTimeSlotId = viewModel.AlternateTimeSlotId,
          YearInSchool = viewModel.YearInSchool,
          Address1 = viewModel.Address1,
          Address2 = viewModel.Address2,
          City = viewModel.City,
          Country = viewModel.Country,
          PostalCode = viewModel.PostalCode,
          State = viewModel.State
        };


      foreach (CheckBoxItem checkBoxItem in viewModel.AcademicSubjects.Where(a => a.IsChecked))
      {
        // get the academic subject in a manner that the context is aware of the academic subject
        // so it will not add a new academic subject
        AcademicSubject academicSubject = VisitorsDb.Find<AcademicSubject>(checkBoxItem.Id); 
        visitor.AcademicSubjects.Add(academicSubject);        
      }

      VisitorsDb.Add(visitor);
      VisitorsDb.Save();

      WebUIHelper.UpdateSeatsRemaining(viewModel.PreferedTimeSlotId, VisitorsDb);

      SendWelcomeMessages(visitor);      

      TempData[Constants.TempDataKeys.Message] =
        "Thank you for registering your class visit. You should be receiving a confirmation email shortly. A McIntire Ambassador will be contacting you soon.";

      return RedirectToAction("Details", new { id = visitor.Id });
    }


    // GET: /ClassVisit/Delete/5
    public ActionResult Cancel(Guid id)
    {
      ClassVisitorViewModel viewModel = WebUIHelper.GetClassVisitorViewModel(id, VisitorsDb);

      if (viewModel != null)
      {
        return View(viewModel);
      }

      throw new ApplicationException(
        String.Format("Could not find a class visit request with that id. id:{0}", id));
    }


    // POST: /ClassVisit/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Cancel(Guid id, ClassVisitorViewModel viewModel)
    {
      if (VisitorsDb.Query<ClassVisitor>().Any(v => v.Id == id))
      {
        ClassVisitor visitor = VisitorsDb.Query<ClassVisitor>().First(v => v.Id == id);

        WebUIHelper.SendCancellationMessage(visitor);

        VisitorsDb.Remove(visitor);
        VisitorsDb.Save();

        WebUIHelper.UpdateSeatsRemaining(visitor.PreferredTimeSlotId, VisitorsDb);
      }
      else
      {
        throw new ApplicationException(
          string.Format("Could not find a class visit request with that id. id:{0}", id));
      }

      ViewBag.Message =
        "Your class visit has been cancelled. You should be receiving a cancellation email message shortly.";
      return View("Message");
    }


    private void SendWelcomeMessages(ClassVisitor visitor)
    {
      UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);

      string detailsURL = string.Format("{0}://{1}{2}",
        ControllerContext.RequestContext.HttpContext.Request.Url.Scheme,
        ControllerContext.RequestContext.HttpContext.Request.Url.Authority,
        urlHelper.Action("Details", "ClassVisitor", new { id = visitor.Id }));

      string cancelURL = string.Format("{0}://{1}{2}",
        ControllerContext.RequestContext.HttpContext.Request.Url.Scheme,
        ControllerContext.RequestContext.HttpContext.Request.Url.Authority,
        urlHelper.Action("Cancel", "ClassVisitor", new { id = visitor.Id }));

      string editURL = string.Format("{0}://{1}{2}",
        ControllerContext.RequestContext.HttpContext.Request.Url.Scheme,
        ControllerContext.RequestContext.HttpContext.Request.Url.Authority,
        urlHelper.Action("Edit", "ClassVisitor", new { id = visitor.Id, Area = "Admin" }));

      string adminDetailsURL = string.Format("{0}://{1}{2}",
        ControllerContext.RequestContext.HttpContext.Request.Url.Scheme,
        ControllerContext.RequestContext.HttpContext.Request.Url.Authority,
        urlHelper.Action("Details", "ClassVisitor", new { id = visitor.Id, Area = "Admin" }));

      WebUIHelper.SendWelcomeMessageToVisitor(visitor, detailsURL, cancelURL);

      WebUIHelper.SendRegistrationMessageToAmbassadors(visitor, editURL, adminDetailsURL);
    }
  }
}