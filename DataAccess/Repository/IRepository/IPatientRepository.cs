using Models;
using Models.Responses;

namespace DataAccess.Repository.IRepository;

public interface IPatientRepository : IRepository<Patient> 
{
    void Update(Patient patient);
    Task<PatientResponse> GetPatientByEmail(string Search);
    Task<IEnumerable<PatientResponse>> GetClinicPatients(int clinicId);
}
