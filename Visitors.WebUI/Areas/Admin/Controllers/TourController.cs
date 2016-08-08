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
  public class TourController : AdminBaseController
  {
    public TourController(IVisitorRepository db)
      : base(db)
    { }


    // GET: /Admin/Tour/
    public ActionResult Index()
    {
      // the default date range should be from 10 days before the current date to 20 days past the academic year
      // this should allow an administrator to see a reasonable number of past tours as well as
      // tours happening 20 days in future when we get close to the end of the academic year
      int year = WebUIHelper.CurrentAcademicYearBegin.Year;

      DateTime beginDate = DateTime.Now.AddDays(-10);
      DateTime endDate = WebUIHelper.GetAcademicYearEnd(year).AddDays(20);

      ListToursViewModel viewModel = GetToursListViewModel(beginDate, endDate, year);
      return View(viewModel);
    }


    // GET: /Admin/ChangeYear/
    public ActionResult ChangeYear(int year)
    {
      DateTime academicYearBegin = WebUIHelper.GetAcademicYearBegin(year);
      DateTime academicYearEnd = WebUIHelper.GetAcademicYearEnd(year);

      ListToursViewModel viewModel = GetToursListViewModel(academicYearBegin, academicYearEnd, year);
      return View("Index", viewModel);
    }


    // GET: /Admin/Tour/Tourists/5
    public ActionResult Tourists(int id)
    {
      IEnumerable<TouristViewModel> tourists = new List<TouristViewModel>();

      if (VisitorsDb.GetTouristsForTour(id).Any())
      {
        tourists = VisitorsDb.GetTouristsForTour(id).Select(t => new TouristViewModel
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

      if (Request.IsAjaxRequest())
      {
        return PartialView("_Tourists", tourists);
      }

      return View(tourists);
    }


    // GET: /Admin/Tour/Create
    public ActionResult Create()
    {
      var activeAmbassadors = VisitorsDb.Query<Ambassador>().Where(a => a.Active);

      TourViewModel viewModel = new TourViewModel
        {
          StartTime = DateTime.Now
        };

      if (activeAmbassadors.Any())
      {
        viewModel.ActiveAmbassadors = activeAmbassadors.AsEnumerable().Select(a => new SelectListItem
          {
            Text = a.FullName,
            Value = a.Id.ToString(CultureInfo.CurrentCulture)
          });
      }

      return View(viewModel);
    }


    // POST: /Admin/Tour/Create
    [HttpPost]
    public ActionResult Create(TourViewModel viewModel)
    {
      McIntireTour tour = new McIntireTour
      {
        StartTime = viewModel.StartTime,
        AssistantAmbassadorId = viewModel.AssistantAmbassadorId,
        PrimaryAmbassadorId = viewModel.PrimaryAmbassadorId,
        Comments = viewModel.Comments
      };

      VisitorsDb.Add(tour);
      VisitorsDb.Save();

      return RedirectToAction("Index");
    }


    // GET: /Admin/Tour/Edit/5
    public ActionResult Edit(int id)
    {
      TourViewModel viewModel = GetTourViewModel(id);

      if (viewModel != null)
      {
        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find a tour with that id. id:{0}", id));
    }


    // POST: /Admin/Tour/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, TourViewModel viewModel)
    {
      McIntireTour tour = VisitorsDb.Query<McIntireTour>().First(t => t.Id == id);
      tour.Comments = viewModel.Comments;
      tour.StartTime = viewModel.StartTime;
      tour.PrimaryAmbassadorId = viewModel.PrimaryAmbassadorId;
      tour.AssistantAmbassadorId = viewModel.AssistantAmbassadorId;

      VisitorsDb.Update(tour);
      VisitorsDb.Save();

      return RedirectToAction("Index");
    }


    // GET: /Admin/Tour/Delete/5
    public ActionResult Delete(int id)
    {
      TourViewModel viewModel = GetTourViewModel(id);

      if (viewModel != null)
      {
        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find a tour with that id. id:{0}", id));
    }


    // POST: /Admin/Tour/Delete/5
    [HttpPost]
    public ActionResult Delete(int id, TourViewModel viewModel)
    {
      McIntireTour tour = VisitorsDb.Query<McIntireTour>().First(t => t.Id == id);

      VisitorsDb.Remove(tour);
      VisitorsDb.Save();

      return RedirectToAction("Index");
    }

    private TourViewModel GetTourViewModel(int tourId)
    {
      if (VisitorsDb.Query<McIntireTour>().Any(t => t.Id == tourId))
      {
        McIntireTour tour = VisitorsDb.Query<McIntireTour>().First(t => t.Id == tourId);
        TourViewModel viewModel = new TourViewModel
        {
          Id = tour.Id,
          Comments = tour.Comments,
          StartTime = tour.StartTime,
          PrimaryAmbassadorId = tour.PrimaryAmbassadorId,
          AssistantAmbassadorId = tour.AssistantAmbassadorId,
          PrimaryAmbassador = tour.PrimaryAmbassador,
          AssistantAmbassador = tour.AssistantAmbassador,
          Tourists = tour.Tourists
        };

        var activeAmbassadors = VisitorsDb.Query<Ambassador>().Where(a => a.Active);

        if (activeAmbassadors.Any())
        {
          viewModel.ActiveAmbassadors = activeAmbassadors.AsEnumerable().Select(a => new SelectListItem
          {
            Text = a.FullName,
            Value = a.Id.ToString(CultureInfo.CurrentCulture)
          });
        }

        return viewModel;
      }

      return null;
    }


    private ListToursViewModel GetToursListViewModel(DateTime startDate, DateTime endDate, int year)
    {

      ListToursViewModel viewModel = new ListToursViewModel
        {
          Year = year,
          BeginDate = startDate,
          EndDate = endDate
        };

      if (VisitorsDb.Query<McIntireTour>().Any(t => (t.StartTime > startDate) && (t.StartTime < endDate)))
      {
        viewModel.Tours = VisitorsDb.Query<McIntireTour>().Where(t => (t.StartTime > startDate) && (t.StartTime < endDate)).Select(t => new TourViewModel
        {
          Id = t.Id,
          AssistantAmbassador = t.AssistantAmbassador,
          AssistantAmbassadorId = t.AssistantAmbassadorId,
          Comments = t.Comments,
          PrimaryAmbassador = t.PrimaryAmbassador,
          PrimaryAmbassadorId = t.PrimaryAmbassadorId,
          StartTime = t.StartTime,
          Tourists = t.Tourists,
        }).ToList();
      }

      return viewModel;
    }
  }
}
