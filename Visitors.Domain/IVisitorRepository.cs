using System.Linq;
using McIntireEntities.Core;

namespace Visitors.Domain
{
  public interface IVisitorRepository : IRepository
  {
    IQueryable<McIntireTour> GetToursForAmbassabor(int ambassadorId);
    IQueryable<McIntireTourist> GetTouristsForTour(int tourId);
    IQueryable<ClassVisitor> GetVisitorsForAmbassabor(int ambassadorId);
    IQueryable<ClassVisitor> GetVisitorsForTimeSlot(int timeSlotId);
    IQueryable<ClassVisitTimeSlot> AvailableTimeSlots { get; }
    IQueryable<McIntireTour> UpcomingTours { get; }
    IQueryable<ClassVisitor> UpcomingVisitors { get; }
  }
}