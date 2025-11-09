using Application.DTOs.Auth;
using Application.DTOs.Inventory;
using Application.Services.Implementations;
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
            var inventory = await inventoryService.GetInventoryAsync(filter, userId);
            return Ok(inventory);
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
            catch
            {
                return BadRequest("Bad request");
            }
        }


        [HttpGet("company/devices")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyDevicesAsync()
        {
            try
            {
                var devices = await inventoryService.GetCompanyInventoryAsync();
                return Ok(devices, "Succes");
            }
            catch (Exception ex)
            { 
                return BadRequest("Bad request");  
            }
        }
        [HttpGet("sections/{sectionId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSectionInventoryAsync(int sectionId)
        {
            try
            {
                var userIdStr = User.FindFirst("id").Value;
                if (int.TryParse(userIdStr, out var userId))
                {
                    return BadRequest("Bad request");
                }
                var devices = await inventoryService.GetSectionInventoryAsync(sectionId, userId);
                return Ok(devices);
            }
            catch
            {
               return BadRequest("Bad Request");
            }
        }
        [HttpGet("ownedSection")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOwnSectionInventory()
        {
            var userSection = User.FindFirst("id").Value;
            if(int.TryParse(userSection, out var sectionId))
            {
                try
                {
                   var devices = await inventoryService.GetUsersOwnSectionInventory(sectionId);
                   return Ok(devices);
                }
                catch
                {
                    return BadRequest();
                }
            }
            else
            {
             return BadRequest();   
            }


        }
        #endregion
        #region POST
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterDevice([FromBody] InsertDeviceRequestDto request)
        {
            await inventoryService.RegisterDeviceAsync(request);
            return Ok("Registered");
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
                return BadRequest("Bad request");
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
                return BadRequest("Bad request");
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
                return BadRequest("Bad request");
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
            catch
            {
                return BadRequest();
               
            }
        }
        #endregion

    }

}
