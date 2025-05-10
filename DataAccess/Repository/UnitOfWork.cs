using DataAccess.Context;
using DataAccess.Repository;
using DataAccess.Repository.IRepository;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            User = new UserRepository(_db);
            Doctor = new DoctorRepository(_db);
            Notification = new NotificationRepository(_db);
            Patient = new PatientRepository(_db);
            Address = new AddressRepository(_db);
            Clinic = new ClinicRepository(_db);
            MedicalHistory = new MedicalHistoryRepository(_db);
            Allergic = new AllergicRepository(_db);
            Appointment = new AppointmentRepository(_db);
            Employee = new EmployeeRepository(_db);
            Post = new PostRepository(_db);
            Like = new LikeRepository(_db);
            Comment = new CommentRepository(_db);
            Inventory = new InventoryRepository(_db);
            Stock = new StockRepository(_db);
            Treatment = new TreatmentRepository(_db);
            Session = new SessionRepository(_db);
            Examination = new MedicalExaminationRepository(_db);
            MedicalScan = new MedicalScanRepository(_db);
            LabTest = new LabTestRepository(_db);
            Prescription = new PrescriptionRepository(_db);
            Diagnosis = new DiagnosisRepository(_db);
        }
        public IUserRepository User { get; private set; }
        public IDoctorRepository Doctor { get; private set; }
        public INotificationRepository Notification { get; private set; }
        public IPatientRepository Patient { get; private set; }
        public IAddressRepository Address { get; private set; }
        public IClinicRepository Clinic { get; private set; }
        public IMedicalHistoryRepository MedicalHistory { get; private set; }
        public IAllergicRepository Allergic { get; private set; }
        public IAppointmentRepository Appointment { get; private set; }
        public IEmployeeRepository Employee { get; private set; }
        public IPostRepository Post { get; private set; }
        public ILikeRepository Like { get; private set; }
        public ICommentRepository Comment { get; private set; }
        public IInventoryRepository Inventory { get; private set; }
        public IStockRepository Stock { get; private set; }
        public ITreatmentRepository Treatment { get; private set; }
        public ISessionRepository Session { get; private set; }
        public IMedicalExaminationRepository Examination { get; private set; }
        public IMedicalScanRepository MedicalScan { get; private set; }
        public ILabTestRepository LabTest { get; private set; }
        public IPrescriptionRepository Prescription { get; private set; }
        public IDiagnosisRepository Diagnosis { get; private set; }

        public async Task<bool> SaveAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }
        public bool SaveSync()
        {
            return _db.SaveChanges() > 0;
        }
    }
}
