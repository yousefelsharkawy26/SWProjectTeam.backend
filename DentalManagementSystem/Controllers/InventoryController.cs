using Utilities;
using Models.Requests;
using Microsoft.AspNetCore.Mvc;
using DentalManagementSystem.Services.Interfaces;

namespace DentalManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryServices _inventoryServices;
        private readonly IAuthServices _authServices;
        private readonly IClinicServices _clinicServices;
        public InventoryController(IInventoryServices inventoryServices,
                                   IAuthServices authServices,
                                   IClinicServices clinicServices)
        {
            _inventoryServices = inventoryServices;
            _authServices = authServices;
            _clinicServices = clinicServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentInventories()
        {
            var claims = _authServices.GetClaims(Request);
            var permissions = claims?.FirstOrDefault(c => c.Type == "role")?.Value;
            var userId = claims?.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (permissions != Utility.Admin_Role && permissions != Utility.Receptionist_Role && permissions != Utility.Dentist_Role)
                return Unauthorized(new { message = "You don't have permission to access this resource." });
            
            var clinic = await _clinicServices.GetClinicByUserId(userId);

            try
            {
                var inventories = _inventoryServices.GetInventories(clinic.Id);

                return Ok(inventories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddNewInventory(InventoryRequest inventory)
        {
            var claims = _authServices.GetClaims(Request);
            var permissions = claims?.FirstOrDefault(c => c.Type == "role")?.Value;
            var userId = claims?.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (permissions != Utility.Admin_Role && permissions != Utility.Receptionist_Role && permissions != Utility.Dentist_Role)
                return Unauthorized(new { message = "You don't have permission to access this resource." });

            var clinic = await _clinicServices.GetClinicByUserId(userId);

            try
            {
                await _inventoryServices.AddNewInventory(clinic.Id, inventory);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Restock(string inventoryId, RestockRequest restock)
        {
            var claims = _authServices.GetClaims(Request);
            var permissions = claims?.FirstOrDefault(c => c.Type == "role")?.Value;
            var userId = claims?.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (permissions != Utility.Admin_Role && permissions != Utility.Receptionist_Role && permissions != Utility.Dentist_Role)
                return Unauthorized(new { message = "You don't have permission to access this resource." });

            var clinic = await _clinicServices.GetClinicByUserId(userId);

            try
            {
                if (int.TryParse(inventoryId, out int id))
                    await _inventoryServices.Restock(id, restock);
                else
                    throw new Exception("Wrong Id passed");

                    return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
