using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using Visitors.Domain;
using Visitors.WebUI.Models;

namespace Visitors.WebUI
{
  public class WebUIHelper
  {
    private static readonly string AmbassadorGroupEmailAddress;
    private static readonly string FromEmailAddress;
    private static readonly string FromEmailName;
    private static readonly string FromEmailAlias;
    private static readonly HashSet<SelectListItem> StatesHashSet;
    private static readonly bool EnableEmailMessages;


    static WebUIHelper()
    {
      FromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"];
      FromEmailName = ConfigurationManager.AppSettings["FromEmailName"];
      FromEmailAlias = ConfigurationManager.AppSettings["FromEmailAlias"];
      AmbassadorGroupEmailAddress = ConfigurationManager.AppSettings["AmbassadorGroupEmailAddress"];
      EnableEmailMessages = Boolean.Parse(ConfigurationManager.AppSettings["EnableEmailMessages"]);

      StatesHashSet = new HashSet<SelectListItem>
        {
          new SelectListItem {Value = "AL", Text = "Alabama"},
          new SelectListItem {Value = "AK", Text = "Alaska"},
          new SelectListItem {Value = "AZ", Text = "Arizona"},
          new SelectListItem {Value = "AR", Text = "Arkansas"},
          new SelectListItem {Value = "CA", Text = "California"},
          new SelectListItem {Value = "CO", Text = "Colorado"},
          new SelectListItem {Value = "CT", Text = "Connecticut"},
          new SelectListItem {Value = "DE", Text = "Delaware"},
          new SelectListItem {Value = "DC", Text = "District Of Columbia"},
          new SelectListItem {Value = "FL", Text = "Florida"},
          new SelectListItem {Value = "GA", Text = "Georgia"},
          new SelectListItem {Value = "HI", Text = "Hawaii"},
          new SelectListItem {Value = "ID", Text = "Idaho"},
          new SelectListItem {Value = "IL", Text = "Illinois"},
          new SelectListItem {Value = "IN", Text = "Indiana"},
          new SelectListItem {Value = "IA", Text = "Iowa"},
          new SelectListItem {Value = "KS", Text = "Kansas"},
          new SelectListItem {Value = "KY", Text = "Kentucky"},
          new SelectListItem {Value = "LA", Text = "Louisiana"},
          new SelectListItem {Value = "ME", Text = "Maine"},
          new SelectListItem {Value = "MD", Text = "Maryland"},
          new SelectListItem {Value = "MA", Text = "Massachusetts"},
          new SelectListItem {Value = "MI", Text = "Michigan"},
          new SelectListItem {Value = "MN", Text = "Minnesota"},
          new SelectListItem {Value = "MS", Text = "Mississippi"},
          new SelectListItem {Value = "MO", Text = "Missouri"},
          new SelectListItem {Value = "MT", Text = "Montana"},
          new SelectListItem {Value = "NE", Text = "Nebraska"},
          new SelectListItem {Value = "NV", Text = "Nevada"},
          new SelectListItem {Value = "NH", Text = "New Hampshire"},
          new SelectListItem {Value = "NJ", Text = "New Jersey"},
          new SelectListItem {Value = "NM", Text = "New Mexico"},
          new SelectListItem {Value = "NY", Text = "New York"},
          new SelectListItem {Value = "NC", Text = "North Carolina"},
          new SelectListItem {Value = "ND", Text = "North Dakota"},
          new SelectListItem {Value = "OH", Text = "Ohio"},
          new SelectListItem {Value = "OK", Text = "Oklahoma"},
          new SelectListItem {Value = "OR", Text = "Oregon"},
          new SelectListItem {Value = "PA", Text = "Pennsylvania"},
          new SelectListItem {Value = "RI", Text = "Rhode Island"},
          new SelectListItem {Value = "SC", Text = "South Carolina"},
          new SelectListItem {Value = "SD", Text = "South Dakota"},
          new SelectListItem {Value = "TN", Text = "Tennessee"},
          new SelectListItem {Value = "TX", Text = "Texas"},
          new SelectListItem {Value = "UT", Text = "Utah"},
          new SelectListItem {Value = "VT", Text = "Vermont"},
          new SelectListItem {Value = "VA", Text = "Virginia"},
          new SelectListItem {Value = "WA", Text = "Washington"},
          new SelectListItem {Value = "WV", Text = "West Virginia"},
          new SelectListItem {Value = "WI", Text = "Wisconsin"},
          new SelectListItem {Value = "WY", Text = "Wyoming"}
        };
    }


    public static ClassVisitorViewModel GetClassVisitorViewModel(Guid id, IVisitorRepository visitorsDb)
    {
      var visitors = visitorsDb.Query<ClassVisitor>().Where(v => v.Id == id);

      if (visitors.Any())
      {
        ClassVisitor visitor = visitors.First();

        ClassVisitorViewModel viewModel = new ClassVisitorViewModel
          {
            FirstName = visitor.FirstName,
            LastName = visitor.LastName,
            Email = visitor.Email,
            Ambassador = visitor.Ambassador,
            PreferedTimeSlot = visitor.PreferredTimeSlot,
            PreferedTimeSlotId = visitor.PreferredTimeSlot.Id,
            AlternateTimeSlot = visitor.AlternateTimeSlot,
            Address1 = visitor.Address1,
            Address2 = visitor.Address2,
            City = visitor.City,
            Country = visitor.Country,
            Phone = visitor.Phone,
            State = visitor.State,
            PostalCode = visitor.PostalCode,
            YearInSchool = visitor.YearInSchool,
            Id = visitor.Id
          };

        if (visitor.AlternateTimeSlot != null)
        {
          viewModel.AlternateTimeSlotId = visitor.AlternateTimeSlot.Id;
        }

        if (visitor.AcademicSubjects.Any())
        {
          viewModel.AcademicSubjects =
            visitor.AcademicSubjects.Select(a => new CheckBoxItem { Label = a.Name, Id = a.Id, IsChecked = true });
        }

        // add the available time slots
        if (visitorsDb.AvailableTimeSlots.Any())
        {
          viewModel.AvailableTimeSlots =
            visitorsDb.AvailableTimeSlots.AsEnumerable().OrderBy(t => t.StartDateTime).Select(t => new SelectListItem
              {
                Text = t.Description,
                Value = t.Id.ToString(CultureInfo.CurrentCulture)
              }).ToList();

          viewModel.AlternateTimeSlots = visitorsDb.AvailableTimeSlots.AsEnumerable().OrderBy(t => t.StartDateTime).Select(t => new SelectListItem
          {
            Text = t.Description,
            Value = t.Id.ToString(CultureInfo.CurrentCulture)
          }).ToList();
        }

        // if the preferred time slot for this visitor is no longer an available timeslot add it to the view model
        if (
          !viewModel.AvailableTimeSlots.Any(
            t => t.Value.Equals(viewModel.PreferedTimeSlotId.ToString(CultureInfo.CurrentCulture))))
        {
          viewModel.AvailableTimeSlots.Add(new SelectListItem
            {
              Value = viewModel.PreferedTimeSlot.Id.ToString(CultureInfo.CurrentCulture),
              Text = viewModel.PreferedTimeSlot.Description
            });
        }

        if (visitor.AlternateTimeSlot != null)
        {
          // if the alternate time slot for this visitor is no longer an available timeslot add it to the view model       
          if (
            !viewModel.AlternateTimeSlots.Any(
              t => t.Value.Equals(viewModel.AlternateTimeSlotId.ToString())))
          {
            viewModel.AlternateTimeSlots.Add(new SelectListItem
            {
              Value = viewModel.AlternateTimeSlot.Id.ToString(CultureInfo.CurrentCulture),
              Text = viewModel.AlternateTimeSlot.Description
            });
          }
        }

        if (visitor.Ambassador != null)
        {
          viewModel.AmbassadorId = visitor.Ambassador.Id;
        }

        return viewModel;
      }

      return null;
    }


    public static IEnumerable<SelectListItem> States
    {
      get { return StatesHashSet; }
    }


    public static void UpdateSeatsRemaining(int timeSlotId, IVisitorRepository visitorsDb)
    {
      if (visitorsDb.Query<ClassVisitTimeSlot>().Any(t => t.Id == timeSlotId))
      {
        ClassVisitTimeSlot timeSlot = visitorsDb.Query<ClassVisitTimeSlot>().First(t => t.Id == timeSlotId);
        int seatsTaken = visitorsDb.Query<ClassVisitor>().Count(v => v.PreferredTimeSlotId == timeSlot.Id);
        timeSlot.SeatsRemaining = timeSlot.SeatsAvailable - seatsTaken;

        visitorsDb.Update(timeSlot);
        visitorsDb.Save();
      }
    }


    public static void SendEmail(string bodyText, string subject, List<string> toAddrs)
    {
      if (EnableEmailMessages)
      {
        MailMessage message = new MailMessage { From = new MailAddress(FromEmailAddress, FromEmailAlias) };

        foreach (string toAddr in toAddrs)
        {
          message.To.Add(new MailAddress(toAddr));
        }

        message.Subject = subject;

        message.Body = bodyText;

        SmtpClient client = new SmtpClient("exchange.comm.virginia.edu", 587) { UseDefaultCredentials = true };
        client.Send(message);
      }
    }


    public static void SendWelcomeMessageToVisitor(ClassVisitor visitor, string detailsURL, string cancelURL)
    {
      List<string> recipients = new List<string>
        {
          visitor.Email
        };

      StringBuilder welcomeMessage = new StringBuilder();
      welcomeMessage.AppendFormat("{0},\n", visitor.FirstName);
      welcomeMessage.AppendLine();
      welcomeMessage.AppendLine(
        "Thank you registering for a McIntire class visit. We look forward to your vist. A McIntire Ambassador will be contacting you shortly.");
      welcomeMessage.AppendLine();
      welcomeMessage.AppendFormat("You requested a class visit on: {0}\n", visitor.PreferredTimeSlot.Description);
      welcomeMessage.AppendLine();
      welcomeMessage.AppendFormat("Click this link to review the details of your class visit: {0}\n", detailsURL);
      welcomeMessage.AppendLine();
      welcomeMessage.AppendFormat("Click this link if you need to cancel your class visit: {0}\n", cancelURL);
      welcomeMessage.AppendLine();
      welcomeMessage.AppendLine("Thank You,");
      welcomeMessage.AppendLine();
      welcomeMessage.AppendLine(FromEmailName);
      welcomeMessage.AppendLine("Class Visit Coordinator");


      SendEmail(welcomeMessage.ToString(), "McIntire Class Visit Registration", recipients);
    }


    public static void SendRegistrationMessageToAmbassadors(ClassVisitor visitor, string editURL, string detailsURL)
    {
      List<string> recipients = new List<string> { AmbassadorGroupEmailAddress };

      StringBuilder registrationMessage = new StringBuilder();
      registrationMessage.AppendLine("McIntire Ambassadors,");
      registrationMessage.AppendLine();
      registrationMessage.AppendLine("A prospective student has registered for a class visit (details below).");
      registrationMessage.AppendLine();
      registrationMessage.AppendFormat("Visitor: {0}\n", visitor.FullName);
      registrationMessage.AppendFormat("Year in School: {0}\n", visitor.YearInSchool);
      registrationMessage.AppendFormat("Visit Date: {0}\n", visitor.PreferredTimeSlot.Description);
      registrationMessage.AppendLine();
      registrationMessage.AppendFormat("Click this link to review the details of the class visit: {0}\n", detailsURL);
      registrationMessage.AppendLine();
      registrationMessage.AppendFormat("Click this link to sign up to be an ambassador for this class visit: {0}\n",
                                       editURL);
      registrationMessage.AppendLine();
      registrationMessage.AppendLine("Thank You,");
      registrationMessage.AppendLine();
      registrationMessage.AppendLine(FromEmailName);
      registrationMessage.AppendLine("Class Visit Coordinator");

      SendEmail(registrationMessage.ToString(), "New Class Visit Registration", recipients);
    }


    public static void SendAmbassadorCancelledMessage(ClassVisitor visitor, string detailsURL)
    {
      if (visitor.Ambassador != null)
      {
        return;
      }

      List<string> recipients = new List<string> { AmbassadorGroupEmailAddress };

      StringBuilder ambassadorAssignedMessage = new StringBuilder();
      ambassadorAssignedMessage.AppendLine("McIntire Ambassador,");
      ambassadorAssignedMessage.AppendLine();
      ambassadorAssignedMessage.AppendFormat("An ambassador is no longer assigned to the class visit for {0} on {1}.\n",
                                             visitor.FullName, visitor.PreferredTimeSlot.Description);
      ambassadorAssignedMessage.AppendLine();
      ambassadorAssignedMessage.AppendLine("A new ambassador is needed for this class visit");
      ambassadorAssignedMessage.AppendLine();
      ambassadorAssignedMessage.AppendFormat("Click this link to review the details of the class visit: {0}\n", detailsURL);
      ambassadorAssignedMessage.AppendLine();
      ambassadorAssignedMessage.AppendLine("Thank You,");
      ambassadorAssignedMessage.AppendLine();
      ambassadorAssignedMessage.AppendLine(FromEmailName);
      ambassadorAssignedMessage.AppendLine("Class Visit Coordinator");

      SendEmail(ambassadorAssignedMessage.ToString(), "Ambassador Needed for Class Visit", recipients);
    }


    public static void SendCancellationMessage(ClassVisitor visitor)
    {
      List<string> recipients = new List<string> { visitor.Email };

      if ((visitor.Ambassador != null) && (!string.IsNullOrEmpty(visitor.Ambassador.Email)))
      {
        recipients.Add(visitor.Ambassador.Email);
      }

      StringBuilder cancelMessage = new StringBuilder();
      cancelMessage.AppendFormat("{0},\n", visitor.FirstName);
      cancelMessage.AppendLine();
      cancelMessage.AppendFormat(
        "This message is to confirm cancellation of your class visit, which was scheduled for {0}\n",
        visitor.PreferredTimeSlot.Description);
      cancelMessage.AppendLine();
      cancelMessage.AppendLine("Thank You,");
      cancelMessage.AppendLine();
      cancelMessage.AppendLine(FromEmailName);
      cancelMessage.AppendLine("Class Visit Coordinator");

      SendEmail(cancelMessage.ToString(), "McIntire Class Visit Cancellation", recipients);
    }


    public static void SendWelcomeMessageToTourist(McIntireTourist tourist, string detailsURL, string cancelURL)
    {
      List<string> recipients = new List<string>
        {
          tourist.Email
        };

      StringBuilder welcomeMessage = new StringBuilder();
      welcomeMessage.AppendFormat("{0},\n", tourist.FirstName);
      welcomeMessage.AppendLine();
      welcomeMessage.AppendLine(
        "Thank you for registering for a Tour of McIntire. We look forward to your visit.");
      welcomeMessage.AppendLine();
      welcomeMessage.AppendFormat("Please meet the McIntire Ambassador on the fourth-level of Rouss & Robertson Halls at: {0}\n", tourist.Tour.StartTime);
      welcomeMessage.AppendLine();
      welcomeMessage.AppendFormat("Click this link to review the details of your tour: {0}\n", detailsURL);
      welcomeMessage.AppendLine();
      welcomeMessage.AppendFormat("Click this link if you need to cancel your tour: {0}\n", cancelURL);
      welcomeMessage.AppendLine();
      welcomeMessage.AppendLine("Thank You,");
      welcomeMessage.AppendLine();
      welcomeMessage.AppendLine(FromEmailName);
      welcomeMessage.AppendLine("McIntire Tour Coordinator");

      SendEmail(welcomeMessage.ToString(), "McIntire Tour Registration", recipients);
    }


    public static void SendTourSignupMessageToAmbassadors(McIntireTourist tourist, string detailsURL)
    {
      List<string> recipients = new List<string> { AmbassadorGroupEmailAddress };

      StringBuilder touristMessage = new StringBuilder();
      touristMessage.AppendLine("McIntire Ambassadors,");
      touristMessage.AppendLine();
      touristMessage.AppendLine("A guest has registered for a Tour of McIntire (details below).");
      touristMessage.AppendLine();
      touristMessage.AppendFormat("Visitor: {0}\n", tourist.FullName);
      touristMessage.AppendFormat("Tour Date: {0}\n", tourist.Tour.StartTime);
      touristMessage.AppendLine();
      touristMessage.AppendFormat("Click this link to review the details: {0}\n", detailsURL);
      touristMessage.AppendLine();
      touristMessage.AppendLine("Thank You,");
      touristMessage.AppendLine();
      touristMessage.AppendLine(FromEmailName);
      touristMessage.AppendLine("McIntire Tour Coordinator");

      SendEmail(touristMessage.ToString(), "New McIntire Tour Registration", recipients);
    }


    public static void SendTouristCancelMessage(McIntireTourist tourist)
    {
      List<string> recipients = new List<string> { tourist.Email };

      if ((tourist.Tour != null) && (tourist.Tour.PrimaryAmbassador != null) && (!String.IsNullOrWhiteSpace(tourist.Tour.PrimaryAmbassador.Email)))
      {
        recipients.Add(tourist.Tour.PrimaryAmbassador.Email);
      }

      StringBuilder cancelMessage = new StringBuilder();
      cancelMessage.AppendFormat("{0},\n", tourist.FirstName);
      cancelMessage.AppendLine();

      if ((tourist.Tour != null))
      {
        cancelMessage.AppendFormat(
          "This message is to confirm cancellation of your McIntire Tour, which was scheduled for {0}\n",
          tourist.Tour.StartTime);
      }
      else
      {
        cancelMessage.AppendLine("This message is to confirm cancellation of your McIntire Tour.");
      }

      cancelMessage.AppendLine();
      cancelMessage.AppendLine("Thank You,");
      cancelMessage.AppendLine();
      cancelMessage.AppendLine(FromEmailName);
      cancelMessage.AppendLine("McIntire Tour Coordinator");

      SendEmail(cancelMessage.ToString(), "McIntire Tour Cancellation", recipients);
    }


    public static DateTime GetAcademicYearBegin(int year)
    {
      return new DateTime(year, 8, 20);
    }


    public static DateTime GetAcademicYearEnd(int year)
    {
      return new DateTime(year + 1, 8, 20);
    }

    public static DateTime CurrentAcademicYearBegin
    {
      get
      {
        // if the current date is between January 1st and August 20th, use the year that starts the academic year.
        if ((DateTime.Now.Month < 8) || ((DateTime.Now.Month == 8) && (DateTime.Now.Day < 20)))
        {
          return new DateTime(DateTime.Now.Year - 1,8,20);
        }
        return new DateTime(DateTime.Now.Year, 8, 20);
      }
    }
  }
}