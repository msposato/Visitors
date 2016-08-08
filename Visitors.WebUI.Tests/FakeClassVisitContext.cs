using System;
using System.Linq;
using Visitors.Domain;

namespace Visitors.WebUI.Tests
{
  public class FakeClassVisitContext : IVisitorRepository
  {
    public IQueryable<ClassVisitor> GetVisitorsForAmbassabor(int ambassadorId)
    {
      throw new NotImplementedException();
    }

    public IQueryable<ClassVisitor> GetVisitorsForTimeSlot(int timeSlotId)
    {
      throw new NotImplementedException();
    }

    public void AddAcademicSubject(AcademicSubject academicSubject)
    {
      throw new NotImplementedException();
    }

    public IQueryable<ClassVisitTimeSlot> AvailableTimeSlots { get; private set; }

    public void Add<T>(T entity) where T : class
    {
      throw new NotImplementedException();
    }

    public IQueryable<T> Query<T>() where T : class
    {
      throw new NotImplementedException();
    }

    public void Remove<T>(T entity) where T : class
    {
      throw new NotImplementedException();
    }

    public T Find<T>(int id) where T : class
    {
      throw new NotImplementedException();
    }

    public void Save()
    {
      throw new NotImplementedException();
    }

    public void Update<T>(T entity) where T : class
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }


    public IQueryable<McIntireTourist> GetTouristsForTour(int tourId)
    {
      throw new NotImplementedException();
    }

    public IQueryable<McIntireTour> GetToursForAmbassabor(int ambassadorId)
    {
      throw new NotImplementedException();
    }

    public IQueryable<McIntireTour> UpcomingTours
    {
      get { throw new NotImplementedException(); }
    }


    public IQueryable<ClassVisitor> UpcomingVisitors
    {
      get { throw new NotImplementedException(); }
    }
  }
}