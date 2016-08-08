using System;
using System.Data.Entity;
using System.Linq;
using Visitors.Domain;

namespace Visitors.DataAccess
{
  public class VisitorContext : DbContext, IVisitorRepository
  {
    public VisitorContext()
      : base("Visitors")
    {
    }

    public DbSet<AcademicSubject> AcademicSubjects { get; set; }
    public DbSet<ClassVisitor> Visitors { get; set; }
    public DbSet<Ambassador> Ambassadors { get; set; }
    public DbSet<ClassVisitTimeSlot> ClassVisitTimeSlots { get; set; }
    public DbSet<McIntireTour> Tours { get; set; }
    public DbSet<McIntireTourist> Tourists { get; set; }

    public IQueryable<T> Query<T>() where T : class
    {
      return Set<T>();
    }

    public void Add<T>(T entity) where T : class
    {
      Set<T>().Add(entity);
    }

    public void Update<T>(T entity) where T : class
    {
      Entry(entity).State = EntityState.Modified;
    }

    public void Remove<T>(T entity) where T : class
    {
      Set<T>().Remove(entity);
    }

    public T Find<T>(int id) where T : class
    {
      return Set<T>().Find(id);
    }

    public void Save()
    {
      SaveChanges();
    }

    public IQueryable<ClassVisitor> GetVisitorsForAmbassabor(int ambassadorId)
    {
      var visits = Visitors.Where(v => v.AmbassadorId == ambassadorId);
      return visits;
    }

    public IQueryable<ClassVisitor> GetVisitorsForTimeSlot(int timeSlotId)
    {
      var visits = Visitors.Where(v => (v.PreferredTimeSlotId == timeSlotId) || (v.AlternateTimeSlotId == timeSlotId));
      return visits;
    }

    public IQueryable<ClassVisitTimeSlot> AvailableTimeSlots
    {
      get
      {
        DateTime twoDaysFromNow = DateTime.Now.AddDays(2);
        var timeSlots = ClassVisitTimeSlots.Where(t => (t.SeatsRemaining > 0) && (t.StartDateTime >= twoDaysFromNow));
        return timeSlots;
      }
    }

    public IQueryable<McIntireTour> GetToursForAmbassabor(int ambassadorId)
    {
      var tours = Tours.Where(t => (t.PrimaryAmbassadorId == ambassadorId) || (t.AssistantAmbassadorId == ambassadorId));
      return tours;
    }

    public IQueryable<McIntireTourist> GetTouristsForTour(int tourId)
    {
      var tours = Tourists.Where(t => t.TourId == tourId);
      return tours;
    }

    public IQueryable<McIntireTour> UpcomingTours
    {
      get
      {
        var upcomingTours = Tours.Where(t => t.StartTime >= DateTime.Now);
        return upcomingTours;
      }
    }


    public IQueryable<ClassVisitor> UpcomingVisitors
    {
      get
      {
        DateTime twoHoursAgo = DateTime.Now.AddHours(-2);

        var upcomingVisitors = Visitors.Where(v => v.PreferredTimeSlot.StartDateTime >= twoHoursAgo);

        if (upcomingVisitors.Any())
        {
          return upcomingVisitors.OrderBy(v => v.PreferredTimeSlot.StartDateTime);
        }

        return upcomingVisitors;
      }
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      // this code changes the default model which uses casacading deletes
      modelBuilder.Entity<Ambassador>().HasMany(a => a.Visitors).WithOptional(v => v.Ambassador).WillCascadeOnDelete(false);
      modelBuilder.Entity<Ambassador>().HasMany(a => a.PrimaryTours).WithOptional(v => v.PrimaryAmbassador).WillCascadeOnDelete(false);
      modelBuilder.Entity<Ambassador>().HasMany(a => a.AssistantTours).WithOptional(v => v.AssistantAmbassador).WillCascadeOnDelete(false);
      modelBuilder.Entity<ClassVisitTimeSlot>().HasMany(t => t.PrimaryVisitors).WithRequired(v => v.PreferredTimeSlot).WillCascadeOnDelete(false);
      modelBuilder.Entity<ClassVisitTimeSlot>().HasMany(t => t.AlternateVisitors).WithOptional(v => v.AlternateTimeSlot).WillCascadeOnDelete(false);
      base.OnModelCreating(modelBuilder);
    }

  }
}