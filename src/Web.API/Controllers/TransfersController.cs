using Application.DTOs.Auth;
using Application.DTOs.DevicesDTOs;
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
        [HttpGet("devices/{deviceName}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TransferDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransfersByDeviceNameAsync(string deviceName)
        {
            try
            {
                var transfersByDevice = await transferService.GetTransfersByDeviceNameAsync(deviceName);
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
        [Authorize(Roles = "SectionManager")]
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
        public async Task<IActionResult> ConfirmReception(int transferId)
        {
            try
            {
                var username = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value
                    ?? throw new UnauthorizedAccessException("Username not found in token");

                await transferService.ConfirmReceptionAsync(transferId, username);
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
                await transferService.UpdateEquipmentLocationAsync(request.DeviceName, request.DepartmentName);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("desactivate/{id}")]
        public async Task<IActionResult> DesactivateTransferAsync(int id)
        {
            try
            {
                await transferService.DesactivateTransferAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransferAsync(int id)
        {
            try
            {
                await transferService.DeleteTransferAsync(id);
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
