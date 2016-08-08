using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web.Mvc;
using Visitors.Domain;
using Visitors.WebUI.Admin.Models;
using Visitors.WebUI.Models;

namespace Visitors.WebUI.Admin.Controllers
{
  public class AmbassadorController : AdminBaseController
  {
    public AmbassadorController(IVisitorRepository db)
      : base(db)
    {
    }


    // GET: /Admin/Ambassador/
    public ActionResult Index()
    {
      List<AmbassadorViewModel> ambassadors = new List<AmbassadorViewModel>();

      if (VisitorsDb.Query<Ambassador>().Any())
      {
        ambassadors = VisitorsDb.Query<Ambassador>().Select(a => new AmbassadorViewModel
          {
            Id = a.Id,
            Active = a.Active,
            LastName = a.LastName,
            FirstName = a.FirstName,
            Email = a.Email,
            PhoneNumber = a.PhoneNumber,
            UserName = a.UserName,
            Visitors = a.Visitors,
            PrimaryTours = a.PrimaryTours,
            AssistantTours = a.AssistantTours
          }).ToList();
      }

      if (TempData[Constants.TempDataKeys.Message] != null)
      {
        ViewBag.Message = TempData[Constants.TempDataKeys.Message];
      }

      return View(ambassadors);
    }


    // GET: /Admin/Ambassador/Tours/5
    public ActionResult Tours(int id)
    {
      List<TourViewModel> tours = new List<TourViewModel>();
      var ambassadorTours = VisitorsDb.GetToursForAmbassabor(id);

      if (ambassadorTours.Any())
      {
        tours = ambassadorTours.Select(t => new TourViewModel
          {
            Id = t.Id,
            PrimaryAmbassador = t.PrimaryAmbassador,
            AssistantAmbassador = t.AssistantAmbassador,
            Comments = t.Comments,
            StartTime = t.StartTime,
            Tourists = t.Tourists
          }).ToList();
      }

      if (Request.IsAjaxRequest())
      {
        return PartialView("_Tours", tours);
      }

      return View(tours);
    }


    // GET: /Admin/Ambassador/DomainMembers
    public ActionResult DomainMembers()
    {
      List<AmbassadorViewModel> domainAmbassadors = new List<AmbassadorViewModel>();

      using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, "MCINTIRE"))
      {
        GroupPrincipal group = GroupPrincipal.FindByIdentity(principalContext, "McIntire-Ambassadors");

        if (group != null)
        {
          // add members to the collection
          foreach (Principal p in group.GetMembers())
          {
            UserPrincipal theUser = p as UserPrincipal;

            if (theUser != null)
            {
              // is this user already already an ambassador?
              if (!VisitorsDb.Query<Ambassador>().Any(a => a.Email.Equals(p.UserPrincipalName)))
              {

                // parse the Name into Last Name, First Name and UserName
                string[] upnParts = p.UserPrincipalName.Split(new[] { '@' });
                string userName = upnParts[0];
                string userNameWithparentheses = String.Format("({0})", userName);
                string[] nameParts = p.DisplayName.Split(new[] { ',' });
                string firstName = nameParts[1].Replace(userNameWithparentheses, string.Empty);

                AmbassadorViewModel ambassador = new AmbassadorViewModel
                  {
                    FirstName = firstName,
                    LastName = nameParts[0],
                    Email = p.UserPrincipalName,
                    UserName = userName
                  };
                domainAmbassadors.Add(ambassador);
              }
            }
          }
        }
      }
      if (TempData[Constants.TempDataKeys.Message] != null)
      {
        ViewBag.Message = TempData[Constants.TempDataKeys.Message].ToString();
      }

      return View(domainAmbassadors);
    }


    // POST: /Admin/Ambassador/Add/username
    [HttpPost]
    public ActionResult Add(string userPrincipalName)
    {
      // is this user already already an ambassador?
      if (VisitorsDb.Query<Ambassador>().Any(a => a.Email.Equals(userPrincipalName)))
      {
        ViewBag.Message = string.Format("A user with email address {0} is already an ambassador", userPrincipalName);
        return View("Message");
      }

      using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, "MCINTIRE"))
      {
        UserPrincipal user = UserPrincipal.FindByIdentity(principalContext, IdentityType.UserPrincipalName, userPrincipalName);

        if (user != null)
        {
          // parse the Name into Last Name, First Name and UserName
          string[] upnParts = user.UserPrincipalName.Split(new[] { '@' });
          string userName = upnParts[0];
          string userNameWithparentheses = String.Format("({0})", userName);
          string[] nameParts = user.DisplayName.Split(new[] { ',' });
          string firstName = nameParts[1].Replace(userNameWithparentheses, string.Empty);

          Ambassador ambassador = new Ambassador
          {
            FirstName = firstName,
            LastName = nameParts[0],
            Email = user.UserPrincipalName,
            UserName = userName,
            Active = true
          };

          VisitorsDb.Add(ambassador);
          VisitorsDb.Save();

          TempData[Constants.TempDataKeys.Message] =
            String.Format("{0} was added as an ambassador to this website", ambassador.FullName);
        }
      }

      return RedirectToAction("DomainMembers");
    }


    // GET: /Admin/Ambassador/Visitors/5
    public ActionResult Visitors(int id)
    {
      List<ClassVisitorViewModel> visitors = new List<ClassVisitorViewModel>();
      var ambassadorVisitors = VisitorsDb.GetVisitorsForAmbassabor(id);

      if (ambassadorVisitors.Any())
      {
        visitors = ambassadorVisitors.Select(t => new ClassVisitorViewModel
          {
            PreferedTimeSlot = t.PreferredTimeSlot,
            Ambassador = t.Ambassador,
            Email = t.Email,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Phone = t.Phone,
            Id = t.Id
          }).ToList();
      }

      if (Request.IsAjaxRequest())
      {
        return PartialView("_Visitors", visitors);
      }

      return View(visitors);
    }


    // GET: /Admin/Ambassador/Create
    public ActionResult Create()
    {
      AmbassadorViewModel ambassador = new AmbassadorViewModel
        {
          Active = true
        };
      return View(ambassador);
    }


    // POST: /Admin/Ambassador/Create
    [HttpPost]
    public ActionResult Create(AmbassadorViewModel viewModel)
    {
      Ambassador ambassador = new Ambassador
        {
          FirstName = viewModel.FirstName,
          LastName = viewModel.LastName,
          Active = viewModel.Active,
          UserName = viewModel.UserName,
          Email = viewModel.Email,
          PhoneNumber = viewModel.PhoneNumber
        };

      VisitorsDb.Add(ambassador);
      VisitorsDb.Save();

      return RedirectToAction("Index");
    }


    // GET: /Admin/Ambassador/Edit/5
    public ActionResult Edit(int id)
    {
      AmbassadorViewModel viewModel = GetAmbassadorViewModel(id);

      if (viewModel != null)
      {
        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find an ambassador with that id. id:{0}", id));
    }


    // POST: /Admin/Ambassador/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, AmbassadorViewModel viewModel)
    {
      if (VisitorsDb.Query<Ambassador>().Any(v => v.Id == id))
      {
        Ambassador ambassador = VisitorsDb.Query<Ambassador>().First(v => v.Id == id);
        ambassador.FirstName = viewModel.FirstName;
        ambassador.LastName = viewModel.LastName;
        ambassador.PhoneNumber = viewModel.PhoneNumber;
        ambassador.UserName = viewModel.UserName;
        ambassador.Email = viewModel.Email;
        ambassador.Active = viewModel.Active;

        VisitorsDb.Update(ambassador);
        VisitorsDb.Save();
      }

      return RedirectToAction("Index");
    }


    // GET: /Admin/Ambassador/Delete/5
    public ActionResult Delete(int id)
    {
      AmbassadorViewModel viewModel = GetAmbassadorViewModel(id);

      if (viewModel != null)
      {
        return View(viewModel);
      }

      throw new Elmah.ApplicationException(string.Format("We could not find an ambassador with that id. id:{0}", id));
    }


    // POST: /Admin/Ambassador/Delete/5
    [HttpPost]
    public ActionResult Delete(int id, AmbassadorViewModel viewModel)
    {
      if (VisitorsDb.Query<Ambassador>().Any(v => v.Id == id))
      {
        Ambassador ambassador = VisitorsDb.Query<Ambassador>().First(v => v.Id == id);
        TempData[Constants.TempDataKeys.Message] = String.Format("Ambassador {0} has been deleted.", ambassador.FullName);

        VisitorsDb.Remove(ambassador);
        VisitorsDb.Save();
      }
      else
      {
        throw new Elmah.ApplicationException(string.Format("We could not find an ambassador with that id. id:{0}", id));
      }

      return RedirectToAction("Index");
    }


    private AmbassadorViewModel GetAmbassadorViewModel(int id)
    {
      if (VisitorsDb.Query<Ambassador>().Any(a => a.Id == id))
      {
        Ambassador ambassador = VisitorsDb.Query<Ambassador>().First(a => a.Id == id);

        AmbassadorViewModel viewModel = new AmbassadorViewModel
          {
            Active = ambassador.Active,
            AssistantTours = ambassador.AssistantTours,
            Email = ambassador.Email,
            FirstName = ambassador.FirstName,
            LastName = ambassador.LastName,
            Id = ambassador.Id,
            PhoneNumber = ambassador.PhoneNumber,
            PrimaryTours = ambassador.PrimaryTours,
            UserName = ambassador.UserName,
            Visitors = ambassador.Visitors
          };

        return viewModel;
      }

      return null;
    }
  }
}