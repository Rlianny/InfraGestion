using Application.DTOs.Auth;
using Application.DTOs.Inventory;
using Application.DTOs.Transfer;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Web.API.Shared;
using Web.API.DTOs;
namespace Web.API.Controllers
{
    [ApiController]
    [Route("transfers")]
    public class TransfersController : BaseApiController
    {
        private readonly ITransferService transferService;


        public TransfersController(ITransferService transferService)
        {
            this.transferService = transferService;
        }
        #region GET
        [HttpGet("pending")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TransferDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPendingTransfersAsync()
        {
            try
            {
                var pendingTransfers = await transferService.GetPendingTransfersAsync();
                return Ok(pendingTransfers);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TransferDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransferByIdAsync(int id)
        {
            try
            {
                var transfer = await transferService.GetTransferByIdAsync(id);
                return Ok(transfer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("devices/{deviceId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TransferDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransfersByDeviceIdAsync(int deviceId) 
        {
            try
            {
                var transfersByDevice = await transferService.GetTransfersByDeviceAsync(deviceId);
                return Ok(transfersByDevice);
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
        [Authorize(Roles= "SectionManager")]
        public async Task<IActionResult> InitiateTransferAsync([FromBody] CreateTransferRequestDto requestDto)
        {
            try
            {
                await transferService.InitiateTransferAsync(requestDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("confirmations/{transferId}")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task <IActionResult> ConfirmReception(int transferId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value);

                await transferService.ConfirmReceptionAsync(transferId,userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region PUT
        [HttpPut("location")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateEquipmentLocationAsync([FromBody] UpdateEquipmentLocationRequestDto request)
        {
            try
            {
                await transferService.UpdateEquipmentLocationAsync(request.DeviceId, request.DepartmentId);
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
