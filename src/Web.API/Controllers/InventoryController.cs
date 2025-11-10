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
                var userId = int.Parse(User.FindFirst("sub")!.Value);
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
                var userId = int.Parse(User.FindFirst("sub")!.Value);
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

        [HttpPost("rejections")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RejectDevice([FromBody] RejectDeviceRequestDto rejectDeviceRequest)
        {
            try
            {
                await inventoryService.RejectDevice(rejectDeviceRequest.DeviceId, rejectDeviceRequest.TechnicianId, rejectDeviceRequest.Reason);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("approbals")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AcceptDevice([FromBody] AcceptDeviceRequestDto acceptDeviceRequest)
        {
            try
            {
                await inventoryService.ApproveDevice(acceptDeviceRequest.DeviceId,acceptDeviceRequest.TechnicianId);
                return Ok();
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

    }

}
