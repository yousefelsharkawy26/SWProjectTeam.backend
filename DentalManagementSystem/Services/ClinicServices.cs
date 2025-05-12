using Models;
using Models.Requests;
using Models.Responses;
using Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccess.Repository.IRepository;
using DentalManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;

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

    public async Task<User> AddMember(string adminId, MemberRequest request)
    {
        var admin = await _unitOfWork.Employee.Get(u => u.UserId == adminId);

        var member = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (member == null)
            throw new ArgumentNullException("Email is wrong.");

        var newMember = await _unitOfWork.Employee.Get(u => u.UserId == member.Id && u.ClinicId == admin.ClinicId);
        
        if (newMember != null)
            throw new ArgumentException("User is already exists in clinic");

        await _userManager.RemoveFromRoleAsync(member, member.Permission);
        await _userManager.AddToRoleAsync(member, request.Role);

        if (request.Role == Utility.Dentist_Role)
            await _unitOfWork.Doctor.Add(new()
            {
                Employee = new Employee()
                {
                    ClinicId = admin.ClinicId,
                    UserId = member.Id,
                    Status = "active",
                    CreatedAt = DateTime.Now,
                    Specialization = request.Specialization,
                    Name = request.FirstName + ' ' + request.LastName,
                    Phone = request.Phone,
                },
                Name = member.FirstName + " " + member.LastName,
            });
        else
            await _unitOfWork.Employee.Add(new()
            {
                ClinicId = admin.ClinicId,
                UserId = member.Id,
                Status = "active",
                CreatedAt = DateTime.Now,
                Specialization = request.Specialization,
                Name = request.FirstName + ' ' + request.LastName,
                Phone = request.Phone,
            });

        member.Permission = request.Role;

        try
        {
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
        var doc = await _unitOfWork.Doctor.Get(u => u.Id == int.Parse(appointment.DentistId), includeProp: "Employee");
        newAppointment.DoctorId = doc.Employee.UserId;
        newAppointment.Notes = appointment.Notes;
        newAppointment.ClinicId = emp.ClinicId;
        newAppointment.LastVisitDate = null;

        await _unitOfWork.Appointment.Add(newAppointment);

        try
        {
            var result = await _unitOfWork.SaveAsync();

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
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
    public async Task<IEnumerable<DoctorResponse>> GetClinicDoctors(string userId)
    {
        var emp = await _unitOfWork.Employee.Get(u => u.UserId == userId);

        var doctors = _unitOfWork.Doctor
            .GetAll(u => u.Employee.ClinicId == emp.ClinicId, includeProp: "Employee")
            .ToList()
            .Select(i => new DoctorResponse()
            {
                Id = i.Id,
                Name = i.Name,
                Role = i.Specialization,
            });

        if (doctors == null) throw new ArgumentNullException("There is no doctors in clinic");

        return doctors;
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
            clinic = (await _unitOfWork.Employee.Get(u => u.UserId == userId, includeProp: "Clinic")).Clinic;
        
        if (clinic == null)
            throw new ArgumentNullException("Clinic not exists");

        return clinic;
    }
    public async Task<IEnumerable<EmployeesResponse>> GetClinicMembers(string userId)
    {
        var emp = await _unitOfWork.Employee.Get(u => u.UserId == userId);

        if (emp == null)
            throw new ArgumentNullException("You doesn't have a clinic.");

        var employees = _unitOfWork.Employee
            .GetAll(u => u.ClinicId == emp.ClinicId, includeProp: "User")
            .Select(member => new EmployeesResponse()
            {
                Id = member.UserId,
                Name = member.Name,
                Email = member.User.Email,
                Image = member.User.ImageUrl,
                Role = member.User.Permission,
                Phone = member.Phone,
                CreateAt = new DateOnly(member.CreatedAt.Year, member.CreatedAt.Month, member.CreatedAt.Day),
                Specialization = member.Specialization,
                status = member.Status
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
    public async Task UpdateClinicMember(MemberRequest data, string adminId)
    {
        var admin = await _unitOfWork.Employee.Get(u => u.UserId == adminId);

        var employee = await _unitOfWork.Employee
            .Get(u => u.UserId == data.Id && u.ClinicId == admin.ClinicId, "User", traced: true);

        if (employee == null)
            throw new UnauthorizedAccessException();

        var member = employee.User;
        employee.Specialization = data.Specialization;
        employee.Name = data.FirstName + ' ' + data.LastName;
        employee.Phone = data.Phone;

        try
        {
            await _userManager.RemoveFromRoleAsync(member, member.Permission);
            await _userManager.AddToRoleAsync(member, data.Role);
            member.Permission = data.Role;
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

    public async Task AddTreatmentPlan(TreatmentPlanRequest plan)
    {
        await _unitOfWork.Session.Add(new()
        {
            Completed = false,
            Date = new DateOnly(plan.StartDate.Year, plan.StartDate.Month, plan.StartDate.Day),
            Notes = plan.Notes,
            TreatmentPlan = new()
            {
                Description = plan.Notes,
                Cost = plan.Cost,
                PatientId = plan.PatientId,
                DoctorId = plan.DentistId,
                Type = plan.TreatmentType,
            }
        });

        await _unitOfWork.SaveAsync();
    }
    public async Task AddPlanSession(int TreatmentId, PlanSessionRequest session)
    {
        await _unitOfWork.Session.Add(new()
        {
            Completed = false,
            Date = new DateOnly(session.Date.Year, session.Date.Month, session.Date.Day),
            Notes = session.Notes,
            TreatmentId = TreatmentId,
        });

        await _unitOfWork.SaveAsync();
    }
    public async Task UpdateSession(int sessionId, PlanSessionRequest session)
    {
        var dbSession = await _unitOfWork.Session.Get(u => u.Id == sessionId);
        dbSession.Notes = session.Notes;
        dbSession.Date = new DateOnly(session.Date.Year, session.Date.Month, session.Date.Day);

        _unitOfWork.Session.Update(dbSession);

        await _unitOfWork.SaveAsync();
    }
    public async Task CompleteSession(int sessionId)
    {
        var session = await _unitOfWork.Session.Get(u => u.Id == sessionId);
        session.Completed = true;

        _unitOfWork.Session.Update(session);

        await _unitOfWork.SaveAsync();
    }
    public async Task DeleteSession(int sessionId)
    {
        var session = await _unitOfWork.Session.Get(u => u.Id == sessionId);

        _unitOfWork.Session.Delete(session);

        await _unitOfWork.SaveAsync();
    }
    public IEnumerable<TreatmentPlanResponse> GetTreatmentPlanAsync(int clinicId)
    {
        var plans = _unitOfWork.Treatment
            .GetAll(u => u.Patient.ClinicId == clinicId, includeProp: "Patient,Doctor")
            .ToList()
            .Select(u => {
                var patient = _unitOfWork.Patient.Get(p => p.Id == u.PatientId, includeProp: "User").Result;
                var sessions = _unitOfWork.Session
                    .GetAll(s => s.TreatmentId == u.Id)
                    .ToList()
                    .Select(s => new PlanSessionResponse()
                    {
                        Id = s.Id,
                        Completed = s.Completed,
                        Date = s.Date,
                        Notes = s.Notes,
                    });

                return new TreatmentPlanResponse()
                {
                    Id = u.Id,
                    Cost = u.Cost,
                    DentistName = u.Doctor.Name,
                    Notes = u.Description,
                    PatientId = u.PatientId,
                    PatientName = patient.User.FirstName + ' ' + patient.User.LastName,
                    TreatmentType = u.Type,
                    Sessions = sessions,
                    StartDate = sessions.Select(p => p.Date).Min(),
                    EndDate = sessions.Select(p => p.Date).Max(),
                    Status = !sessions.Select(s => s.Completed).FirstOrDefault(u => u == false) ? "in-progress" : "completed",
                };
            });

        return plans;
    }

    
}
