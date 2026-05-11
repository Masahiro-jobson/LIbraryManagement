using Microsoft.EntityFrameworkCore;
using LibraryMainApp.Models;

namespace LibraryMainApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        // DbSet maps each cs file to a table in the database.
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        // The code procided below is to avoid entirely removing the related data when the main data is deleted..
        //OnModeCreating is the inherited method from the parent class DbContext.
        //override is telling app to use the custom method instead of the default.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            // modeBuilder.Model.GetEntityTypes check each entity created in the database.
            //SelectMany(e => e.GetForeignKeys()) find each foreing key in the each entity.
            // relationship.DeleteBehavior = DeleteBehavior.Restrict; set rule not to delete each foreign key but to restrict for each foreign key.
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

    }
}
