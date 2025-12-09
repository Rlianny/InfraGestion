using Application.DTOs;
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
        public async Task<IActionResult> GetInventoryAsync(int userID)
        {
            try
            {
                System.Console.WriteLine(userID);
                var devices = await inventoryService.GetCompanyInventoryAsync(userID);
                System.Console.WriteLine(devices.Count());
                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("receiving-inspection-requests")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ReceivingInspectionRequestDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReceivingInspectionRequestsByTechnicianAsync(int technicianId)
        {
            try
            {
                var inspectionRequests = await inventoryService.ReceivingInspectionRequestsByTechnician(technicianId);
                return Ok(inspectionRequests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("pendingFirstInspection/technicianId")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ReceivingInspectionRequestDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPendingReceivingInspectionRequestByTechnician(int technicianId)
        {
            try
            {
                return Ok(await inventoryService.GetPendingReceivingInspectionRequestByTechnicianAsync(technicianId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("RevisedDevices/adminId")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRevisedDevicesByAdmin(int adminId)
        {
            try
            {
                var devices = await inventoryService.GetRevisedDevicesByAdmin(adminId);
                return Ok(devices);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("firstInspection/adminId")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ReceivingInspectionRequestDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReceivingInspectionRequestByAdmin(int adminId)
        {
            try
            {
                return Ok(await inventoryService.GetReceivingInspectionRequestsByAdminAsync(adminId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("deviceDetail/{id}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDetailDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDeviceDetailAsync(int id)
        {
            try
            {
                var deviceDetail = await inventoryService.GetDeviceDetailAsync(id);
                return Ok(deviceDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("company/devices")]
        [Authorize(Roles = "Director")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyDevicesAsync(int userID)
        {
            try
            {
                var devices = await inventoryService.GetCompanyInventoryAsync(userID);
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
            catch (Exception ex)
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
                // Always tie the request to the authenticated administrator to avoid missing/invalid IDs
                var userId = int.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value);
                request.userID = userId;
                await inventoryService.RegisterDeviceAsync(request);
                return Ok("Registered");
            }
            catch (Exception ex)
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
        [HttpPost("disables{id}")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisableDevicesAsync(int id)
        {
            try
            {
                await inventoryService.DisableEquipmentAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpPost("ProcessFirstInspection")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ProcessFirstInspection(InspectionDecisionRequestDto inspectionReviewRequestDto)
        {
            try
            {
                await inventoryService.ProcessInspectionDecisionAsync(inspectionReviewRequestDto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
            catch (Exception ex)
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
                await inventoryService.DeleteEquipmentAsync(id);
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
