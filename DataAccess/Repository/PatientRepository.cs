using Models;
using Models.Responses;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using DataAccess.Repository.IRepository;

namespace Repository
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly AppDbContext _db;
        public PatientRepository(AppDbContext db) : base(db) 
        {
            _db = db;
        }
        public void Update(Patient patient)
        {
            _db.Patients.Update(patient);
        }

        public async Task<PatientResponse> GetPatientByEmail(string search)
        {
            try
            {
                var query = from patient in _db.Patients
                            join user in _db.Users on patient.UserId equals user.Id
                            from address in _db.Addresses.Where(u => u.UserId == user.Id).DefaultIfEmpty()
                            from mdHistory in _db.MedicalHistories.Where(u => u.PatientId == patient.Id).DefaultIfEmpty()
                            from allgric in _db.Allergies.Where(u => u.PatientId == patient.Id).DefaultIfEmpty()
                            where user.Email == search
                            select new PatientResponse()
                            {
                                Id = patient.Id,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Email = user.Email,
                                ImageUrl = user.ImageUrl,
                                Phone = user.PhoneNumber,
                                Gender = user.Gender,
                                DateOfBirth = user.DateOfBirth,
                                Allergies = allgric != null? allgric.Name: null,
                                Country = address != null ? address.Country : null,
                                City = address != null ? address.City : null,
                                State = address != null ? address.State : null   ,
                                ZipCode = address != null ? address.PostalCode : null,
                                MedicalHistory = mdHistory != null? mdHistory.MedicalRecord: null,
                                CreatedAt = patient.CreatedAt,
                            };

                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException($"Error on ({nameof(GetPatientByEmail)}) message: {ex.Message}");
            }
        }

        public async Task<IEnumerable<PatientResponse>> GetClinicPatients(int clinicId)
        {
            var query = from patient in _db.Patients
                        join user in _db.Users on patient.UserId equals user.Id
                        join address in _db.Addresses on user.Id equals address.UserId
                        join allergic in _db.Allergies on patient.Id equals allergic.PatientId
                        join medicalHistory in _db.MedicalHistories on patient.Id equals medicalHistory.PatientId
                        from appintment in _db.Appointments.Where(u => u.PatientId == patient.Id).DefaultIfEmpty()
                        where patient.ClinicId == clinicId
                        select new PatientResponse
                        {
                            Id = patient.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            ImageUrl = user.ImageUrl,
                            DateOfBirth = user.DateOfBirth,
                            Email = user.Email,
                            Gender = user.Gender,
                            Phone = user.PhoneNumber,
                            Country = address.Country,
                            City = address.City,
                            State = address.State,
                            ZipCode = address.PostalCode,
                            Allergies = allergic.Name,
                            MedicalHistory = medicalHistory.MedicalRecord,
                            StartTime = appintment != null ? appintment.StartTime : null,
                            EndTime = appintment != null? appintment.EndTime: null,
                            NextAppointment = appintment != null ? appintment.AppointmentDate : null,
                            LastVisit = appintment != null ? appintment.LastVisitDate : null,
                            Status = appintment != null ? appintment.Status : null,
                            CreatedAt = patient.CreatedAt,
                        };

            return await query.ToListAsync();
        }
    }
}
