using Application.DTOs.Transfer;
using Application.Services.Interfaces;
using Domain.Aggregations;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
namespace Application.Services.Implementations
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository transferRepository;
        private readonly IDeviceRepository deviceRepository;
        private readonly ISectionRepository sectionRepository;
        private readonly IUserRepository userRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IUnitOfWork unitOfWork;

        public TransferService(ITransferRepository transferRepository, IDeviceRepository deviceRepository, ISectionRepository sectionRepository,
                IUserRepository userRepository, IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
        {
            this.transferRepository = transferRepository;
            this.deviceRepository = deviceRepository;
            this.sectionRepository = sectionRepository;
            this.userRepository = userRepository;
            this.departmentRepository = departmentRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task ConfirmReceptionAsync(int transferId, string username)
        {
            var transfer = await transferRepository.GetByIdAsync(transferId)
                ?? throw new EntityNotFoundException("Transfer", transferId);
            var user = await userRepository.GetByUsernameAsync(username)
                ?? throw new EntityNotFoundException("User", username);
            if (user.UserId != transfer.DeviceReceiverId)
            {
                throw new UserValidationException($"User '{username}' is not the receiver of the device");
            }
            transfer.ConfirmReceipt();
            var device = await deviceRepository.GetByIdAsync(transfer.DeviceId)
                ?? throw new EntityNotFoundException("Device", transfer.DeviceId);
            device.UpdateOperationalState(OperationalState.Operational);
            await deviceRepository.UpdateAsync(device);
            await transferRepository.UpdateAsync(transfer);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTransferAsync(int transferId)
        {
            var transfer = await transferRepository.GetByIdAsync(transferId)??throw new EntityNotFoundException("Transfer",transferId);
            await transferRepository.DeleteAsync(transfer);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DesactivateTransferAsync(int transferId)
        {
            var transfer = await transferRepository.GetByIdAsync(transferId) ?? throw new EntityNotFoundException("Transfer", transferId);
            transfer.Disable();
            await transferRepository.UpdateAsync(transfer);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<TransferDto>> GetPendingTransfersAsync()
        {
            var transfers = await transferRepository.GetAllAsync();
            var pendingTransfers = transfers.Where(t => t.IsPending());

            var dtos = await Task.WhenAll(pendingTransfers.Select(async t =>
            {
                var dev = await deviceRepository.GetByIdAsync(t.DeviceId)
                          ?? throw new EntityNotFoundException("Device", t.TransferId);
                var src = await sectionRepository.GetByIdAsync(t.SourceSectionId)
                          ?? throw new EntityNotFoundException("Section", t.SourceSectionId);
                var dst = await sectionRepository.GetByIdAsync(t.DestinationSectionId)
                          ?? throw new EntityNotFoundException("Section", t.DestinationSectionId);
                var usr = await userRepository.GetByIdAsync(t.DeviceReceiverId)
                          ?? throw new EntityNotFoundException("User", t.DeviceReceiverId);

                return new TransferDto(
                    t.TransferId, t.DeviceId, dev.Name, t.Date,
                    t.SourceSectionId, src.Name,
                    t.DestinationSectionId, dst.Name,
                    t.DeviceReceiverId, usr.FullName, t.Status);
            }));

            return dtos;
        }

        public async Task<TransferDto> GetTransferByIdAsync(int transferId)
        {
            var transfer = await transferRepository.GetByIdAsync(transferId) ?? throw new EntityNotFoundException("Transfer", transferId);
            var device = await deviceRepository.GetByIdAsync(transfer.DeviceId) ?? throw new EntityNotFoundException("Device", transfer.DeviceId);
            var sourceSection = await sectionRepository.GetByIdAsync(transfer.SourceSectionId)
                          ?? throw new EntityNotFoundException("Section", transfer.SourceSectionId);
            var destinySection = await sectionRepository.GetByIdAsync(transfer.DestinationSectionId)
                      ?? throw new EntityNotFoundException("Section", transfer.DestinationSectionId);
            var user = await userRepository.GetByIdAsync(transfer.DeviceReceiverId)
                      ?? throw new EntityNotFoundException("User", transfer.DeviceReceiverId);
            return new TransferDto(
                transfer.TransferId, transfer.DeviceId, device.Name,
                transfer.Date, transfer.SourceSectionId,
                sourceSection.Name, transfer.DestinationSectionId,
                destinySection.Name, transfer.DeviceReceiverId,
                user.FullName, transfer.Status
                );
        }

        public async Task<IEnumerable<TransferDto>> GetTransfersByDeviceNameAsync(string deviceName)
        {
            var device = await deviceRepository.GetDeviceByNameAsync(deviceName)
                ?? throw new EntityNotFoundException("Device", deviceName);

            var transfers = await transferRepository.GetTransfersByDeviceAsync(device.DeviceId);
            var dtos = await Task.WhenAll(transfers.Select(async t =>
            {
                var src = await sectionRepository.GetByIdAsync(t.SourceSectionId)
                          ?? throw new EntityNotFoundException("Section", t.SourceSectionId);
                var dst = await sectionRepository.GetByIdAsync(t.DestinationSectionId)
                          ?? throw new EntityNotFoundException("Section", t.DestinationSectionId);
                var usr = await userRepository.GetByIdAsync(t.DeviceReceiverId)
                          ?? throw new EntityNotFoundException("User", t.DeviceReceiverId);

                return new TransferDto(
                    t.TransferId, t.DeviceId, device.Name, t.Date,
                    t.SourceSectionId, src.Name,
                    t.DestinationSectionId, dst.Name,
                    t.DeviceReceiverId, usr.FullName, t.Status);
            }));

            return dtos;
        }

        public async Task InitiateTransferAsync(CreateTransferRequestDto request)
        {
            var device = await deviceRepository.GetDeviceByNameAsync(request.DeviceName)
                ?? throw new EntityNotFoundException("Device", request.DeviceName);
            var sourceSection = await sectionRepository.GetSectionByNameAsync(request.SourceSectionName)
                ?? throw new EntityNotFoundException("Section", request.SourceSectionName);
            var destinationSection = await sectionRepository.GetSectionByNameAsync(request.DestinationSectionName)
                ?? throw new EntityNotFoundException("Section", request.DestinationSectionName);
            var receiver = await userRepository.GetByUsernameAsync(request.DeviceReceiverUsername)
                ?? throw new EntityNotFoundException("User", request.DeviceReceiverUsername);

            await transferRepository.AddAsync(new Transfer(
                request.TransferDate,
                device.DeviceId,
                sourceSection.SectionId,
                destinationSection.SectionId,
                receiver.UserId));

            device.UpdateOperationalState(OperationalState.BeingTransferred);
            await deviceRepository.UpdateAsync(device);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateEquipmentLocationAsync(string deviceName, string newDepartmentName)
        {
            var device = await deviceRepository.GetDeviceByNameAsync(deviceName)
                ?? throw new EntityNotFoundException("Device", deviceName);
            var department = await departmentRepository.GetDepartmentByNameAsync(newDepartmentName)
                ?? throw new EntityNotFoundException("Department", newDepartmentName);

            device.ChangeDepartment(department.DepartmentId);
            await deviceRepository.UpdateAsync(device);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
