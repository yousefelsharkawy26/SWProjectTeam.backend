using Models;
using Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace DataAccess.Context;

public partial class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) 
    {
        //if (Database.EnsureCreated())
            Database.Migrate();
    }

    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<Appointment> Appointments { get; set; }
    public virtual DbSet<Diagnosis> Diagnoses { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<Follow> Follows { get; set; }
    public virtual DbSet<LabTest> LabTests { get; set; }
    public virtual DbSet<MedicalExamination> MedicalExaminations { get; set; }
    public virtual DbSet<MedicalScan> MedicalScans { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<Patient> Patients { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<Like> Likes { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<Prescription> Prescriptions { get; set; }
    public virtual DbSet<Procedure> Procedures { get; set; }
    public virtual DbSet<Subscription> Subscriptions { get; set; }
    public virtual DbSet<Allergic> Allergies { get; set; }
    public virtual DbSet<MedicalHistory> MedicalHistories { get; set; }
    public virtual DbSet<Clinic> Clinics { get; set; }
    public virtual DbSet<User> Users {  get; set; }
    public virtual DbSet<Inventory> Inventories { get; set; }
    public virtual DbSet<Stock> Stocks { get; set; }
    public virtual DbSet<TreatmentPlan> Treatments { get; set; }
    public virtual DbSet<PlanSession> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(new[]
        {
            new IdentityRole() { Id = "1", Name = Utility.Admin_Role, NormalizedName = Utility.Admin_Role.ToUpper()},
            new IdentityRole() { Id = "2", Name = Utility.Dentist_Role, NormalizedName = Utility.Dentist_Role.ToUpper()},
            new IdentityRole() { Id = "3", Name = Utility.Assistant_Role, NormalizedName = Utility.Assistant_Role.ToUpper()},
            new IdentityRole() { Id = "4", Name = Utility.Receptionist_Role, NormalizedName = Utility.Receptionist_Role.ToUpper()},
            new IdentityRole() { Id = "5", Name = Utility.Hygienist_Role, NormalizedName = Utility.Hygienist_Role.ToUpper()},
            new IdentityRole() { Id = "6", Name = Utility.User_Role, NormalizedName = Utility.User_Role.ToUpper()},
        });
        builder.Entity<Clinic>().HasData(new[]
        {
            new Clinic() {
                Id = 1,
                Name = "DentalCare",
                ClinicEmail = "DentalCare.Customers@gmail.com",
                ClinicPhone = "01019292839",
                City = "Mansoura",
                Country = "Egypt",
                State = "Gamaa",
                PostalCode = "37111",
                WorkingDate = "today 24 hours"
            }
        });

        base.OnModelCreating(builder);
    }
}