using Models;
using Models.Requests;
using Models.Responses;

namespace DentalManagementSystem.Services.Interfaces;
public interface IClinicServices
{
    Task<User> AddMember(string adminId, User member, string role);
    Task<User> AddPatinet(string memberId, PatientRequest patient);
    Task<bool> CreateAppointment(string memberId, AppointmentRequest appointment);
    Task<bool> CreateClinic(string adminId, Clinic clinic);
    Task<PatientResponse> FindPatientAsync(string search);
    Task<IEnumerable<AppointmentResponse>> GetAppointments(string userId);
    Task<Clinic> GetClinicByUserId(string userId);
    Task<IEnumerable<UserResponse>> GetClinicMembers(string userId);
    Task<IEnumerable<PatientResponse>> GetClinicPatients(string memberId);
    Task<bool> UpdateClinic(string adminId, Clinic clinic);
    Task UpdateAppointmentStatus(int appointmentId, string status);
    Task UpdateClinicMemberRole(string memberId, string role, string adminId);
    Task DeleteClinicMember(string memberId, string adminId);
}
