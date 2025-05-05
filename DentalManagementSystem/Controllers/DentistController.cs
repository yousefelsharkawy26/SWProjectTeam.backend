using Microsoft.AspNetCore.Mvc;
using DentalManagementSystem.Services.Interfaces;

namespace DentalManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentistController : ControllerBase
    {
        private readonly IAuthServices _authServices;
        private readonly IClinicServices _clinicServices;
        public DentistController(IAuthServices authServices
                               , IClinicServices clinicServices)
        {
            _authServices = authServices;
            _clinicServices = clinicServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByClinicId()
        {
            try
            {
                var claims = _authServices.GetClaims(Request);
                var permissions = claims.First(x => x.Type == "role").Value.ToLower();
                if (permissions == "user")
                    return Unauthorized(new { message = "You don't have permission to do this." });

                var userId = claims.First(x => x.Type == "userId").Value;
                
                var members = await _clinicServices.GetClinicMembers(userId);

                return Ok(members);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
