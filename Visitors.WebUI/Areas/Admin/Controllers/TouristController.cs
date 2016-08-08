using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Visitors.Domain;
using Visitors.WebUI.Admin.Models;
using Visitors.WebUI.Models;

namespace Visitors.WebUI.Admin.Controllers
{
  public class TouristController : AdminBaseController
  {
    public TouristController(IVisitorRepository dB)
      : base(dB)
    {
    }

    public ActionResult Index()
    {
      // the default date range should be from 10 days before the current date to 20 days past the academic year
      // this should allow an administrator to see a reasonable number of past tours as well as
      // tours happening 20 days in future when we get close to the end of the academic year
      int year = WebUIHelper.CurrentAcademicYearBegin.Year;

      DateTime beginDate = DateTime.Now.AddDays(-10);
      DateTime endDate = WebUIHelper.GetAcademicYearEnd(year).AddDays(20);

      ListTouristsViewModel viewModel = GetTouristsListViewModel(beginDate, endDate, year);

      if (TempData[Constants.TempDataKeys.Message] != null)
      {
        ViewBag.Message = TempData[Constants.TempDataKeys.Message];
      }

      return View(viewModel);
    }


    public ActionResult ChangeYear(int year)
    {
      DateTime beginDate = WebUIHelper.GetAcademicYearBegin(year);
      DateTime endDate = WebUIHelper.GetAcademicYearEnd(year);

      ListTouristsViewModel viewModel = GetTouristsListViewModel(beginDate, endDate, year);
      return View("Index", viewModel);
    }


    // GET: /Tourist/Details/Guid
    public ActionResult Details(Guid id)
    {
      TouristViewModel viewModel = GetTouristViewModel(id);

      if (viewModel != null)
      {
        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find a tourist with that id. id:{0}", id));
    }


    // GET: /Admin/Tour/Edit/5
    public ActionResult Edit(Guid id)
    {
      TouristViewModel viewModel = GetTouristViewModel(id);

      if (viewModel != null)
      {
        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find a tourist with that id. id:{0}", id));
    }


    // POST: /Admin/Tour/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Guid id, TouristViewModel viewModel)
    {
      McIntireTour tour = VisitorsDb.Query<McIntireTour>().First(t => t.Id == viewModel.TourId);
      McIntireTourist tourist = VisitorsDb.Query<McIntireTourist>().First(t => t.Id == id);
      tourist.City = viewModel.City;
      tourist.Email = viewModel.Email;
      tourist.FirstName = viewModel.FirstName;
      tourist.LastName = viewModel.LastName;
      tourist.Phone = viewModel.Phone;
      tourist.State = viewModel.State;
      tourist.Tour = tour;
      tourist.NumberInParty = viewModel.NumberInParty;

      VisitorsDb.Update(tourist);
      VisitorsDb.Save();

      return RedirectToAction("Index");
    }


    // GET: /Tour/Delete/5
    public ActionResult Delete(Guid id)
    {
      TouristViewModel viewModel = GetTouristViewModel(id);

      if (viewModel != null)
      {
        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find a tourist with that id. id:{0}", id));
    }


    // POST: /Tour/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(Guid id, TouristViewModel viewModel)
    {
      if (VisitorsDb.Query<McIntireTourist>().Any(t => t.Id == id))
      {
        McIntireTourist tourist = VisitorsDb.Query<McIntireTourist>().First(t => t.Id == id);
        TempData[Constants.TempDataKeys.Message] = String.Format("The tour registration for {0} on {1} has been cancelled.", tourist.FullName, tourist.Tour.StartTime);

        WebUIHelper.SendTouristCancelMessage(tourist);

        VisitorsDb.Remove(tourist);
        VisitorsDb.Save();

        return RedirectToAction("Index");
      }

      throw new Elmah.ApplicationException(string.Format("We could not find a tourist with that id. id:{0}", id));
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

        // if the tour for this tourist is no longer available, for example it was in the past,
        // add it to the view model here, so the tour will still be displayed
        if (
          !viewModel.AvailableTours.Any(
            t => t.Value.Equals(viewModel.TourId.ToString(CultureInfo.CurrentCulture))))
        {
          viewModel.AvailableTours.Add(new SelectListItem
          {
            Value = viewModel.TourId.ToString(CultureInfo.CurrentCulture),
            Text = viewModel.Tour.StartTime.ToString("F")
          });
        }


        return viewModel;
      }

      return null;
    }


    private ListTouristsViewModel GetTouristsListViewModel(DateTime beginDate, DateTime endDate, int year)
    {
      ListTouristsViewModel viewModel = new ListTouristsViewModel
      {
        Year = year,
        BeginDate = beginDate,
        EndDate = endDate
      };

      if (VisitorsDb.Query<McIntireTourist>().Any(t => (t.Tour.StartTime > beginDate) && (t.Tour.StartTime < endDate)))
      {
        viewModel.Tourists =
          VisitorsDb.Query<McIntireTourist>().Where(t => (t.Tour.StartTime > beginDate) && (t.Tour.StartTime < endDate)).Select(t => new TouristViewModel
            {
              Id = t.Id,
              FirstName = t.FirstName,
              LastName = t.LastName,
              Email = t.Email,
              City = t.City,
              Phone = t.Phone,
              State = t.State,
              Tour = t.Tour,
              NumberInParty = t.NumberInParty
            }).ToList();
      }

      return viewModel;
    }
  }
}