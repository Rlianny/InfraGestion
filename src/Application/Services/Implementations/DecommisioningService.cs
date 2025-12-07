using Application.Services.Interfaces;
using Application.DTOs.Decommissioning;
using Domain.Aggregations;
using Domain.Interfaces;
using Domain.Enums;
using Domain.Exceptions;

public class DecommissioningService : IDecommissioningService
{
    private readonly IDecommissioningRequestRepository _requestRepository;
    private readonly IDecommissioningRepository _decommissioningRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DecommissioningService(
        IDecommissioningRequestRepository requestRepository,
        IDecommissioningRepository decommissioningRepository,
        IDeviceRepository deviceRepository,
        IUserRepository userRepository,
        IDepartmentRepository departmentRepository,
        IUnitOfWork unitOfWork)
    {
        _requestRepository = requestRepository;
        _decommissioningRepository = decommissioningRepository;
        _deviceRepository = deviceRepository;
        _userRepository = userRepository;
        _departmentRepository = departmentRepository;
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

        var receiver = await _userRepository.GetByIdAsync(request.DeviceReceiverId)
            ?? throw new EntityNotFoundException("DeviceReceiver", request.DeviceReceiverId);

        var decommissioningRequest = new DecommissioningRequest(
            request.TechnicianId,
            request.DeviceId,
            request.DeviceReceiverId,
            request.RequestDate
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

        var device = await _deviceRepository.GetByIdAsync(request.DeviceId);
        var technician = await _userRepository.GetByIdAsync(request.TechnicianId);
        var receiver = await _userRepository.GetByIdAsync(request.DeviceReceiverId);

        return new DecommissioningRequestDto
        {
            DecommissioningRequestId = request.DecommissioningRequestId,
            DeviceId = request.DeviceId,
            DeviceName = device?.Name ?? "Unknown",
            TechnicianId = request.TechnicianId,
            TechnicianName = technician?.FullName ?? "Unknown",
            DeviceReceiverId = request.DeviceReceiverId,
            DeviceReceiverName = receiver?.FullName ?? "Unknown",
            RequestDate = request.Date,
            Status = MapToDecommissioningStatus(request.Status)
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
            request.Approve();

            var decommissioning = new Decommissioning(
                request.DeviceId,
                request.DecommissioningRequestId,
                request.DeviceReceiverId,
                review.ReceiverDepartmentId ?? 1,
                DateTime.Now,
                review.DecommissioningReason ?? DecommissioningReason.EOL,
                review.FinalDestination ?? "Storage"
            );

            await _decommissioningRepository.AddAsync(decommissioning);

            device.UpdateOperationalState(OperationalState.Decommissioned);
            await _deviceRepository.UpdateAsync(device);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(review.RejectionReason))
            {
                throw new Exception("Rejection reason is required when rejecting a decommissioning request");
            }
            request.Reject();
        }

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
            var receiver = await _userRepository.GetByIdAsync(request.DeviceReceiverId);

            dtos.Add(new DecommissioningRequestDto
            {
                DecommissioningRequestId = request.DecommissioningRequestId,
                DeviceId = request.DeviceId,
                DeviceName = device?.Name ?? "Unknown",
                TechnicianId = request.TechnicianId,
                TechnicianName = technician?.FullName ?? "Unknown",
                DeviceReceiverId = request.DeviceReceiverId,
                DeviceReceiverName = receiver?.FullName ?? "Unknown",
                RequestDate = request.Date,
                Status = MapToDecommissioningStatus(request.Status)
            });
        }

        return dtos;
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
        var decommissionings = await _decommissioningRepository.GetAllAsync();
        return await MapDecommissioningsToDto(decommissionings);
    }

    public async Task<DecommissioningDto> GetDecommissioningByIdAsync(int decommissioningId)
    {
        var decommissioning = await _decommissioningRepository.GetByIdAsync(decommissioningId)
            ?? throw new EntityNotFoundException("Decommissioning", decommissioningId);

        return await MapDecommissioningToDto(decommissioning);
    }

    // There can only be ONE decommissioning per device
    public async Task<DecommissioningDto?> GetDecommissioningByDeviceIdAsync(int deviceId)
    {
        var decommissioning = await _decommissioningRepository.GetDecommissioningByDeviceAsync(deviceId);
        if (decommissioning == null)
            return null;
        return await MapDecommissioningToDto(decommissioning);
    }

    public async Task<IEnumerable<DecommissioningDto>> GetDecommissioningsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var decommissionings = await _decommissioningRepository.GetDecommissioningsByDateRangeAsync(startDate, endDate);
        return await MapDecommissioningsToDto(decommissionings);
    }

    public async Task<IEnumerable<DecommissioningDto>> GetDecommissioningsByDepartmentAsync(int departmentId)
    {
        var decommissionings = await _decommissioningRepository.GetDecommissioningsByDepartmentAsync(departmentId);
        return await MapDecommissioningsToDto(decommissionings);
    }

    public async Task<IEnumerable<DecommissioningDto>> GetDecommissioningsByReasonAsync(DecommissioningReason reason)
    {
        var decommissionings = await _decommissioningRepository.GetDecommissioningsByReasonAsync(reason);
        return await MapDecommissioningsToDto(decommissionings);
    }
    private async Task<DecommissioningDto> MapDecommissioningToDto(Decommissioning decommissioning)
    {
        var device = await _deviceRepository.GetByIdAsync(decommissioning.DeviceId);
        var receiver = await _userRepository.GetByIdAsync(decommissioning.DeviceReceiverId);
        var department = await _departmentRepository.GetByIdAsync(decommissioning.ReceiverDepartmentId);

        return new DecommissioningDto
        {
            DecommissioningId = decommissioning.DecommissioningId,
            DeviceId = decommissioning.DeviceId,
            DeviceName = device?.Name ?? "Unknown",
            DecommissioningRequestId = decommissioning.DecommissioningRequestId,
            DeviceReceiverId = decommissioning.DeviceReceiverId,
            DeviceReceiverName = receiver?.FullName ?? "Unknown",
            ReceiverDepartmentId = decommissioning.ReceiverDepartmentId,
            ReceiverDepartmentName = department?.Name ?? "Unknown",
            DecommissioningDate = decommissioning.DecommissioningDate,
            Reason = decommissioning.Reason,
            FinalDestination = decommissioning.FinalDestination
        };
    }
    private async Task<IEnumerable<DecommissioningDto>> MapDecommissioningsToDto(IEnumerable<Decommissioning> decommissionings)
    {
        var dtos = new List<DecommissioningDto>();
        foreach (var decommissioning in decommissionings)
        {
            dtos.Add(await MapDecommissioningToDto(decommissioning));
        }
        return dtos;
    }

    #endregion
}