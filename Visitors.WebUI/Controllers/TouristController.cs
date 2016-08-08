using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Visitors.Domain;
using Visitors.WebUI.Models;

namespace Visitors.WebUI.Controllers
{
  public class TouristController : VisitorsBaseController
  {
    public TouristController(IVisitorRepository dB)
      : base(dB)
    {
    }

    // GET: /Tourist/Details/5
    public ActionResult Details(Guid id)
    {
      TouristViewModel viewModel = GetTouristViewModel(id);

      if (viewModel != null)
      {
        if (TempData[Constants.TempDataKeys.Message] != null)
        {
          ViewBag.Message = TempData[Constants.TempDataKeys.Message];
        }

        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find a tourist with that id. id:{0}", id));
    }


    // GET: /Tourist/Create
    public ActionResult Create()
    {
      TouristViewModel viewModel = new TouristViewModel();

      if (VisitorsDb.UpcomingTours.Any())
      {
        viewModel.AvailableTours =
          VisitorsDb.UpcomingTours.ToList().Select(
            t =>
            new SelectListItem { Text = t.StartTime.ToString("F"), Value = t.Id.ToString(CultureInfo.CurrentCulture) }).ToList();
      }
      else
      {
        ViewBag.Message = "There are no tours scheduled at this time. Click on the contact link above for more information";
      }


      return View(viewModel);
    }


    // POST: /Tourist/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(TouristViewModel viewModel)
    {
      McIntireTour tour = VisitorsDb.Query<McIntireTour>().First(t => t.Id == viewModel.TourId);

      McIntireTourist tourist = new McIntireTourist
        {
          FirstName = viewModel.FirstName,
          LastName = viewModel.LastName,
          City = viewModel.City,
          Email = viewModel.Email,
          Id = Guid.NewGuid(),
          Phone = viewModel.Phone,
          State = viewModel.State,
          TourId = viewModel.TourId,
          Tour = tour,
          NumberInParty = viewModel.NumberInParty
        };

      VisitorsDb.Add(tourist);
      VisitorsDb.Save();

      SendWelcomeMessages(tourist);

      TempData[Constants.TempDataKeys.Message] =
        @"Thank you for registering your tour. We look forward to your visit. You should be receiving a confirmation email shortly.";

      return RedirectToAction("Details", new { id = tourist.Id });
    }


    // GET: /Tourist/Delete/5
    public ActionResult Cancel(Guid id)
    {
      TouristViewModel viewModel = GetTouristViewModel(id);

      if (viewModel != null)
      {
        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find a tourist with that id. id:{0}", id));
    }


    // POST: /Tourist/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Cancel(Guid id, TouristViewModel viewModel)
    {
      if (VisitorsDb.Query<McIntireTourist>().Any(t => t.Id == id))
      {
        McIntireTourist tourist = VisitorsDb.Query<McIntireTourist>().First(t => t.Id == id);

        WebUIHelper.SendTouristCancelMessage(tourist);

        VisitorsDb.Remove(tourist);
        VisitorsDb.Save();
      }
      else
      {
        throw new Elmah.ApplicationException(string.Format("We could not find a tourist with that id. id:{0}", id));
      }

      ViewBag.Message = "Your tour has been cancelled.";
      return View("Message");
    }


    private TouristViewModel GetTouristViewModel(Guid id)
    {
      if (VisitorsDb.Query<McIntireTourist>().Any(t => t.Id == id))
      {
        McIntireTourist tourist = VisitorsDb.Query<McIntireTourist>().First(t => t.Id == id);

        TouristViewModel viewModel = new TouristViewModel
          {
            FirstName = tourist.FirstName,
            LastName = tourist.LastName,
            City = tourist.City,
            Email = tourist.Email,
            Tour = tourist.Tour,
            TourId = tourist.TourId,
            Id = tourist.Id,
            Phone = tourist.Phone,
            State = tourist.State,
            NumberInParty = tourist.NumberInParty
          };

        if (VisitorsDb.UpcomingTours.Any())
        {
          viewModel.AvailableTours =
            VisitorsDb.UpcomingTours.ToList().Select(
              t =>
              new SelectListItem { Text = t.StartTime.ToString("F"), Value = t.Id.ToString(CultureInfo.CurrentCulture) }).ToList();
        }

        return viewModel;
      }

      return null;
    }


    private void SendWelcomeMessages(McIntireTourist tourist)
    {
      UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);

      string detailsURL = string.Format("{0}://{1}{2}",
        ControllerContext.RequestContext.HttpContext.Request.Url.Scheme,
        ControllerContext.RequestContext.HttpContext.Request.Url.Authority,
        urlHelper.Action("Details", "Tourist", new { id = tourist.Id }));

      string cancelURL = string.Format("{0}://{1}{2}",
        ControllerContext.RequestContext.HttpContext.Request.Url.Scheme,
        ControllerContext.RequestContext.HttpContext.Request.Url.Authority,
        urlHelper.Action("Cancel", "Tourist", new { id = tourist.Id }));

      string adminDetailsURL = string.Format("{0}://{1}{2}",
        ControllerContext.RequestContext.HttpContext.Request.Url.Scheme,
        ControllerContext.RequestContext.HttpContext.Request.Url.Authority,
        urlHelper.Action("Details", "Tourist", new { id = tourist.Id, Area = "Admin" }));

      WebUIHelper.SendWelcomeMessageToTourist(tourist, detailsURL, cancelURL);

      WebUIHelper.SendTourSignupMessageToAmbassadors(tourist, adminDetailsURL);
    }

  }
}