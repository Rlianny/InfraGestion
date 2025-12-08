using Application.DTOs.Report;
using Application.Services.Interfaces;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IPdfReportGenerator pdfReportGenerator;
        private readonly IDeviceRepository deviceRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMaintenanceRecordRepository maintenanceRecordRepository;
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new();
        public ReportService(IPdfReportGenerator pdfReportGenerator,IDeviceRepository deviceRepository,IDepartmentRepository departmentRepository,IMaintenanceRecordRepository maintenanceRecordRepository)
        {
            this.pdfReportGenerator = pdfReportGenerator;
            this.deviceRepository = deviceRepository;
            this.departmentRepository = departmentRepository;
            this.maintenanceRecordRepository = maintenanceRecordRepository;
        }
       
        public async Task<PdfReportDTO> ExportToPdf<T>(IEnumerable<T> report)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = _propertyCache.GetOrAdd(type, t => t.GetProperties());

            List<string> headers = properties.Select(p => p.Name).ToList();
            List<TableRow> rows = new List<TableRow>();

            foreach (var row in report)
            {
                List<string> cells = new();
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(row);
                    cells.Add(propertyValue?.ToString() ?? "null");
                }
                rows.Add(new TableRow(cells));
            }

            Table table = new Table(headers, rows);
            byte[] pdfReportBytes = await pdfReportGenerator.CreatePdfTable(table);
            return new PdfReportDTO { PdfBytes = pdfReportBytes };
        }

        public async Task<IEnumerable<BonusDeterminationReportDto>> GenerateBonusDeterminationReportAsync(BonusReportCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CorrelationAnalysisReportDto>> GenerateCorrelationAnalysisReportAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DepartmentTransferReportDto>> GenerateDepartmentTransferReportAsync(string departmentId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DecommissioningReportDto>> GenerateDischargeReportAsync(DecommissioningReportFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DeviceReplacementReportDto>> GenerateEquipmentReplacementReportAsync()
        {
           
            var devices = await deviceRepository.GetAllAsync();
            var res = new List<DeviceReplacementReportDto>();   
            foreach (var device in devices)
            {
                var maintenances = await maintenanceRecordRepository.GetMaintenancesByDeviceAsync(device.DeviceId);
                if (maintenances.Count(m => m.Date.Year==DateTime.Now.Year)>3)
                {
                    res.Add(new DeviceReplacementReportDto(device.DeviceId, device.Name));
                }
                
            }
            return res;
        }

        public async Task<IEnumerable<DeviceReportDto>> GenerateInventoryReportAsync(DeviceReportFilterDto filter)
        {
            
            var devices = await deviceRepository.GetAllAsync();
            var res = new List<DeviceReportDto>();
            foreach (var device in devices)
            {
                
                var department = await departmentRepository.GetByIdAsync(device.DepartmentId)??throw new EntityNotFoundException("Department",device.DepartmentId);
                var section = department.Section??throw new EntityNotFoundException("Section",department.SectionId);
                var maintenanceCount = (await maintenanceRecordRepository.GetMaintenancesByDeviceAsync(device.DeviceId)).Count();
                var maintenanceCost = await maintenanceRecordRepository.GetTotalMaintenanceCostByDeviceAsync(device.DeviceId);  
                var lastMaintenance = (await maintenanceRecordRepository.GetMaintenancesByDeviceAsync(device.DeviceId)).MaxBy(x => x.Date);
                res.Add(
                    
                    new DeviceReportDto(device.DeviceId, device.Name, device.Type,
                                    device.OperationalState, device.DepartmentId, 
                                department.Name, section.SectionId, section.Name,
                                device.AcquisitionDate, maintenanceCount,maintenanceCost,lastMaintenance.Date));
            }
            return res;
        }

        public async Task<IEnumerable<PersonnelEffectivenessReportDto>> GeneratePersonnelEffectivenessReportAsync(PersonnelReportFilterDto criteria)
        {
            throw new NotImplementedException();
        }
    }
}
