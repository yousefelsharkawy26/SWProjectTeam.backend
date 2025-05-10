using Models;
using Utilities;
using Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DentalManagementSystem.Services.Interfaces;
using System.Threading.Tasks;
using System.Numerics;


namespace DentalManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicController : ControllerBase
    {
        private readonly IAuthServices _authServices;
        private readonly IClinicServices _clinicServices;
        private readonly INotificationServices _notificationServices;
        private readonly UserManager<User> _userManager;
        public ClinicController(IAuthServices authServices,
                                IClinicServices clinicServices,
                                UserManager<User> userManager,
                                INotificationServices notificationServices)
        {
            _authServices = authServices;
            _userManager = userManager;
            _clinicServices = clinicServices;
            _notificationServices = notificationServices;
        }
        // GET: api/Clinic
        [HttpGet]
        public async Task<IActionResult> GetClinic()
        {
            try
            {
                var claims = _authServices.GetClaims(Request);
                var permissions = claims?.FirstOrDefault(c => c.Type == "role")?.Value;
                var userId = claims?.FirstOrDefault(c => c.Type == "userId")?.Value;

                if (permissions != Utility.Admin_Role)
                    return Unauthorized(new { message = "You don't have permission to access this resource." });

                var clinic = await _clinicServices.GetClinicByUserId(userId);

                if (clinic != null)
                    return Ok(clinic);
                return BadRequest("Clinic not exists");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("team-members")]
        public async Task<IActionResult> GetMembers()
        {
            try
            {
                var claims = _authServices.GetClaims(Request);
                var permissions = claims.First(x => x.Type == "role").Value.ToLower();
                if (permissions != Utility.Admin_Role)
                    return Unauthorized(new { message = "you don't have permission to do this." });

                var userId = claims.First(x => x.Type == "userId").Value;

                var users = await _clinicServices.GetClinicMembers(userId);

                if (users == null) return NotFound();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("patients")]
        public async Task<IActionResult> GetPatients()
        {
                var claims = _authServices.GetClaims(Request);
                var permission = claims.First(x => x.Type == "role").Value;

                if (permission == Utility.User_Role)
                    return Unauthorized();

                var userId = claims.First(x => x.Type == "userId").Value;
            try
            {
                var patients = await _clinicServices.GetClinicPatients(userId);
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClinic(Clinic clinic)
        {
            try
            {
                var claims = _authServices.GetClaims(Request);
                var permissions = claims?.FirstOrDefault(c => c.Type == "role")?.Value;
                var userId = claims?.FirstOrDefault(c => c.Type == "userId")?.Value;
                if (permissions.ToLower() != Utility.Admin_Role)
                    return Unauthorized(new { message = "You don't have permission to access this resource." });

                var result = await _clinicServices.UpdateClinic(userId, clinic);
                if (result)
                    return Ok(new { message = "Clinic updated successfully." });
                return BadRequest(new { message = "Something is wrong try it letter" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateClinic(Clinic clinic)
        {
            try
            {
                var claims = _authServices.GetClaims(Request);
                var permissions = claims?.FirstOrDefault(c => c.Type == "role")?.Value;
                var userId = claims?.FirstOrDefault(c => c.Type == "userId")?.Value;
                if (permissions.ToLower() != Utility.Admin_Role)
                    return Unauthorized(new { message = "You don't have permission to access this resource." });

                var result = await _clinicServices.CreateClinic(userId, clinic);

                if (result)
                    return Ok(new { message = "Clinic created successfully." });
                return StatusCode(500, new { message = "Internal server error" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost("add-member")]
        public async Task<IActionResult> AddMember(MemberRequest member)
        {
            try
            {
                var claims = _authServices.GetClaims(Request);
                var permissions = claims.First(x => x.Type == "role").Value.ToLower();
                if (permissions != Utility.Admin_Role)
                    return Unauthorized(new { message = "You don't have permission to do this." });

                var userId = claims.First(x => x.Type == "userId").Value;
                var admin = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                var result = await _clinicServices.AddMember(userId, member);

                await _notificationServices.SendNotification(AddMemberNotification(result, admin), userId);

                if (result != null)
                    return Ok(new { message = "User Added Successfully"});

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in ({nameof(AddMember)}) Message {ex.Message}");
            }
        }  //Not completed
        
        [HttpPost("add-patient")]
        public async Task<IActionResult> AddNewPatient(PatientRequest dto)
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value.ToLower();
            if (permission == Utility.User_Role)
                return Unauthorized();

            var userId = claims.First(x => x.Type == "userId").Value;
            var admin = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            try
            {
                var result = await _clinicServices.AddPatinet(userId, dto);
                if (result != null)
                {
                    await _notificationServices.SendNotification(AddPatientNotification(result, admin), userId);

                    return Ok("Patient Added Successfully.");
                }
                return BadRequest("There is some errors or the patient Is allready exists.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("appointments")]
        public async Task<IActionResult> GetAppointments()
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value;

            if (permission == Utility.User_Role)
                return Unauthorized();

            var userId = claims.First(x => x.Type == "userId").Value;

            try
            {
                var appoints = await _clinicServices.GetAppointments(userId);
                return Ok(appoints);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-member")]
        public async Task<IActionResult> UpdateMember(MemberRequest data)
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value;
            if (permission.ToLower() != Utility.Admin_Role)
                return Unauthorized(); 
            
            var adminId = claims.First(x => x.Type == "userId").Value;

            try
            {
                await _clinicServices.UpdateClinicMember(data, adminId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-member")]
        public async Task<IActionResult> DeleteClinicMember(string memberId)
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value;
            if (permission != Utility.Admin_Role)
                return Unauthorized();

            var adminId = claims.First(x => x.Type == "userId").Value;
            try
            {
                await _clinicServices.DeleteClinicMember(memberId, adminId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-appointment-status")]
        public async Task<IActionResult> UpdateAppointmentStatus(UpdateAppointmentStatusRequest appointmentRequest)
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value;
            if (permission.ToLower() == Utility.User_Role)
                return Unauthorized();

            var adminId = claims.First(x => x.Type == "userId").Value;

            try
            {
                await _clinicServices
                    .UpdateAppointmentStatus(appointmentRequest.AppointmentId, appointmentRequest.AppointmentStatus);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("add-plan")]
        public async Task<IActionResult> AddPlan(TreatmentPlanRequest plan)
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value;
            if (permission.ToLower() == Utility.User_Role)
                return Unauthorized();

            var adminId = claims.First(x => x.Type == "userId").Value;

            try
            {
                await _clinicServices.AddTreatmentPlan(plan);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("add-session")]
        public async Task<IActionResult> AddSession(int treatmentId, PlanSessionRequest session)
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value;
            if (permission.ToLower() == Utility.User_Role)
                return Unauthorized();

            var adminId = claims.First(x => x.Type == "userId").Value;

            try
            {
                await _clinicServices.AddPlanSession(treatmentId, session);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-session")]
        public async Task<IActionResult> UpdateSession(int sessionId, PlanSessionRequest session)
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value;
            if (permission.ToLower() == Utility.User_Role)
                return Unauthorized();

            var adminId = claims.First(x => x.Type == "userId").Value;

            try
            {
                await _clinicServices.UpdateSession(sessionId, session);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("complete-session")]
        public async Task<IActionResult> CompleteSession(int sessionId)
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value;
            if (permission.ToLower() == Utility.User_Role)
                return Unauthorized();

            var adminId = claims.First(x => x.Type == "userId").Value;

            try
            {
                await _clinicServices.CompleteSession(sessionId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-session")]
        public async Task<IActionResult> DeleteSession(int sessionId)
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value;
            if (permission.ToLower() == Utility.User_Role)
                return Unauthorized();

            var adminId = claims.First(x => x.Type == "userId").Value;

            try
            {
                await _clinicServices.DeleteSession(sessionId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-plans")]
        public async Task<IActionResult> GetPlans()
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value;
            if (permission.ToLower() == Utility.User_Role)
                return Unauthorized();

            var adminId = claims.First(x => x.Type == "userId").Value;
            var clinic = await _clinicServices.GetClinicByUserId(adminId);

            try
            {
                var plans = _clinicServices.GetTreatmentPlanAsync(clinic.Id);

                return Ok(plans);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        NotificationRequest AddMemberNotification(User member, User admin)
        {
            return new()
            {
                UserId = member.Id,
                Message = "we have send to you an invitation to join our team",
                Title = $"Hello {member.FirstName + " " + member.LastName}",
                Type = "Invitation",
                SenderId = admin.Id,
            };
        }
        NotificationRequest AddPatientNotification(User member, User admin)
        {
            return new()
            {
                UserId = member.Id,
                Message = "you are added to our clinic",
                Title = $"Hello {member.FirstName + " " + member.LastName}",
                Type = "Invitation",
                SenderId = admin.Id,
            };
        }
    }
}
