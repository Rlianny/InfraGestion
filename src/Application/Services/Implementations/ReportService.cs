using Application.DTOs.Report;
using Application.Services.Interfaces;
using Domain.Extensions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    internal class ReportService : IReportService
    {
        public ReportService(IPdfReportGenerator pdfReportGenerator) 
        {
            this.pdfReportGenerator = pdfReportGenerator;
        }
        
        private readonly IPdfReportGenerator pdfReportGenerator;

        public Task<IEnumerable<BonusDeterminationReportDto>> GenerateBonusDeterminationReportAsync(BonusReportCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CorrelationAnalysisReportDto>> GenerateCorrelationAnalysisReportAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DepartmentTransferReportDto>> GenerateDepartmentTransferReportAsync(string departmentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DecommissioningReportDto>> GenerateDischargeReportAsync(DecommissioningReportFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeviceReplacementReportDto>> GenerateEquipmentReplacementReportAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeviceReportDto>> GenerateInventoryReportAsync(DeviceReportFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public async Task<PdfExportDto> GeneratePdfReport(string reportType)
        {
            
            switch (reportType)
            {
                case "correlation-analysis":
                    var table = await GenerateCorrelationAnalysisReportAsync();
                    var pdf = await pdfReportGenerator.GeneratePdf(table);
                    return new PdfExportDto(pdf);
                default:
                    throw new Exception("Invalid report type");
            }
            
        }

        public Task<IEnumerable<PersonnelEffectivenessReportDto>> GeneratePersonnelEffectivenessReportAsync(PersonnelReportFilterDto criteria)
        {
           throw new NotImplementedException() 
           {
           
           };
        }
    }
}
