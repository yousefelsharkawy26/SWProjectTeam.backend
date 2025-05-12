using Models.Requests;
using Models.Responses;
using Microsoft.AspNetCore.Mvc;
using DentalManagementSystem.Services.Interfaces;
using System.Threading.Tasks;

namespace DentalManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IAuthServices _authServices;
        private readonly IClinicServices _clinicServices;
        private readonly IUserServices _userServices;
        public PatientsController(IAuthServices authServices
                                , IClinicServices clinicServices
                                , IUserServices userServices)
        {
            _authServices = authServices;
            _clinicServices = clinicServices;
            _userServices = userServices;
        }

        
        
        [HttpGet("search")]
        public async Task<IActionResult> FindUser(string email)
        {
            try
            {
                PatientResponse patient = await _clinicServices.FindPatientAsync(email);
                return Ok(patient);
            }
            catch
            {
                return Ok(new { Email = email });
            }
        }
        
        [HttpPost("create-appointment")]
        public async Task<IActionResult> CreateAppointment([FromBody]AppointmentRequest dto)
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value.ToLower();

            if (permission == "user")
                return Unauthorized();
            var userId = claims.First(x => x.Type == "userId").Value;

            try
            {
                var result = await _clinicServices.CreateAppointment(userId, dto);
                if (result)
                    return Ok("success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest("failed");
        }

        [HttpGet("patient-medicalRecords")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value.ToLower();

            if (permission == "user")
                return Unauthorized();

            try
            {
                var result = await _userServices.GetPatientMedicalRecords(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-medical-examination")]
        public async Task<IActionResult> AddMedicalExamination(MedicalExaminationRequest examination)
        {
            var claims = _authServices.GetClaims(Request);
            var permission = claims.First(x => x.Type == "role").Value.ToLower();

            if (permission == "user")
                return Unauthorized();
            var userId = claims.First(x => x.Type == "userId").Value;

            try
            {
                await _userServices.AddMedicalExamination(examination);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
