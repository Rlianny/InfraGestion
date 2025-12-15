using Application.DTOs.Decommissioning;
using Application.Services.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Shared;
using Application.DTOs;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("decommissioning")]
    public class DecommissioningController : BaseApiController
    {
        private readonly IDecommissioningService _decommissioningService;

        public DecommissioningController(IDecommissioningService decommissioningService)
        {
            _decommissioningService = decommissioningService;
        }

        #region DecommissioningRequest (Decommissioning Requests)

        /// <summary>
        /// Create a device decommissioning request
        /// </summary>
        [HttpPost("requests")]
        [Authorize(Roles = "Technician")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDecommissioningRequest([FromBody] CreateDecommissioningRequestDto request)
        {
            try
            {
                await _decommissioningService.CreateDecommissioningRequestAsync(request);
                return Ok("Decommissioning request created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all pending decommissioning requests
        /// </summary>
        [HttpGet("requests/pending")]
        [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DecommissioningRequestDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPendingRequests()
        {
            try
            {
                var requests = await _decommissioningService.GetPendingRequestsAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all decommissioning requests
        /// </summary>
        [HttpGet("requests")]
        [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DecommissioningRequestDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRequests()
        {
            try
            {
                int userId = GetCurrentUserId();
                var requests = await _decommissioningService.GetAllRequestsAsync(userId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        ///<summary>
        /// Get all decommissioning requests
        /// </summary>
        [HttpGet("requests/technicianId")]
        [Authorize(Roles = "Technician")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DecommissioningRequestDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByTechnician(int technicianId)
        {
            try
            {
                var requests = await _decommissioningService.GetAllRequestsAsync(technicianId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a decommissioning request by ID
        /// </summary>
        [HttpGet("requests/{requestId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<DecommissioningRequestDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRequestById(int requestId)
        {
            try
            {
                var request = await _decommissioningService.GetRequestByIdAsync(requestId);
                return Ok(request);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get decommissioning requests by device
        /// </summary>
        [HttpGet("requests/device/{deviceId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DecommissioningRequestDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRequestsByDeviceId(int deviceId)
        {
            try
            {
                var requests = await _decommissioningService.GetRequestsByDeviceIdAsync(deviceId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Review (approve/reject) a decommissioning request
        /// </summary>
        [HttpPost("requests/review")]
        [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReviewDecommissioningRequest([FromBody] ReviewDecommissioningRequestDto review)
        {
            try
            {
                await _decommissioningService.ReviewDecommissioningRequestAsync(review);
                var message = review.IsApproved
                    ? "Decommissioning request approved - device has been decommissioned"
                    : "Decommissioning request rejected";
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Decommissioning (Completed Decommissionings)

        /// <summary>
        /// Get all completed decommissionings
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DecommissioningDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDecommissionings()
        {
            try
            {
                var decommissionings = await _decommissioningService.GetAllDecommissioningsAsync();
                return Ok(decommissionings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a decommissioning by ID
        /// </summary>
        [HttpGet("{decommissioningId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<DecommissioningDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDecommissioningById(int decommissioningId)
        {
            try
            {
                var decommissioning = await _decommissioningService.GetDecommissioningByIdAsync(decommissioningId);
                return Ok(decommissioning);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get the decommissioning for a device (only one can exist)
        /// </summary>
        [HttpGet("device/{deviceId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<DecommissioningDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDecommissioningByDeviceId(int deviceId)
        {
            try
            {
                var decommissioning = await _decommissioningService.GetDecommissioningByDeviceIdAsync(deviceId);
                if (decommissioning == null)
                    return NotFound($"No decommissioning found for device {deviceId}");
                return Ok(decommissioning);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get decommissionings by date range
        /// </summary>
        [HttpGet("by-date-range")]
        [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DecommissioningDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDecommissioningsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var decommissionings = await _decommissioningService.GetDecommissioningsByDateRangeAsync(startDate, endDate);
                return Ok(decommissionings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get decommissionings by department
        /// </summary>
        [HttpGet("department/{departmentId}")]
        [Authorize(Roles = "Administrator,Director,SectionManager")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DecommissioningDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDecommissioningsByDepartment(int departmentId)
        {
            try
            {
                var decommissionings = await _decommissioningService.GetDecommissioningsByDepartmentAsync(departmentId);
                return Ok(decommissionings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get decommissionings by reason
        /// </summary>
        [HttpGet("by-reason/{reason}")]
        [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DecommissioningDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDecommissioningsByReason(DecommissioningReason reason)
        {
            try
            {
                var decommissionings = await _decommissioningService.GetDecommissioningsByReasonAsync(reason);
                return Ok(decommissionings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
        #region PUT
        [HttpPut("requests/{requestId}")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDecommissioningRequest([FromBody] UpdateDecommissioningRequestDto updateRequest)
        {
            try
            {
                await _decommissioningService.UpdateDecommissioningRequestAsync(updateRequest);
                return Ok("Decommissioning request updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region DELETE

        /// <summary>
        /// Elimina un dispositivo del sistema.
        /// Solo accesible para Administrator.
        /// </summary>
        [HttpDelete("delete/requestId")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDeviceAsync(int requestid)
        {
            try
            {
                await _decommissioningService.DeleteDecommissioningRequestAsync(requestid);
                return Ok("Decommissioning request deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
