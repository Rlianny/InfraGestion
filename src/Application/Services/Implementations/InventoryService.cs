using Application.DTOs.Inventory;
using Application.DTOs.Maintenance;
using Application.DTOs.Transfer;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Aggregations;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using static System.Collections.Specialized.BitVector32;

namespace Application.Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly IDeviceRepository deviceRepo;
        private readonly ISectionRepository sectionRepo;
        private readonly IUserRepository userRepo;
        private readonly IReceivingInspectionRequestRepository receivingInspectionRequestRepo;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMaintenanceRecordRepository maintenanceRepository;
        private readonly ITransferRepository transferRepository;
        private readonly IDecommissioningRequestRepository decommissioningRepository;

        public InventoryService(
            IDeviceRepository deviceRepo,
            IUserRepository userRepo,
            ISectionRepository sectionRepo,
            IReceivingInspectionRequestRepository receivingInspectionRequestRepo,
            IUnitOfWork unitOfWork,
            IDepartmentRepository departmentRepository,
            IMaintenanceRecordRepository maintenanceRecordRepository,
            IDecommissioningRequestRepository decommissioningRepository
            )
        {
            this.deviceRepo = deviceRepo;
            this.sectionRepo = sectionRepo;
            this.userRepo = userRepo;
            this.unitOfWork = unitOfWork;
            this.departmentRepository = departmentRepository;
            this.decommissioningRepository = decommissioningRepository;
            this.receivingInspectionRequestRepo = receivingInspectionRequestRepo;
        }

        public async Task ApproveDevice(int deviceID, int technicianID)
        {
            ReceivingInspectionRequest inspectionRequest = await receivingInspectionRequestRepo.GetReceivingInspectionRequestsByDeviceAsync(deviceID);
            if (technicianID != inspectionRequest.TechnicianID)
            {
                throw new ArgumentException($"The techinichian with id {technicianID} its not allowed to approve the device", "technicianID");
            }

            inspectionRequest.Accept();
            await receivingInspectionRequestRepo.UpdateAsync(inspectionRequest);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task AssignDeviceForReviewAsync(AssignDeviceForInspectionRequestDto inspectionRequestDto)
        {
            await receivingInspectionRequestRepo.AddAsync(new ReceivingInspectionRequest(DateTime.Now, inspectionRequestDto.DeviceId, inspectionRequestDto.AdministratorID, inspectionRequestDto.TechnicianId));
        }

        public async Task<IEnumerable<DeviceDto>> GetCompanyInventoryAsync()
        {
            var devices = await deviceRepo.GetAllAsync();
            return await Task.WhenAll(
                devices.Select(async device =>
                {
                    var department = await departmentRepository.GetByIdAsync(device.DepartmentID);
                    return new DeviceDto(device.DeviceID, device.Name, device.Type, device.OperationalState, department.Name);
                })
            );
        }

        public async Task<DeviceDetailDto> GetDeviceDetailAsync(int DeviceID)
        {
            ////TODO Refactor the code in a more declarative way
            //var device = await deviceRepo.GetByIdAsync(DeviceID);
            //var department = await departmentRepository.GetByIdAsync(device.DepartmentID);
            //var maintenanceHistory = await maintenanceRepository.GetDeviceMaintenanceHistoryAsync(device.DeviceID);
            //var transferHistory = await transferRepository.GetTransfersByDeviceAsync(device.DeviceID);
            //var decommisionings = await decommissioningRepository.GetDecommissioningsByDeviceAsync(device.DeviceID);
            //if (decommisionings.FirstOrDefault() == null)
            //{
            //    return new DeviceDetailDto(device.DeviceID, device.Name, device.Type, device.OperationalState,
            //   department.Name, maintenanceHistory, transferHistory, null);
            //}
            //var finalDecommisioning = decommisionings.First(x => x.FinalDestination != null);
            //return new DeviceDetailDto(device.DeviceID, device.Name, device.Type, device.OperationalState,
            //   department.Name, maintenanceHistory, transferHistory, new DTOs.Decommissioning.DecommissioningDto(finalDecommisioning.DecommissioningID, finalDecommisioning.DeviceReceiverID, finalDecommisioning.DecommissioningRequestID, finalDecommisioning.DeviceID, finalDecommisioning.DecommissioningDate, finalDecommisioning.Reason, finalDecommisioning.FinalDestination, finalDecommisioning.ReceiverDepartmentID));
            return await Task.FromResult(new DeviceDetailDto(1, "", DeviceType.ConnectivityAndNetwork, OperationalState.Operational, "deparment",Enumerable.Empty<MaintenanceRecordDto>(), Enumerable.Empty<TransferDto>(),null));


        }

        public async Task<IEnumerable<DeviceDto>> GetInventoryAsync(DeviceFilterDto filter, int UserId)
        {
            //TODO Refactor the code in a more declarative way and maybe search for a design pattern
            var devicesDTOs = new List<DeviceDto>();
            var devices = await deviceRepo.GetAllAsync();
            foreach (var device in devices)
            {
                if ((device.Type == filter.DeviceType || filter.DeviceType is null) &&
                    (device.OperationalState == filter.OperationalState || filter.OperationalState is null) &&
                    (device.DepartmentID == filter.DepartmentId || filter.DepartmentId is null)
                    )
                {
                    var department = await departmentRepository.GetByIdAsync(device.DepartmentID);

                    devicesDTOs.Add(new DeviceDto(device.DeviceID, device.Name, device.Type, device.OperationalState, department.Name));
                }
            }
            if (filter.OrderBy != null)
            {
                int start = filter.PageNumber * filter.PageSize;
                int end = start + filter.PageSize;
                switch (filter.OrderBy)
                {
                    case "Name":
                        var x = devicesDTOs.OrderBy(x => x.Name).Skip(start).Take(end);
                        return x;

                    case "DeviceId":
                        x = devicesDTOs.OrderBy(x => x.DeviceId).Skip(start).Take(end);
                        return x;
                    default:
                        throw new NotImplementedException();
                }
            }
            return devicesDTOs;
        }

        public async Task<IEnumerable<DeviceDto>> GetSectionInventoryAsync(int userID, int sectionId)
        {
            //TODO Refactor the code in a more declarative way and maybe search for a design pattern
            var user = await userRepo.GetByIdAsync(userID);
            var devices = await deviceRepo.GetAllAsync();
            if (user.Department.Section.SectionID != sectionId)
            {
                throw new Exception();
            }
            var deviceDtos = new List<DeviceDto>();
            foreach (var device in devices)
            {
                var department = await departmentRepository.GetByIdAsync(device.DepartmentID);
                if (department.Section.SectionID == sectionId)
                {
                    deviceDtos.Add(new DeviceDto(device.DeviceID, device.Name, device.Type, device.OperationalState, department.Name));
                }
            }
            return deviceDtos;
        }

        public async Task<IEnumerable<DeviceDto>> GetUsersOwnSectionInventory(int userID)
        {
            //TODO Refactor the code in a more declarative way and maybe search for a design pattern
            var user = await userRepo.GetByIdAsync(userID) ?? throw new Exception("User does not exist");
            var userDepartment = await departmentRepository.GetByIdAsync(user.DepartmentId) ?? throw new Exception("The given user does not exist");
            var devices = await deviceRepo.GetAllAsync();
            var deviceDtos = new List<DeviceDto>();
            foreach (var device in devices)
            {
                var deviceDepartment = await departmentRepository.GetByIdAsync(user.DepartmentId) ?? throw new Exception("User is not on that departament exist");
                if (userDepartment.SectionID == deviceDepartment.SectionID)
                {
                    deviceDtos.Add(new DeviceDto(device.DeviceID, device.Name, device.Type, device.OperationalState, deviceDepartment.Name));
                }
            }
            return deviceDtos;
        }

        public async Task RegisterDeviceAsync(InsertDeviceRequestDto request)
        {
            await deviceRepo.AddAsync(new Device(request.Name, request.DeviceType, OperationalState.Operational, request.DepartmentID, DateTime.Now));
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RejectDevice(int deviceID, int technicianID, string reason)
        {
            ReceivingInspectionRequest inspectionRequest = await receivingInspectionRequestRepo.GetReceivingInspectionRequestsByDeviceAsync(deviceID);
            User? tech = await userRepo.GetByIdAsync(technicianID);
            Device? device = await deviceRepo.GetByIdAsync(deviceID);
            if (tech == null || device == null || !tech.IsTechnician || technicianID != inspectionRequest.TechnicianID)
            {
                throw new Exception();
            }
            inspectionRequest.Reject(reason);
            await receivingInspectionRequestRepo.UpdateAsync(inspectionRequest);
        }

        public async Task UpdateEquipmentAsync(UpdateDeviceRequestDto request)
        {
            Device device = new Device(request.Name, request.DeviceType, request.OperationalState, request.DepartmentId, DateTime.Now, request.DeviceId);
            await deviceRepo.UpdateAsync(device);
        }
    }
}