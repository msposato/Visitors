using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Visitors.Domain;
using Visitors.WebUI.Admin.Models;
using Visitors.WebUI.Models;

namespace Visitors.WebUI.Admin.Controllers
{
  public class TimeSlotController : AdminBaseController
  {

    public TimeSlotController(IVisitorRepository db)
      : base(db)
    { }

    // GET: /Admin/TimeSlot/
    public ActionResult Index()
    {
      ListTimeSlotsViewModel viewModel = GetTimeSlotsLists(DateTime.Now.Year, DateTime.Now.Month);

      return View(viewModel);
    }


    // GET: /Admin/ChangeYear/
    public ActionResult ChangeYear(int year, int month = 0)
    {
      ListTimeSlotsViewModel viewModel = GetTimeSlotsLists(year, month);

      return View("Index", viewModel);
    }


    // GET: /Admin/TimeSlot/Visitors/5
    public ActionResult Visitors(int id)
    {
      List<ClassVisitorViewModel> visitors = new List<ClassVisitorViewModel>();

      if (VisitorsDb.Query<ClassVisitor>().Any(v => v.PreferredTimeSlotId == id))
      {
        visitors =
          VisitorsDb.Query<ClassVisitor>().Where(v => v.PreferredTimeSlotId == id).Select(v => new ClassVisitorViewModel
            {
              PreferedTimeSlot = v.PreferredTimeSlot,
              Ambassador = v.Ambassador,
              Email = v.Email,
              FirstName = v.FirstName,
              LastName = v.LastName,
              Phone = v.Phone,
              Id = v.Id
            }).ToList();

      }
      if (Request.IsAjaxRequest())
      {
        return PartialView("_Visitors", visitors);
      }

      return View(visitors);
    }


    // GET: /Admin/TimeSlot/Create
    public ActionResult Create()
    {
      return View();
    }


    // POST: /Admin/TimeSlot/Create
    [HttpPost]
    public ActionResult Create(ClassVisitTimeSlotViewModel viewModel)
    {
      ClassVisitTimeSlot timeslot = new ClassVisitTimeSlot
        {
          EndDateTime = viewModel.EndDateTime,
          StartDateTime = viewModel.StartDateTime,
          SeatsAvailable = viewModel.SeatsAvailable,
          SeatsRemaining = viewModel.SeatsAvailable
        };

      VisitorsDb.Add(timeslot);
      VisitorsDb.Save();

      return RedirectToAction("Index");
    }


    // GET: /Admin/TimeSlot/Edit/5
    public ActionResult Edit(int id)
    {
      if (VisitorsDb.Query<ClassVisitTimeSlot>().Any(t => t.Id == id))
      {
        ClassVisitTimeSlot timeSlot = VisitorsDb.Query<ClassVisitTimeSlot>().First(t => t.Id == id);
        ClassVisitTimeSlotViewModel viewModel = new ClassVisitTimeSlotViewModel
          {
            EndDateTime = timeSlot.EndDateTime,
            StartDateTime = timeSlot.StartDateTime,
            Id = timeSlot.Id,
            SeatsAvailable = timeSlot.SeatsAvailable,
            SeatsRemaining = timeSlot.SeatsRemaining
          };

        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find a time slot with that id. id:{0}", id));
    }


    // POST: /Admin/TimeSlot/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, ClassVisitTimeSlotViewModel viewModel)
    {
      ClassVisitTimeSlot timeSlot = VisitorsDb.Query<ClassVisitTimeSlot>().First(t => t.Id == id);
      timeSlot.SeatsAvailable = viewModel.SeatsAvailable;
      timeSlot.EndDateTime = viewModel.EndDateTime;
      timeSlot.StartDateTime = viewModel.StartDateTime;

      VisitorsDb.Update(timeSlot);
      VisitorsDb.Save();

      WebUIHelper.UpdateSeatsRemaining(id, VisitorsDb);

      return RedirectToAction("Index");
    }


    // GET: /Admin/TimeSlot/Delete/5
    public ActionResult Delete(int id)
    {
      if (VisitorsDb.Query<ClassVisitTimeSlot>().Any(t => t.Id == id))
      {
        ClassVisitTimeSlot timeSlot = VisitorsDb.Query<ClassVisitTimeSlot>().First(t => t.Id == id);
        ClassVisitTimeSlotViewModel viewModel = new ClassVisitTimeSlotViewModel
        {
          EndDateTime = timeSlot.EndDateTime,
          StartDateTime = timeSlot.StartDateTime,
          Id = timeSlot.Id,
          SeatsAvailable = timeSlot.SeatsAvailable,
          SeatsRemaining = timeSlot.SeatsRemaining
        };

        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find a time slot with that id. id:{0}", id));
    }


    // POST: /Admin/TimeSlot/Delete/5
    [HttpPost]
    public ActionResult Delete(int id, ClassVisitTimeSlotViewModel viewModel)
    {
      ClassVisitTimeSlot timeSlot = VisitorsDb.Query<ClassVisitTimeSlot>().First(t => t.Id == id);

      VisitorsDb.Remove(timeSlot);
      VisitorsDb.Save();

      return RedirectToAction("Index");
    }


    private ListTimeSlotsViewModel GetTimeSlotsLists(int year, int month)
    {
      ListTimeSlotsViewModel viewModel = new ListTimeSlotsViewModel();
      List<ClassVisitTimeSlotViewModel> timeslots = new List<ClassVisitTimeSlotViewModel>();

      DateTime begin, end;

      if (month == 0)
      {
        begin = new DateTime(year, 1, 1);
        end = new DateTime(year + 1, 1, 1);
      }
      else
      {
        begin = new DateTime(year, month, 1);
        end = month >= 12 ? new DateTime(year + 1, 1, 1) : new DateTime(year, month + 1, 1);
      }

      if (VisitorsDb.Query<ClassVisitTimeSlot>().Any(t => (t.StartDateTime > begin) && (t.EndDateTime < end)))
      {
        timeslots = VisitorsDb.Query<ClassVisitTimeSlot>().Where(t => (t.StartDateTime > begin) && (t.EndDateTime < end))
          .Select(t => new ClassVisitTimeSlotViewModel
        {
          StartDateTime = t.StartDateTime,
          EndDateTime = t.EndDateTime,
          Id = t.Id,
          SeatsAvailable = t.SeatsAvailable,
          SeatsRemaining = t.SeatsRemaining,
          PrimaryVisitors = t.PrimaryVisitors
        }).ToList();
      }

      viewModel.Year = year;
      viewModel.Month = month;
      viewModel.TimeSlots = timeslots;

      return viewModel;
    }
  }
}
