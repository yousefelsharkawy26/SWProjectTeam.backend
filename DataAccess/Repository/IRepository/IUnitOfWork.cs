namespace DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IDoctorRepository Doctor { get; }
        IAppointmentRepository Appointment { get; }
        INotificationRepository Notification { get; }
        IPatientRepository Patient { get; }
        IAddressRepository Address { get; }
        IClinicRepository Clinic { get; }
        IMedicalHistoryRepository MedicalHistory { get; }
        IAllergicRepository Allergic { get; }
        IEmployeeRepository Employee { get; }
        IPostRepository Post { get; }
        ILikeRepository Like { get; }
        ICommentRepository Comment { get; }
        Task<bool> SaveAsync();
        bool SaveSync();
    }
}
