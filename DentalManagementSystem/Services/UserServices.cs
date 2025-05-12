using Models;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DentalManagementSystem.Services.Interfaces;
using Models.Responses;
using DataAccess.Repository.IRepository;
using Models.Requests;

namespace DentalManagementSystem.Services;
public class UserServices : IUserServices
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailServices _emailServices;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileServices _fileServices;
    public UserServices(UserManager<User> userManager,
                        IUnitOfWork unitOfWork,
                        IEmailServices emailServices,
                        IFileServices fileServices)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _emailServices = emailServices;
        _fileServices = fileServices;
    }

    public async Task<User> CreateUser(User user)
    {
        string password = GenerateTemporaryPassword();
        user.UserName = await GenerateRandomUserName();
        await _userManager.CreateAsync(user, password);
        user = await _userManager.FindByEmailAsync(user.Email);
        await _userManager.AddToRoleAsync(user, "user");

        try
        {
            //if (user.FirstName != null)
            //    await _emailServices.SendConfirmationEmail(user.Email, password, user.FirstName);
            //else
            //    await _emailServices.SendConfirmationEmail(user.Email, password, user.Email.Substring(0, user.Email.IndexOf('@')));

        }
        catch (Exception ex)
        {
            throw new SmtpException($"Send Mail error: {ex.Message}");
        }

        return user;
    }

    public IEnumerable<UserResponse> GetUserByName(string userName)
    {
        return _unitOfWork.User.SearchUsersByName(userName);
    }

    public async Task<UserResponse> GetUserDetails(string userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

        var userDetails = new UserResponse()
        {
            Name = user.FirstName + ' ' + user.LastName,
            Email = user.Email,
            Phone = user.PhoneNumber,
            ImageUrl = user.ImageUrl != null ? user.ImageUrl : "avatar.png",
            Bio = user.Bio,
            Permission = user.Permission,
            Id = user.Id
        };

        return userDetails;
    }
    public async Task<bool> UpdateUser(string userId, UserResponse userDetails)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

        var name = userDetails.Name.Split(' ');
        user.FirstName = name[0];
        user.LastName = name[1];
        user.Email = userDetails.Email;
        user.PhoneNumber = userDetails.Phone;
        user.Bio = userDetails.Bio;
        user.PhoneNumber = userDetails.Phone;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    private string GenerateTemporaryPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public async Task<string> GenerateRandomUserName()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random rand = new Random();
        var username = new string(Enumerable.Repeat(chars, 8)
        .Select(s => s[rand.Next(s.Length)]).ToArray());

        if (await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username) != null)
            username = await GenerateRandomUserName();

        return username;
    }

    public IEnumerable<UserResponse> FindUsersByEmailOrUserName(string search)
    {
        var users = _unitOfWork.User.FindUsersByEmailOrUserName(search);

        return users;
    }

    public User PatientRequestToUser(PatientRequest patient)
    {
        // In future add validation
        var dt = patient.DateOfBirth.Value;
        return new()
        {
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Email = patient.Email,
            DateOfBirth = new DateOnly(dt.Year, dt.Month, dt.Day),
            Gender = patient.Gender,
            PhoneNumber = patient.Phone,
        };
    }

    public async Task<IEnumerable<MedicalRecordResponse>> GetPatientMedicalRecords(int patientId)
    {
        var patient = await _unitOfWork.Patient.Get(u => u.Id == patientId, includeProp: "User");
        if (patient == null)
            throw new ArgumentNullException(nameof(patient));

        

        var medicalRecord = new List<MedicalRecordResponse>();
        var examination = await _unitOfWork.Examination.Get(u => u.PatientId == patientId);
        if (examination != null)
        {
            var medicalScans = _unitOfWork.MedicalScan
                .GetAll(u => u.ExaminationId == examination.Id)
                .ToList()
                .Select(s => {
                    var e = _unitOfWork.Examination.Get(e => e.Id == examination.Id, includeProp: "Doctor").Result;

                    return new MedicalRecordResponse()
                    {
                        CreatedAt = s.CreateAt,
                        Title = "Medical Scan",
                        Description = s.Notes,
                        PatientId = patientId,
                        Type = new { FileUrl = s.FileUrl, Status = s.Status, ScanType = s.ScanType },
                        Id = s.Id,
                        Doctor = e.Doctor.Name,
                    };
                });

            var labTests = _unitOfWork.LabTest
                .GetAll(u => u.ExaminationId == examination.Id)
                .ToList()
                .Select(s => {
                    var e = _unitOfWork.Examination.Get(e => e.Id == examination.Id, includeProp: "Doctor").Result;

                    return new MedicalRecordResponse()
                    {
                        CreatedAt = s.CreateAt,
                        Title = "Lab Test",
                        Description = s.Notes,
                        PatientId = patientId,
                        Type = new { FileUrl = s.FileUrl, Status = s.Status, TestName = s.TestName },
                        Id = s.Id,
                        Doctor = e.Doctor.Name,
                    };
                });
            
            var prescriptions = _unitOfWork.Prescription
                .GetAll(u => u.ExaminationId == examination.Id)
                .ToList()
                .Select(s => {
                    var e = _unitOfWork.Examination.Get(e => e.Id == examination.Id, includeProp: "Doctor").Result;

                    return new MedicalRecordResponse()
                    {
                        CreatedAt = s.CreateAt,
                        Title = "Prescription",
                        Description = s.Instructions,
                        PatientId = patientId,
                        Type = new { Instructions = s.Instructions, Duration = s.Duration },
                        Id = s.Id,
                        Doctor = e.Doctor.Name,
                    };
                });

            var diagnoses = _unitOfWork.Diagnosis
                .GetAll(u => u.ExaminationId == examination.Id)
                .ToList()
                .Select(s => {
                    var e = _unitOfWork.Examination.Get(e => e.Id == examination.Id, includeProp: "Doctor").Result;

                    return new MedicalRecordResponse()
                    {
                        CreatedAt = s.CreatedAt,
                        Title = "Medical Scan",
                        Description = s.Description,
                        PatientId = patientId,
                        Type = null,
                        Id = s.Id,
                        Doctor = e.Doctor.Name,
                    };
                });

            if (medicalScans.Any())
                medicalRecord.AddRange(medicalScans);
            if (labTests.Any())
                medicalRecord.AddRange(labTests);
            if (prescriptions.Any())
                medicalRecord.AddRange(prescriptions);
            if (diagnoses.Any())
                medicalRecord.AddRange(diagnoses);
        }

        

        return medicalRecord;
    }

    public async Task AddMedicalExamination(MedicalExaminationRequest examination)
    {
        var dbExamination = await _unitOfWork.Examination.Get(u => u.PatientId == examination.PatientId);

        if (dbExamination == null)
        {
            dbExamination = new()
            {
                PatientId = examination.PatientId,
                DoctorId = examination.Doctor
            };

            await _unitOfWork.Examination.Add(dbExamination);

            await _unitOfWork.SaveAsync();
        }

        _ = examination.Type switch
        {
            "test" => _unitOfWork.LabTest.Add(new()
            {
                CreateAt = DateTime.Now,
                ExaminationId = dbExamination.Id,
                FileUrl = _fileServices.UploudFile(examination.File, null),
                Notes = examination.Description,
                Status = "Added",
                TestName = examination.Title,
            }),
            "scan" => _unitOfWork.MedicalScan.Add(new()
            {
                CreateAt = DateTime.Now,
                ExaminationId = dbExamination.Id,
                FileUrl = _fileServices.UploudFile(examination.File, null),
                Notes = examination.Description,
                Status = "Added",
                ScanType = examination.Title,
            }),
            _ => throw new ArgumentException("Type Error")
        };

        await _unitOfWork.SaveAsync();
    }
}
