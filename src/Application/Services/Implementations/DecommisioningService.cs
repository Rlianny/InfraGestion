using Application.Services.Interfaces;
using Application.DTOs.Decommissioning;
using Domain.Aggregations;
using Domain.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using Application.DTOs;

public class DecommissioningService : IDecommissioningService
{
    private readonly IDecommissioningRequestRepository _requestRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DecommissioningService(
        IDecommissioningRequestRepository requestRepository,
        IDeviceRepository deviceRepository,
        IUserRepository userRepository,
        IDepartmentRepository departmentRepository,
        ISectionRepository sectionRepository,
        IUnitOfWork unitOfWork)
    {
        _requestRepository = requestRepository;
        _deviceRepository = deviceRepository;
        _userRepository = userRepository;
        _departmentRepository = departmentRepository;
        _sectionRepository = sectionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateDecommissioningRequestAsync(CreateDecommissioningRequestDto request)
    {
        // Validate that the device exists and can be decommissioned
        var device = await _deviceRepository.GetByIdAsync(request.DeviceId)
            ?? throw new EntityNotFoundException("Device", request.DeviceId);

        if (!device.CanBeDecommissioned())
        {
            throw new Exception($"Device {device.Name} cannot be decommissioned in its current state");
        }
        var technician = await _userRepository.GetByIdAsync(request.TechnicianId)
            ?? throw new EntityNotFoundException("Technician", request.TechnicianId);

        var decommissioningRequest = new DecommissioningRequest(
            request.TechnicianId,
            request.DeviceId,
            request.RequestDate,
            request.Reason
        );

        await _requestRepository.AddAsync(decommissioningRequest);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<DecommissioningRequestDto>> GetPendingRequestsAsync()
    {
        var allRequests = await _requestRepository.GetAllAsync();
        var pendingRequests = allRequests.Where(r => r.IsPending());
        return await MapToDto(pendingRequests);
    }

    public async Task<IEnumerable<DecommissioningRequestDto>> GetAllRequestsAsync()
    {
        var requests = await _requestRepository.GetAllAsync();
        return await MapToDto(requests);
    }

    public async Task<DecommissioningRequestDto> GetRequestByIdAsync(int requestId)
    {
        var request = await _requestRepository.GetByIdAsync(requestId)
            ?? throw new EntityNotFoundException("DecommissioningRequest", requestId);

        var device = await _deviceRepository.GetByIdAsync(request.DeviceId)
        ?? throw new EntityNotFoundException("Device", request.DeviceId);
        var technician = await _userRepository.GetByIdAsync(request.TechnicianId)
        ?? throw new EntityNotFoundException("Technician", request.TechnicianId);

        return new DecommissioningRequestDto
        {
            DecommissioningRequestId = request.DecommissioningRequestId,
            DeviceId = request.DeviceId,
            DeviceName = device.Name,
            TechnicianId = request.TechnicianId,
            TechnicianName = technician.FullName,
            RequestDate = request.EmissionDate,
            Status = MapToDecommissioningStatus(request.Status),
            Reason = request.Reason,
            Justification = request.description,
            ReviewedByUserId = request.logisticId,
            ReviewedByUserName = request.DeviceReceiverId == null ? "Unrevised" : (await _userRepository.GetByIdAsync((int)request.DeviceReceiverId)).FullName
        };
    }

    public async Task<IEnumerable<DecommissioningRequestDto>> GetRequestsByDeviceIdAsync(int deviceId)
    {
        var requests = await _requestRepository.GetDecommissioningRequestsByDeviceAsync(deviceId);
        return await MapToDto(requests);
    }

    public async Task ReviewDecommissioningRequestAsync(ReviewDecommissioningRequestDto review)
    {
        var request = await _requestRepository.GetByIdAsync(review.DecommissioningRequestId)
            ?? throw new EntityNotFoundException("DecommissioningRequest", review.DecommissioningRequestId);

        var device = await _deviceRepository.GetByIdAsync(request.DeviceId)
            ?? throw new EntityNotFoundException("Device", request.DeviceId);

        if (review.IsApproved)
        {
            request.Approve((int)review.logisticId, (int)review.ReceiverID, (int)review.FinalDestination, review.description, review.date, (DecommissioningReason)review.DecommissioningReason);
            device.UpdateOperationalState(OperationalState.Decommissioned);
        }
        else
        {
            device.UpdateOperationalState(OperationalState.Operational);
            request.Reject((int)review.logisticId, review.description, review.date);
        }

        await _deviceRepository.UpdateAsync(device);
        await _requestRepository.UpdateAsync(request);
        await _unitOfWork.SaveChangesAsync();
    }
    private async Task<IEnumerable<DecommissioningRequestDto>> MapToDto(IEnumerable<DecommissioningRequest> requests)
    {
        var dtos = new List<DecommissioningRequestDto>();

        foreach (var request in requests)
        {
            var device = await _deviceRepository.GetByIdAsync(request.DeviceId);
            var technician = await _userRepository.GetByIdAsync(request.TechnicianId);
            dtos.Add(new DecommissioningRequestDto
            {
                DecommissioningRequestId = request.DecommissioningRequestId,
                DeviceId = request.DeviceId,
                DeviceName = device?.Name ?? "Unknown",
                TechnicianId = request.TechnicianId,
                TechnicianName = technician?.FullName ?? "Unknown",
                RequestDate = request.EmissionDate,
                Status = MapToDecommissioningStatus(request.Status),
                Reason = request.Reason,
                ReviewedDate = request.AnswerDate,
                Justification = request.description,
                ReviewedByUserId = request.logisticId,
                ReviewedByUserName = request.logisticId == null ? "Unrevised" : (await _userRepository.GetByIdAsync((int)request.logisticId)).FullName
            });
        }

        return dtos;
    }
    private async Task<DecommissioningDto> MapRequestToDecommissioningDto(DecommissioningRequest decommissioningRequest)
    {
        var device = await _deviceRepository.GetByIdAsync(decommissioningRequest.DeviceId);
        var user = await _userRepository.GetByIdAsync((int)decommissioningRequest.DeviceReceiverId);
        var userDptmt = await _departmentRepository.GetByIdAsync(user.DepartmentId);
        return new DecommissioningDto
        {
            DeviceId = decommissioningRequest.DeviceId,
            DeviceName = device.Name,
            DecommissioningRequestId = decommissioningRequest.DecommissioningRequestId,
            DeviceReceiverId = (int)decommissioningRequest.DeviceReceiverId,
            DeviceReceiverName = user.FullName,
            ReceiverDepartmentId = user.DepartmentId,
            ReceiverDepartmentName = userDptmt.Name,
            DecommissioningDate = DateTime.Now,
            Reason = decommissioningRequest.Reason,
            FinalDestination = null
        };
    }
    private DecommissioningStatus MapToDecommissioningStatus(RequestStatus status)
    {
        return status switch
        {
            RequestStatus.Pending => DecommissioningStatus.Pending,
            RequestStatus.Approved => DecommissioningStatus.Accepted,
            RequestStatus.Rejected => DecommissioningStatus.Rejected,
            _ => DecommissioningStatus.Pending
        };
    }

    #region Decommissioning (Bajas finalizadas)

    public async Task<IEnumerable<DecommissioningDto>> GetAllDecommissioningsAsync()
    {
        var decommissionings = (await _requestRepository.GetAllAsync()).Where(decommissioningRequest => decommissioningRequest.IsApproved != null && (bool)decommissioningRequest.IsApproved);
        List<DecommissioningDto> decommissioningDtos = [];
        foreach (var decommission in decommissionings)
        {
            var dto = await MapRequestToDecommissioningDto(decommission);
            decommissioningDtos.Add(dto);
        }
        return decommissioningDtos;
    }

    public async Task<DecommissioningDto> GetDecommissioningByIdAsync(int decommissioningId)
    {
        var decommissioning = await _requestRepository.GetByIdAsync(decommissioningId)
            ?? throw new EntityNotFoundException("Decommissioning", decommissioningId);

        if (decommissioning.IsApproved == null || !(bool)decommissioning.IsApproved)
        {
            throw new EntityNotFoundException("Decommissioning Request not a Decommissioning", decommissioningId);
        }
        return await MapRequestToDecommissioningDto(decommissioning);
    }

    // There can only be ONE decommissioning per device
    public async Task<DecommissioningDto?> GetDecommissioningByDeviceIdAsync(int deviceId)
    {
        var decommissioning = (await _requestRepository.GetDecommissioningRequestsByDeviceAsync(deviceId)).Where(d => d.IsApproved != null && (bool)d.IsApproved);
        if (decommissioning == null || decommissioning.Count() == 0)
        {
            throw new Exception("No decommissions for this device");
        }
        return await MapRequestToDecommissioningDto(decommissioning.First());
    }

    public async Task<IEnumerable<DecommissioningDto>> GetDecommissioningsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var decommissionings = (await _requestRepository.GetDecommissioningRequestsByDateRangeAsync(startDate, endDate)).Where(d => d.IsApproved != null && (bool)d.IsApproved);
        List<DecommissioningDto> decommissioningDtos = [];
        foreach (var decommission in decommissionings)
        {
            var dto = await MapRequestToDecommissioningDto(decommission);
            decommissioningDtos.Add(dto);
        }
        return decommissioningDtos;
    }

    public async Task<IEnumerable<DecommissioningDto>> GetDecommissioningsByDepartmentAsync(int departmentId)
    {
        var devices = (await _deviceRepository.GetAllAsync()).Where(d => d.DepartmentId == departmentId);

        var decommissionings = new List<DecommissioningDto>();
        foreach (var device in devices)
        {
            var decommissioningRequest = await _requestRepository.GetDecommissioningRequestsByDeviceAsync(device.DeviceId);
            var decommissioning = await MapRequestToDecommissioningDto(decommissioningRequest.Where(d => d.IsApproved != null && (bool)d.IsApproved)?.FirstOrDefault());
            if (decommissioning != null && device.DepartmentId == departmentId)
            {
                decommissionings.Add(decommissioning);
            }
        }
        return decommissionings;
    }

    public async Task<IEnumerable<DecommissioningDto>> GetDecommissioningsByReasonAsync(DecommissioningReason reason)
    {
        var decommissionings = (await _requestRepository.GetAllAsync()).Where(d => d.IsApproved != null && (bool)d.IsApproved && d.Reason == reason);
        List<DecommissioningDto> decommissioningDtos = [];
        foreach (var decommission in decommissionings)
        {
            var dto = await MapRequestToDecommissioningDto(decommission);
            decommissioningDtos.Add(dto);
        }
        return decommissioningDtos;
    }

    #endregion

    public async Task DeleteDecommissioningRequestAsync(int requestId)
    {
        var request = await _requestRepository.GetByIdAsync(requestId)
            ?? throw new EntityNotFoundException("DecommissioningRequest", requestId);

        await _requestRepository.DeleteAsync(request);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateDecommissioningRequestAsync(UpdateDecommissioningRequestDto requestDto)
    {
        var decommissioningRequest = await _requestRepository.GetByIdAsync(requestDto.DecommissioningRequestId)
            ?? throw new EntityNotFoundException("DecommissioningRequest", requestDto.DecommissioningRequestId);
        decommissioningRequest.Update(requestDto.DecommissioningRequestId,
        requestDto.TechnicianId,
        requestDto.DeviceId,
        requestDto.EmissionDate,
        requestDto.AnswerDate,
        requestDto.Status,
        requestDto.Reason,
        requestDto.DeviceReceiverId,
        requestDto.IsApproved,
        requestDto.FinalDestinationDepartmentID,
        requestDto.logisticId,
        requestDto.description);
        await _requestRepository.UpdateAsync(decommissioningRequest);
        await _unitOfWork.SaveChangesAsync();
    }
}