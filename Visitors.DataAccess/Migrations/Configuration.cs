using System;
using System.Data.Entity.Migrations;
using Visitors.Domain;

namespace Visitors.DataAccess.Migrations
{
  internal sealed class Configuration : DbMigrationsConfiguration<VisitorContext>
  {
    public Configuration()
    {
      AutomaticMigrationsEnabled = true;
    }

    protected override void Seed(VisitorContext context)
    {
      //  This method will be called after migrating to the latest version.

      //DateTime morningVisitStart = DateTime.Parse("2013/08/12 09:30:00");
      //DateTime morningVisitEnd = DateTime.Parse("2013/08/12 11:30:00");

      //DateTime afternoonVisitStart = DateTime.Parse("2013/08/12 14:00:00");
      //DateTime afternoonVisitEnd = DateTime.Parse("2013/08/12 16:30:00");

      //context.AcademicSubjects.AddOrUpdate(
      //  a => a.Name,
      //  new AcademicSubject { Name = "Accounting" },
      //  new AcademicSubject { Name = "Finance" },
      //  new AcademicSubject { Name = "Information Technology" },
      //  new AcademicSubject { Name = "Management" },
      //  new AcademicSubject { Name = "Marketing" },
      //  new AcademicSubject { Name = "Business Analytics" },
      //  new AcademicSubject { Name = "Entrepreneurship" },
      //  new AcademicSubject { Name = "Global Commerce" },
      //  new AcademicSubject { Name = "Quantitative Finance" },
      //  new AcademicSubject { Name = "Real Estate" }
      //  );

      //context.ClassVisitTimeSlots.AddOrUpdate(t => t.StartDateTime,
      //  new ClassVisitTimeSlot { StartDateTime = morningVisitStart, SeatsAvailable = 4, EndDateTime = morningVisitEnd},
      //  new ClassVisitTimeSlot { StartDateTime = afternoonVisitStart, SeatsAvailable = 4, EndDateTime = afternoonVisitEnd});

      //context.Tours.AddOrUpdate(t => t.StartTime,
      //  new McIntireTour { StartTime = morningVisitStart },
      //  new McIntireTour { StartTime = afternoonVisitStart });

      //context.Ambassadors.AddOrUpdate(a => a.UserName,
      //  new Ambassador { Active = true, FirstName = "Adrian", LastName = "Cannon", Email = "ac7y6@comm.virginia.edu", PhoneNumber = "434.555.1918", UserName = "ac7y6" },
      //  new Ambassador { Active = true, FirstName = "Ashley", LastName = "Fulbright", Email = "adf78@comm.virginia.edu", PhoneNumber = "434.555.1918", UserName = "adf78" },
      //  new Ambassador { Active = true, FirstName = "Jeremy", LastName = "Jensen", Email = "jkj7u@comm.virginia.edu", PhoneNumber = "434.555.1918", UserName = "jkj7u" },
      //  new Ambassador { Active = true, FirstName = "Stephanie", LastName = "Kilpatrick", Email = "sek8k@comm.virginia.edu", PhoneNumber = "434.555.1918", UserName = "sek8k" });
    }
  }
}