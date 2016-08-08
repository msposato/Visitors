using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Elmah;
using Visitors.Domain;
using Visitors.WebUI.Admin.Models;

namespace Visitors.WebUI.Admin.Controllers
{
  public class AcademicSubjectController : AdminBaseController
  {
    public AcademicSubjectController(IVisitorRepository db)
      : base(db)
    { }


    // GET: /Admin/AcademicSubject/
    public ActionResult Index()
    {
      List<AcademicSubjectViewModel> subjects = new List<AcademicSubjectViewModel>();

      if (VisitorsDb.Query<AcademicSubject>().Any())
      {
        subjects = VisitorsDb.Query<AcademicSubject>().Select(a => new AcademicSubjectViewModel
          {
            Id = a.Id,
            Name = a.Name
          }).ToList();
      }

      return View(subjects);
    }


    // GET: /Admin/AcademicSubject/Create
    public ActionResult Create()
    {
      AcademicSubjectViewModel viewModel = new AcademicSubjectViewModel();
      return View(viewModel);
    }


    // POST: /Admin/AcademicSubject/Create
    [HttpPost]
    public ActionResult Create(AcademicSubjectViewModel viewModel)
    {
      AcademicSubject subject = new AcademicSubject
        {
          Name = viewModel.Name
        };

      VisitorsDb.Add(subject);
      VisitorsDb.Save();

      return RedirectToAction("Index");
    }


    // GET: /Admin/AcademicSubject/Edit/5
    public ActionResult Edit(int id)
    {
      AcademicSubjectViewModel viewModel = GetAcademicSubjectViewModel(id);

      if (viewModel != null)
      {
        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find an area of interest with that id. id:{0}", id));
    }


    // POST: /Admin/AcademicSubject/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, AcademicSubjectViewModel viewModel)
    {
      if (VisitorsDb.Query<AcademicSubject>().Any(v => v.Id == id))
      {
        AcademicSubject subject = VisitorsDb.Query<AcademicSubject>().First(v => v.Id == id);
        subject.Name = viewModel.Name;

        VisitorsDb.Update(subject);
        VisitorsDb.Save();
      }

      return RedirectToAction("Index");
    }


    // GET: /Admin/AcademicSubject/Delete/5
    public ActionResult Delete(int id)
    {
      AcademicSubjectViewModel viewModel = GetAcademicSubjectViewModel(id);

      if (viewModel != null)
      {
        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find an area of interest with that id. id:{0}", id));
    }


    // POST: /Admin/AcademicSubject/Delete/5
    [HttpPost]
    public ActionResult Delete(int id, AcademicSubjectViewModel viewModel)
    {
      if (VisitorsDb.Query<AcademicSubject>().Any(v => v.Id == id))
      {
        AcademicSubject subject = VisitorsDb.Query<AcademicSubject>().First(v => v.Id == id);
        VisitorsDb.Remove(subject);
        VisitorsDb.Save();
      }
      else
      {
        throw new Elmah.ApplicationException(string.Format("We could not find an area of interest with that id. id:{0}", id));
      }

      return RedirectToAction("Index");
    }

    private AcademicSubjectViewModel GetAcademicSubjectViewModel(int id)
    {
      if (VisitorsDb.Query<AcademicSubject>().Any(a => a.Id == id))
      {
        AcademicSubject subject = VisitorsDb.Query<AcademicSubject>().First(a => a.Id == id);

        AcademicSubjectViewModel viewModel = new AcademicSubjectViewModel
        {
          Name = subject.Name,
          Id = subject.Id
        };

        return viewModel;
      }

      return null;
    }
  }
}
