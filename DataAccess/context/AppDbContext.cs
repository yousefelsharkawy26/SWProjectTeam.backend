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
        builder.Entity<User>().HasData(new[] {
            new User()
            {
                Id = "af5f457a-26a5-427a-a8a6-b9b753e6029d",
                FirstName = "test",
                LastName = "test",
                AccessFailedCount = 0,
                Email = "test@example.com",
                Bio = "test",
                ConcurrencyStamp = "5390e3a6-de03-4879-b1cd-c403596597a2",
                SecurityStamp = "COI37GAHSBNR4AJNZTXA45UK3FEVR4V4",
                DateOfBirth = new DateOnly(1999,1,1),
                Gender = "male",
                ImageUrl = "avatar.png",
                IsActive = true,
                NormalizedEmail = "test@example.com".ToUpper(),
                NormalizedUserName = "test".ToUpper(),
                UserName = "test",
                Permission = "user",
                PasswordHash = "AQAAAAIAAYagAAAAEIowYWkpRvuQUAgJ5ZWf3jMQnmqVqNJ2Cm3mffstiAmWbcDZhOtOsFRxgExT61mkPw==",
                PhoneNumber = "12345678910",
                EmailConfirmed = false,
                LockoutEnabled = false,
                LockoutEnd  = null,
                PhoneNumberConfirmed = false,
                SubscriptionId = null,
                TwoFactorEnabled = false,
                Subscription = null
            },
            new User()
            {
                Id = "88b5eb92-7aca-40ff-95f4-f8c61f2addea",
                FirstName = "test",
                LastName = "test",
                AccessFailedCount = 0,
                Email = "test@example.com",
                Bio = "test",
                ConcurrencyStamp = "eff47228-f28c-46d3-a129-7bd7df09a8c6",
                SecurityStamp = "2NCD5KHCJLL25PJXWKOK43FMJ2K3OGGW",
                DateOfBirth = new DateOnly(1999,1,1),
                Gender = "male",
                ImageUrl = "avatar.png",
                IsActive = true,
                NormalizedEmail = "admin@example.com".ToUpper(),
                NormalizedUserName = "test".ToUpper(),
                UserName = "test",
                Permission = "administrator",
                PasswordHash = "AQAAAAIAAYagAAAAEHQkRgYUYhXG2tdW333iIeZ2t1zVKpMfD/cUBaCPIBRSfjUhM16T/AlvqJeFSdtmAA==",
                PhoneNumber = "12345678910",
                EmailConfirmed = false,
                LockoutEnabled = false,
                LockoutEnd  = null,
                PhoneNumberConfirmed = false,
                SubscriptionId = null,
                TwoFactorEnabled = false,
                Subscription = null
            },
            new User()
            {
                Id = "cbb82852-6786-4ad0-96ca-4b6c66e68102",
                FirstName = "test",
                LastName = "test",
                AccessFailedCount = 0,
                Email = "dentist@example.com",
                Bio = "test",
                ConcurrencyStamp = "00a994af-6313-4e5d-8833-0214da76ac40",
                SecurityStamp = "L7IN774BE27RT2PT7HOZR6BC7AUEZA2V",
                DateOfBirth = new DateOnly(1999,1,1),
                Gender = "male",
                ImageUrl = "avatar.png",
                IsActive = true,
                NormalizedEmail = "test@example.com".ToUpper(),
                NormalizedUserName = "test".ToUpper(),
                UserName = "test",
                Permission = "user",
                PasswordHash = "AQAAAAIAAYagAAAAEOpyv96AZ/rBPK4UVHMpVhmJJCV2vkvbotV1rItc168YMFSDMrR/NIK8H8iwL9hHVw==",
                PhoneNumber = "12345678910",
                EmailConfirmed = false,
                LockoutEnabled = false,
                LockoutEnd  = null,
                PhoneNumberConfirmed = false,
                SubscriptionId = null,
                TwoFactorEnabled = false,
                Subscription = null
            }
        });
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
                WorkingDate = "today 24 hours",
                OwnerId = "88b5eb92-7aca-40ff-95f4-f8c61f2addea"
            }
        });
        builder.Entity<Employee>().HasData(new[] {
            new Employee()
            {
                UserId = "88b5eb92-7aca-40ff-95f4-f8c61f2addea",
                Id = 1,
                ClinicId = 1,
                CreatedAt = DateTime.Now,
                Status = "on active",
                Specialization = "General Dentist",
                Name = "Admin User",
                Phone = "01015236589"
            },
            new Employee()
            {
                UserId = "cbb82852-6786-4ad0-96ca-4b6c66e68102",
                Id = 2,
                ClinicId = 1,
                CreatedAt = DateTime.Now,
                Status = "on active",
                Specialization = "General Dentist",
                Name = "Dentist User",
                Phone = "01015236589"
            }
        });
        base.OnModelCreating(builder);
    }
}