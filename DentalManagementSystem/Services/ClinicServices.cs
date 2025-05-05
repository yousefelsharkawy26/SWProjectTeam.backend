using Models;
using Models.Requests;
using Models.Responses;
using Microsoft.AspNetCore.Identity;
using DataAccess.Repository.IRepository;
using DentalManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace DentalManagementSystem.Services;
public class ClinicServices : IClinicServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly IUserServices _userServices;
    public ClinicServices(IUnitOfWork unitOfWork
                        , IUserServices userServices
                        , UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _userServices = userServices;
    }

    public async Task<User> AddMember(string adminId, User member, string role)
    {
        var admin = await _unitOfWork.Employee.Get(u => u.UserId == adminId);

        var newMember = await _unitOfWork.Employee.Get(u => u.UserId == member.Id && u.ClinicId == admin.ClinicId);
        
        if (newMember != null)
            throw new ArgumentException("User is already exists in clinic");


        var newEmp = new Employee()
        {
            ClinicId = admin.ClinicId,
            UserId = member.Id,
        };


        await _userManager.RemoveFromRoleAsync(member, member.Permission);
        await _userManager.AddToRoleAsync(member, role);

        member.Permission = role;
        newEmp.UserId = member.Id;

        try
        {
            await _unitOfWork.Employee.Add(newEmp);
            await _userManager.UpdateAsync(member);
            await _unitOfWork.SaveAsync();
            return member;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Error: {ex.Message}");
        }
    }
    public async Task<User> AddPatinet(string memberId, PatientRequest patient)
    {
        var emp = await _unitOfWork.Employee.Get(u => u.UserId == memberId);

        var dbPatient = await _unitOfWork.Patient
            .Get(u => u.User.Email == patient.Email, includeProp: "User");

        if (dbPatient == null)
        {
            dbPatient = new();
            dbPatient.User = _userServices.PatientRequestToUser(patient);
            try
            {
                dbPatient.User = await _userServices.CreateUser(dbPatient.User);
            }
            catch { throw new DbUpdateException(); }

            if (ValidAddress(patient))
            {
                var address = new Address()
                {
                    Country = patient.Country,
                    City = patient.City,
                    PostalCode = patient.ZipCode,
                    State = patient.State,
                    UserId = dbPatient.User.Id,
                };
                await _unitOfWork.Address.Add(address);
            }
        }
        else
        {
            if (CheckPatinentIsExists(dbPatient, emp.ClinicId))
                throw new ArgumentException("Patient already exists in clinic.");

            if (dbPatient.User.PhoneNumber == null)
                dbPatient.User.PhoneNumber = patient.Phone;

            if (dbPatient.User.DateOfBirth == new DateOnly(1, 1, 1))
            {
                var dt = patient.DateOfBirth.Value;
                dbPatient.User.DateOfBirth = new DateOnly(dt.Year, dt.Month, dt.Day);
            }

            if (dbPatient.User.Gender == null)
                dbPatient.User.Gender = patient.Gender;

            var address = await _unitOfWork.Address.Get(u => u.UserId == dbPatient.User.Id);
            if (address == null && ValidAddress(patient))
            {
                address = new()
                {
                    Country = patient.Country,
                    City = patient.City,
                    PostalCode = patient.ZipCode,
                    State = patient.State,
                    UserId = dbPatient.User.Id,
                };
                await _unitOfWork.Address.Add(address);
            }
        }

        var medicalHistory = new MedicalHistory()
        {
            MedicalRecord = patient.MedicalHistory,
            Patient = new Patient()
            {
                UserId = dbPatient.User.Id,
                ClinicId = emp.ClinicId,
            }
        };

        try
        {
            await _unitOfWork.MedicalHistory.Add(medicalHistory);

            if (await _unitOfWork.SaveAsync())
            {
                await _unitOfWork.Allergic.Add(new() { PatientId = medicalHistory.PatientId, Name = patient.Allergies });
            }

            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Save Allergic Error: {ex.Message}");
        }

        return dbPatient.User;
    }
    public async Task<bool> CreateAppointment(string memberId, AppointmentRequest appointment)
    {
        int value;
        if (!int.TryParse(appointment.PatientId, out value))
            throw new InvalidCastException();

        var emp = await _unitOfWork.Employee.Get(u => u.UserId == memberId);
        var oldAppointment = await _unitOfWork.Appointment
            .Get(u => u.PatientId == value && u.Status != Utility.Appointment_Completed);

        if (oldAppointment != null)
            throw new InvalidOperationException("This patient have an appointment not completed.");

        var newAppointment = new Appointment();
        newAppointment.AddedBy_Id = memberId;
        newAppointment.PatientId = value;
        newAppointment.Status = "confirmed";
        newAppointment.AppointmentType = appointment.TreatmentType;
        var dt = appointment.Date;
        newAppointment.AppointmentDate = new(dt.Year, dt.Month, dt.Day);
        var startTime = appointment.StartTime.Split(':');
        var endTime = appointment.EndTime.Split(':');
        newAppointment.StartTime = new TimeOnly(int.Parse(startTime[0]), int.Parse(startTime[1]));
        newAppointment.EndTime = new TimeOnly(int.Parse(endTime[0]), int.Parse(endTime[1]));
        newAppointment.DoctorId = appointment.DentistId;
        newAppointment.Notes = appointment.Notes;
        newAppointment.ClinicId = emp.ClinicId;
        newAppointment.LastVisitDate = null;

        await _unitOfWork.Appointment.Add(newAppointment);

        var result = await _unitOfWork.SaveAsync();

        return result;
    }
    public async Task<bool> CreateClinic(string adminId, Clinic newClinic)
    {
        var clinic = await _unitOfWork.Clinic.Get(c => c.OwnerId == adminId);

        if (clinic != null)
            throw new ArgumentNullException($"{nameof(clinic)} Not Exists");

        newClinic.OwnerId = adminId;

        try
        {
            await _unitOfWork.Clinic.Add(newClinic);

            var result = await _unitOfWork.SaveAsync();
            return result;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Error {ex.Message}");
        }
    }
    public async Task<PatientResponse> FindPatientAsync(string search)
    {
        var patient = await _unitOfWork.Patient.GetPatientByEmail(search);
        if (patient != null)
            return patient;
        throw new ArgumentNullException("Patinet Not Found");
    }
    public async Task<IEnumerable<AppointmentResponse>> GetAppointments(string userId)
    {
        var emp = await _unitOfWork.Employee.Get(u => u.UserId == userId);
        var appointments = _unitOfWork.Appointment
            .GetAll(u => u.ClinicId == emp.ClinicId, includeProp: "Patient,Doctor").ToList();
        try
        {
            var appointRes = await GetAppointmentResponses(appointments);
            return appointRes;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Accessing error in (GetAppointments), {ex.Message}");
        }
    }
    public async Task<Clinic> GetClinicByUserId(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var clinic = await _unitOfWork.Clinic
            .Get(c => c.OwnerId == userId);

        if (clinic == null)
            throw new ArgumentNullException("Clinic not exists");

        return clinic;
    }
    public async Task<IEnumerable<UserResponse>> GetClinicMembers(string userId)
    {
        var emp = await _unitOfWork.Employee.Get(u => u.UserId == userId);

        if (emp == null)
            throw new ArgumentNullException("You doesn't have a clinic.");

        var employees = _unitOfWork.Employee
            .GetAll(u => u.ClinicId == emp.ClinicId, includeProp: "User")
            .Select(member => new UserResponse()
            {
                Id = member.UserId,
                FirstName = member.User.FirstName,
                LastName = member.User.LastName,
                Email = member.User.Email,
                Bio = member.User.Bio,
                ImageUrl = member.User.ImageUrl,
                Permission = member.User.Permission,
                Phone = member.User.PhoneNumber,
            });

        if (employees != null)
            return employees;

        throw new ArgumentNullException("There is no employees in clinic");
    }
    public async Task<IEnumerable<PatientResponse>> GetClinicPatients(string memberId)
    {
        var emp = await _unitOfWork.Employee.Get(u => u.UserId == memberId);
        if (emp == null)
            throw new ArgumentNullException("Clinic not found.");

        var patients = await _unitOfWork.Patient.GetClinicPatients(emp.ClinicId);

        return patients;
    }
    public async Task<bool> UpdateClinic(string adminId, Clinic clinic)
    {
        var pervClinic = await _unitOfWork.Clinic.Get(c => c.OwnerId == adminId);

        if (clinic == null)
            throw new ArgumentNullException($"Just owner can update {pervClinic.Name} data");

        pervClinic.Name = clinic.Name;
        pervClinic.ClinicPhone = clinic.ClinicPhone;
        pervClinic.ClinicEmail = clinic.ClinicEmail;
        pervClinic.Country = clinic.Country;
        pervClinic.City = clinic.City;
        pervClinic.State = clinic.State;
        pervClinic.PostalCode = clinic.PostalCode;
        pervClinic.WorkingDate = clinic.WorkingDate;

        try
        {
            _unitOfWork.Clinic.Update(clinic);
            var result = await _unitOfWork.SaveAsync();
            return result;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Error {ex.Message}");
        }
    }
    public async Task UpdateAppointmentStatus(int appointmentId, string status)
    {
        var appointment = await _unitOfWork.Appointment.Get(u => u.Id == appointmentId, traced: true);

        if (status == Utility.Appointment_Confirmed ||
            status == Utility.Appointment_In_progress ||
            status == Utility.Appointment_Completed ||
            status == Utility.Appointment_Cancelled)
        { 
            appointment.Status = status;
            try
            {
                _unitOfWork.Appointment.Update(appointment);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new DbUpdateException("Error in (UpdateAppointmentStatus) on ", ex);
            }
        }
        
    }
    public async Task UpdateClinicMemberRole(string memberId, string role, string adminId)
    {
        var emp = await _unitOfWork.Employee.Get(u => u.UserId == adminId);

        var memberClinic = await _unitOfWork.Employee
            .Get(u => u.UserId == memberId && u.ClinicId == emp.ClinicId, "User", traced: true);

        if (memberClinic == null)
            throw new UnauthorizedAccessException();

        var member = memberClinic.User;

        try
        {
            await _userManager.RemoveFromRoleAsync(member, member.Permission);
            await _userManager.AddToRoleAsync(member, role);
            member.Permission = role;
            await _userManager.UpdateAsync(member);
        }
        catch (Exception ex)
        {
            throw new DbUpdateException("Error in (UpdateClinicMemberRole) when updating user roles", ex);
        }
    }
    public async Task DeleteClinicMember(string memberId, string adminId)
    {
        var clinic = await _unitOfWork.Employee
            .Get(u => u.UserId == adminId);

        var clinicMemebr = await _unitOfWork.Employee
            .Get(u => u.ClinicId == clinic.ClinicId && u.UserId == memberId);

        try
        {
            _unitOfWork.Employee.Delete(clinicMemebr);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            throw new DbUpdateException(ex.Message, ex);
        }
    }


    private bool ValidAddress(PatientRequest patient)
    {
        if (!string.IsNullOrEmpty(patient.Country))
            return true;
        if (!string.IsNullOrEmpty(patient.City))
            return true;
        if (!string.IsNullOrEmpty(patient.State))
            return true;
        if (!string.IsNullOrEmpty(patient.ZipCode))
            return true;
        return false;
    }
    private bool CheckPatinentIsExists(Patient patient, int clinicId)
    => patient.ClinicId == clinicId;
    private async Task<IEnumerable<AppointmentResponse>> GetAppointmentResponses(IEnumerable<Appointment> appointments)
    {
        List<AppointmentResponse> lst = new List<AppointmentResponse>();
        foreach (var appointment in appointments)
        {
            var p = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == appointment.Patient.UserId);
            lst.Add(new()
            {
                Id = appointment.Id,
                Type = appointment.AppointmentType,
                Status = appointment.Status,
                Notes = appointment.Notes,
                Date = appointment.AppointmentDate,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                DentistName = appointment.Doctor.FirstName + ' ' + appointment.Doctor.LastName,
                PatientId = appointment.PatientId,
                PatientName = p.FirstName + ' ' + p.LastName,
                ImageUrl = p.ImageUrl,
                DentistId = appointment.DoctorId
            });
        }
        return lst;
    }

    
}
