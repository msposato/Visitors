using System;
using System.Data.Entity;
using Visitors.DataAccess;
using Visitors.Domain;

namespace LoadClassVisits
{
  class Program
  {
    static void Main()
    {
	    //return;

      const int year = 2016;
      DateTime startDate = new DateTime(year, 9, 5);
      DateTime endDate = new DateTime(year, 11, 21);

			using (DbContext visitorContext = new VisitorContext())
      {
        for (DateTime thisDate = startDate; thisDate <= endDate; thisDate = thisDate.AddDays(1))
        {
          // no class visits during spring break
          DateTime breakStart = new DateTime(year, 10, 3);
          DateTime breakEnd = new DateTime(year, 10, 4);
          if ((thisDate >= breakStart) && (thisDate <= breakEnd))
          {
            continue;
          }


          if ((thisDate.DayOfWeek == DayOfWeek.Monday) ||
              (thisDate.DayOfWeek == DayOfWeek.Tuesday) ||
              (thisDate.DayOfWeek == DayOfWeek.Wednesday) ||
              (thisDate.DayOfWeek == DayOfWeek.Thursday))
          {
            // In the fall term on Monday thru Thursday there are four time slots: 
            // 9:30AM - 10:45AM (1 spaces)
            // 11:00AM - 12:15PM (2 spaces)
            // 2:00PM - 3:15PM (1 spaces)
            // 3:30PM - 4:45PM (1 space)

            // in the spring term on Monday thru Thursday there are three time slots: 
            // 9:30AM - 10:45AM (2 spaces)
            // 11:00AM - 12:15PM (2 spaces)
            // 2:00PM - 3:15PM (2 spaces)

						Console.WriteLine("Adding class visits on {0}", thisDate.ToShortDateString());

            //ClassVisitTimeSlot slot1 = new ClassVisitTimeSlot
            //{
            //  StartDateTime = new DateTime(year, thisDate.Month, thisDate.Day, 8, 0, 0),
            //  EndDateTime = new DateTime(year, thisDate.Month, thisDate.Day, 9, 15, 0),
            //  SeatsAvailable = 2,
            //  SeatsRemaining = 2
            //};
            //visitorContext.Set<ClassVisitTimeSlot>().Add(slot1);

            ClassVisitTimeSlot slot2 = new ClassVisitTimeSlot
              {
                StartDateTime = new DateTime(year, thisDate.Month, thisDate.Day, 9, 30, 0),
                EndDateTime = new DateTime(year, thisDate.Month, thisDate.Day, 10, 45, 0),
                SeatsAvailable = 1,
                SeatsRemaining = 1
              };
            visitorContext.Set<ClassVisitTimeSlot>().Add(slot2);

            ClassVisitTimeSlot slot3 = new ClassVisitTimeSlot
            {
              StartDateTime = new DateTime(year, thisDate.Month, thisDate.Day, 11, 0, 0),
              EndDateTime = new DateTime(year, thisDate.Month, thisDate.Day, 12, 15, 0),
              SeatsAvailable = 2,
              SeatsRemaining = 2
            };
            visitorContext.Set<ClassVisitTimeSlot>().Add(slot3);

            //ClassVisitTimeSlot slot4 = new ClassVisitTimeSlot
            //{
            //  StartDateTime = new DateTime(year, thisDate.Month, thisDate.Day, 12, 30, 0),
            //  EndDateTime = new DateTime(year, thisDate.Month, thisDate.Day, 1, 45, 0),
            //  SeatsAvailable = 2,
            //  SeatsRemaining = 2
            //};
            //visitorContext.Set<ClassVisitTimeSlot>().Add(slot4);

            ClassVisitTimeSlot slot5 = new ClassVisitTimeSlot
            {
              StartDateTime = new DateTime(year, thisDate.Month, thisDate.Day, 14, 0, 0),
              EndDateTime = new DateTime(year, thisDate.Month, thisDate.Day, 15, 15, 0),
              SeatsAvailable = 1,
              SeatsRemaining = 1
            };
            visitorContext.Set<ClassVisitTimeSlot>().Add(slot5);

						ClassVisitTimeSlot slot6 = new ClassVisitTimeSlot
						{
							StartDateTime = new DateTime(year, thisDate.Month, thisDate.Day, 15, 30, 0),
							EndDateTime = new DateTime(year, thisDate.Month, thisDate.Day, 16, 45, 0),
							SeatsAvailable = 1,
							SeatsRemaining = 1
						};
						visitorContext.Set<ClassVisitTimeSlot>().Add(slot6);
          }
        }
        visitorContext.SaveChanges();
      }

      Console.WriteLine("Finished");
      Console.ReadKey();
    }
  }
}
