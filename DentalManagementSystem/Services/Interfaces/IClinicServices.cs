using Models;
using Models.Requests;
using Models.Responses;

namespace DentalManagementSystem.Services.Interfaces;
public interface IClinicServices
{
    Task<User> AddMember(string adminId, MemberRequest request);
    Task<User> AddPatinet(string memberId, PatientRequest patient);
    Task<bool> CreateAppointment(string memberId, AppointmentRequest appointment);
    Task<bool> CreateClinic(string adminId, Clinic clinic);
    Task<PatientResponse> FindPatientAsync(string search);
    Task<IEnumerable<DoctorResponse>> GetClinicDoctors(string userId);
    Task<IEnumerable<AppointmentResponse>> GetAppointments(string userId);
    Task<Clinic> GetClinicByUserId(string userId);
    Task<IEnumerable<EmployeesResponse>> GetClinicMembers(string userId);
    Task<IEnumerable<PatientResponse>> GetClinicPatients(string memberId);
    Task<bool> UpdateClinic(string adminId, Clinic clinic);
    Task UpdateAppointmentStatus(int appointmentId, string status);
    Task UpdateClinicMember(MemberRequest data, string adminId);
    Task DeleteClinicMember(string memberId, string adminId);
    Task AddTreatmentPlan(TreatmentPlanRequest plan);
    Task AddPlanSession(int TreatmentId, PlanSessionRequest session);
    Task UpdateSession(int sessionId, PlanSessionRequest session);
    Task CompleteSession(int sessionId);
    Task DeleteSession(int sessionId);
    IEnumerable<TreatmentPlanResponse> GetTreatmentPlanAsync(int clinicId);
}
