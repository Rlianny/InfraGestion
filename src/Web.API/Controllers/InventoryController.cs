using Application.DTOs.Auth;
using Application.DTOs.Inventory;
using Application.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Web.API.Shared;

namespace Web.API.Controllers
{


    [ApiController]
    [Route("inventory")]
    public class InventoryController : BaseApiController
    {
        private readonly IInventoryService inventoryService;


        public InventoryController(IInventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }
        #region GET



        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInventoryAsync([FromQuery] DeviceFilterDto filter, [FromQuery] int userId)
        {
            try
            {
                var inventory = await inventoryService.GetInventoryAsync(filter, userId);
                return Ok(inventory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDetailDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDeviceDetailAsync(int id)
        {
            try
            {
                var deviceDetail = await inventoryService.GetDeviceDetailAsync(id);
                return Ok(deviceDetail);    
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("company/devices")]
        [Authorize(Roles ="Director")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyDevicesAsync()
        {
            try
            {
                var devices = await inventoryService.GetCompanyInventoryAsync();
                return Ok(devices);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);  
            }
        }
        [HttpGet("sections/{sectionId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSectionInventoryAsync(int sectionId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value);
                var devices = await inventoryService.GetSectionInventoryAsync(sectionId, userId);
                return Ok(devices);
            }
            catch(Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }
        [HttpGet("ownedSection")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOwnSectionInventory()
        {

            try
            {
                var userId = int.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value);
                var devices = await inventoryService.GetUsersOwnSectionInventory(userId);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region POST
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterDevice([FromBody] InsertDeviceRequestDto request)
        {
            try
            {
                await inventoryService.RegisterDeviceAsync(request);
                return Ok("Registered");
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("inspection-decision")]
        [Authorize(Roles = "Technician")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ProcessInspectionDecision([FromBody] InspectionDecisionRequestDto request)
        {
            try
            {
                await inventoryService.ProcessInspectionDecisionAsync(request);
                var message = request.IsApproved ? "Device approved successfully" : "Device rejected successfully";
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("reviews")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReviewDevices([FromBody] AssignDeviceForInspectionRequestDto inspectionRequest)
        {
            try
            {
                await inventoryService.AssignDeviceForReviewAsync(inspectionRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
        #endregion
        #region PUT

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateEquipmentAsync([FromBody] UpdateDeviceRequestDto updateDeviceRequest)
        {
            try
            {
                await inventoryService.UpdateEquipmentAsync(updateDeviceRequest);
                return Ok();
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region DELETE
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteDeviceAsync(int id)
        {
            try
            {
                await inventoryService.DeleteEquimentAsync( new DeleteDeviceRequestDto { DeviceId =id });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

    }

}
