# Guía Técnica: Implementación de Generación y Exportación de Reportes PDF

## Resumen Ejecutivo

Esta guía proporciona instrucciones detalladas para implementar la funcionalidad de generación y exportación de reportes en formato PDF para el proyecto **InfraGestion**, utilizando la librería **QuestPDF** (ya instalada en el proyecto).

---

## 1. Análisis del Estado Actual

### 1.1 Infraestructura Existente

| Componente | Estado | Ubicación |
|------------|--------|-----------|
| QuestPDF v2025.7.4 | ✅ Instalado | `Infrastructure.csproj` |
| `IReportService` | ✅ Definido | `Application/Services/Interfaces/` |
| `ExportToPdf<T>()` | ✅ Declarado | `IReportService.cs` |
| `PdfReportDTO` | ✅ Creado | `Application/DTOs/Report/` |
| `ReportService` | ❌ No implementado | Pendiente |
| `ReportController` | ❌ No existe | Pendiente |

### 1.2 DTOs de Reportes Disponibles

```
Application/DTOs/Report/
├── DeviceReportDto.cs              # Inventario de equipos
├── DecommissioningReportDto.cs     # Bajas técnicas
├── PersonnelEffectivenessReportDto.cs  # Efectividad de personal
├── DeviceReplacementReportDto.cs   # Reemplazo de equipos
├── DepartmentTransferReportDto.cs  # Transferencias por departamento
├── CorrelationAnalysisReportDto.cs # Análisis de correlación
├── BonusDeterminationReportDto.cs  # Determinación de bonos
└── PdfReportDTO.cs                 # Contenedor del PDF generado
```

---

## 2. Arquitectura Propuesta

### 2.1 Diagrama de Capas

```
┌─────────────────────────────────────────────────────────────┐
│                      Web.API Layer                          │
│  ┌─────────────────────────────────────────────────────┐   │
│  │              ReportController                        │   │
│  │  GET /api/reports/{type}                            │   │
│  │  GET /api/reports/{type}/pdf                        │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                    Application Layer                        │
│  ┌─────────────────────────────────────────────────────┐   │
│  │              IReportService                          │   │
│  │  - GenerateInventoryReportAsync()                   │   │
│  │  - GenerateDischargeReportAsync()                   │   │
│  │  - ExportToPdf<T>()                                 │   │
│  └─────────────────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────────────────┐   │
│  │              IPdfGeneratorService                    │   │
│  │  (Abstracción para generación de PDF)               │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                   Infrastructure Layer                      │
│  ┌─────────────────────────────────────────────────────┐   │
│  │           QuestPdfGeneratorService                   │   │
│  │  (Implementación concreta con QuestPDF)             │   │
│  └─────────────────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────────────────┐   │
│  │              ReportService                           │   │
│  │  (Implementación de IReportService)                 │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

### 2.2 Principios de Diseño

1. **Separación de Responsabilidades**: La generación de PDF es infraestructura; la lógica de negocio está en Application.
2. **Dependency Inversion**: Application define `IPdfGeneratorService`; Infrastructure lo implementa.
3. **Open/Closed**: Nuevos tipos de reportes se agregan sin modificar código existente.

---

## 3. Implementación Paso a Paso

### Paso 1: Definir la Interfaz del Generador PDF

**Archivo:** `src/Application/Services/Interfaces/ExternalServices.cs`

```csharp
// Agregar al archivo existente

/// <summary>
/// Servicio de generación de documentos PDF.
/// Abstracción para desacoplar la librería concreta (QuestPDF) de la capa Application.
/// </summary>
public interface IPdfGeneratorService
{
    /// <summary>
    /// Genera un PDF a partir de una colección de datos tabulares.
    /// </summary>
    /// <typeparam name="T">Tipo del DTO del reporte</typeparam>
    /// <param name="data">Colección de datos a incluir en el reporte</param>
    /// <param name="reportTitle">Título del reporte</param>
    /// <param name="columnMappings">Mapeo opcional de propiedades a nombres de columnas</param>
    /// <returns>Array de bytes del PDF generado</returns>
    byte[] GenerateTableReport<T>(
        IEnumerable<T> data, 
        string reportTitle,
        Dictionary<string, string>? columnMappings = null);
    
    /// <summary>
    /// Genera un PDF con diseño personalizado usando un documento predefinido.
    /// </summary>
    byte[] GenerateCustomReport(Action<IDocumentContainer> documentBuilder);
}
```

### Paso 2: Implementar el Servicio de Generación PDF

**Archivo:** `src/Infrastructure/Services/QuestPdfGeneratorService.cs`

```csharp
using System.Reflection;
using Application.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Services
{
    public class QuestPdfGeneratorService : IPdfGeneratorService
    {
        public QuestPdfGeneratorService()
        {
            // Configurar licencia de QuestPDF (Community es gratis para proyectos pequeños)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerateTableReport<T>(
            IEnumerable<T> data,
            string reportTitle,
            Dictionary<string, string>? columnMappings = null)
        {
            var items = data.ToList();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Crear mapeo de columnas si no se proporciona
            columnMappings ??= properties.ToDictionary(p => p.Name, p => FormatPropertyName(p.Name));

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    // Encabezado
                    page.Header().Element(c => ComposeHeader(c, reportTitle));

                    // Contenido - Tabla
                    page.Content().Element(c => ComposeTable(c, items, properties, columnMappings));

                    // Pie de página
                    page.Footer().Element(ComposeFooter);
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GenerateCustomReport(Action<IDocumentContainer> documentBuilder)
        {
            var document = Document.Create(documentBuilder);
            return document.GeneratePdf();
        }

        private void ComposeHeader(IContainer container, string title)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item()
                            .Text("InfraGestion")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        col.Item()
                            .Text("Sistema de Gestión de Infraestructura")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);
                    });

                    row.ConstantItem(100).AlignRight().Column(col =>
                    {
                        col.Item().Text(DateTime.Now.ToString("dd/MM/yyyy")).FontSize(10);
                        col.Item().Text(DateTime.Now.ToString("HH:mm")).FontSize(10);
                    });
                });

                column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Blue.Darken2);

                column.Item()
                    .PaddingTop(10)
                    .Text(title)
                    .FontSize(16)
                    .SemiBold()
                    .AlignCenter();

                column.Item().PaddingBottom(10);
            });
        }

        private void ComposeTable<T>(
            IContainer container,
            List<T> items,
            PropertyInfo[] properties,
            Dictionary<string, string> columnMappings)
        {
            if (!items.Any())
            {
                container.PaddingVertical(20).AlignCenter().Text("No hay datos para mostrar.")
                    .FontSize(12).Italic().FontColor(Colors.Grey.Darken1);
                return;
            }

            container.Table(table =>
            {
                // Definir columnas
                table.ColumnsDefinition(columns =>
                {
                    foreach (var _ in properties.Where(p => columnMappings.ContainsKey(p.Name)))
                    {
                        columns.RelativeColumn();
                    }
                });

                // Encabezados
                table.Header(header =>
                {
                    foreach (var prop in properties.Where(p => columnMappings.ContainsKey(p.Name)))
                    {
                        header.Cell()
                            .Background(Colors.Blue.Darken2)
                            .Padding(5)
                            .Text(columnMappings[prop.Name])
                            .FontColor(Colors.White)
                            .Bold()
                            .FontSize(9);
                    }
                });

                // Filas de datos
                var isAlternate = false;
                foreach (var item in items)
                {
                    var backgroundColor = isAlternate ? Colors.Grey.Lighten3 : Colors.White;

                    foreach (var prop in properties.Where(p => columnMappings.ContainsKey(p.Name)))
                    {
                        var value = prop.GetValue(item);
                        var displayValue = FormatValue(value);

                        table.Cell()
                            .Background(backgroundColor)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(5)
                            .Text(displayValue)
                            .FontSize(8);
                    }

                    isAlternate = !isAlternate;
                }
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                column.Item().PaddingTop(5).Row(row =>
                {
                    row.RelativeItem().Text(text =>
                    {
                        text.Span("Generado por InfraGestion - ");
                        text.Span(DateTime.Now.ToString("yyyy")).FontColor(Colors.Grey.Darken1);
                    });

                    row.RelativeItem().AlignRight().Text(text =>
                    {
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                });
            });
        }

        private static string FormatPropertyName(string name)
        {
            // Convierte "DeviceType" a "Device Type"
            return string.Concat(name.Select((c, i) =>
                i > 0 && char.IsUpper(c) ? " " + c : c.ToString()));
        }

        private static string FormatValue(object? value)
        {
            return value switch
            {
                null => "-",
                DateTime dt => dt.ToString("dd/MM/yyyy"),
                bool b => b ? "Sí" : "No",
                Enum e => e.ToString(),
                double d => d.ToString("N2"),
                decimal dec => dec.ToString("N2"),
                _ => value.ToString() ?? "-"
            };
        }
    }
}
```

### Paso 3: Implementar el ReportService

**Archivo:** `src/Application/Services/Implementations/ReportService.cs`

```csharp
using Application.DTOs.Report;
using Application.Services.Interfaces;
using Domain.Interfaces;

namespace Application.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDecommissioningRequestRepository _decommissioningRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly IPdfGeneratorService _pdfGenerator;

        public ReportService(
            IDeviceRepository deviceRepository,
            IDecommissioningRequestRepository decommissioningRepository,
            IUserRepository userRepository,
            ITransferRepository transferRepository,
            IPdfGeneratorService pdfGenerator)
        {
            _deviceRepository = deviceRepository;
            _decommissioningRepository = decommissioningRepository;
            _userRepository = userRepository;
            _transferRepository = transferRepository;
            _pdfGenerator = pdfGenerator;
        }

        public async Task<IEnumerable<DeviceReportDto>> GenerateInventoryReportAsync(
            DeviceReportFilterDto filter)
        {
            var devices = await _deviceRepository.GetAllWithDetailsAsync();

            // Aplicar filtros
            var query = devices.AsQueryable();

            if (filter.DepartmentId.HasValue)
                query = query.Where(d => d.DepartmentId == filter.DepartmentId);

            if (filter.DeviceType.HasValue)
                query = query.Where(d => d.DeviceType == filter.DeviceType);

            if (filter.OperationalState.HasValue)
                query = query.Where(d => d.OperationalState == filter.OperationalState);

            if (filter.FromDate.HasValue)
                query = query.Where(d => d.AcquisitionDate >= filter.FromDate);

            if (filter.ToDate.HasValue)
                query = query.Where(d => d.AcquisitionDate <= filter.ToDate);

            return query.Select(d => new DeviceReportDto(
                d.DeviceId,
                d.Name,
                d.DeviceType,
                d.OperationalState,
                d.DepartmentId,
                d.Department?.Name ?? string.Empty,
                d.Department?.SectionId,
                d.Department?.Section?.Name,
                d.AcquisitionDate,
                d.MaintenanceRecords?.Count ?? 0,
                d.MaintenanceRecords?.Sum(m => m.Cost) ?? 0,
                d.MaintenanceRecords?.Max(m => m.MaintenanceDate)
            )).ToList();
        }

        public async Task<IEnumerable<DecommissioningReportDto>> GenerateDischargeReportAsync(
            DecommissioningReportFilterDto filter)
        {
            var requests = await _decommissioningRepository.GetAllWithDetailsAsync();

            var query = requests.AsQueryable();

            if (filter.FromDate.HasValue)
                query = query.Where(r => r.RequestDate >= filter.FromDate);

            if (filter.ToDate.HasValue)
                query = query.Where(r => r.RequestDate <= filter.ToDate);

            if (filter.Reason.HasValue)
                query = query.Where(r => r.Reason == filter.Reason);

            return query.Select(r => new DecommissioningReportDto
            {
                EquipmentId = r.DeviceId,
                EquipmentName = r.Device?.Name ?? string.Empty,
                DecommissionCause = r.Reason.ToString(),
                FinalDestination = r.FinalDestination ?? string.Empty,
                ReceiverName = r.ReceiverName ?? string.Empty,
                DecommissionDate = r.RequestDate
            }).ToList();
        }

        public async Task<IEnumerable<PersonnelEffectivenessReportDto>> GeneratePersonnelEffectivenessReportAsync(
            PersonnelReportFilterDto criteria)
        {
            // TODO: Implementar lógica de cálculo de efectividad
            // Requiere definir métricas de efectividad en el dominio
            throw new NotImplementedException("Pendiente definición de métricas de efectividad");
        }

        public async Task<IEnumerable<DeviceReplacementReportDto>> GenerateEquipmentReplacementReportAsync()
        {
            // TODO: Implementar lógica de recomendación de reemplazo
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DepartmentTransferReportDto>> GenerateDepartmentTransferReportAsync(
            string departmentId)
        {
            // TODO: Implementar
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CorrelationAnalysisReportDto>> GenerateCorrelationAnalysisReportAsync()
        {
            // TODO: Implementar análisis estadístico
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BonusDeterminationReportDto>> GenerateBonusDeterminationReportAsync(
            BonusReportCriteria criteria)
        {
            // TODO: Implementar
            throw new NotImplementedException();
        }

        public Task<PdfReportDTO> ExportToPdf<T>(IEnumerable<T> data)
        {
            var reportTitle = GetReportTitle<T>();
            var columnMappings = GetColumnMappings<T>();

            var pdfBytes = _pdfGenerator.GenerateTableReport(data, reportTitle, columnMappings);

            return Task.FromResult(new PdfReportDTO { PdfBytes = pdfBytes });
        }

        private static string GetReportTitle<T>()
        {
            var typeName = typeof(T).Name;
            return typeName switch
            {
                nameof(DeviceReportDto) => "Reporte de Inventario de Equipos",
                nameof(DecommissioningReportDto) => "Reporte de Bajas Técnicas",
                nameof(PersonnelEffectivenessReportDto) => "Reporte de Efectividad de Personal",
                nameof(DeviceReplacementReportDto) => "Reporte de Reemplazo de Equipos",
                nameof(DepartmentTransferReportDto) => "Reporte de Transferencias por Departamento",
                nameof(CorrelationAnalysisReportDto) => "Análisis de Correlación",
                nameof(BonusDeterminationReportDto) => "Determinación de Bonificaciones",
                _ => $"Reporte - {typeName}"
            };
        }

        private static Dictionary<string, string>? GetColumnMappings<T>()
        {
            var typeName = typeof(T).Name;
            
            return typeName switch
            {
                nameof(DeviceReportDto) => new Dictionary<string, string>
                {
                    ["DeviceId"] = "ID",
                    ["Name"] = "Nombre",
                    ["DeviceType"] = "Tipo",
                    ["OperationalState"] = "Estado",
                    ["DepartmentName"] = "Departamento",
                    ["SectionName"] = "Sección",
                    ["AcquisitionDate"] = "Fecha Adquisición",
                    ["MaintenanceCount"] = "Mantenimientos",
                    ["TotalMaintenanceCost"] = "Costo Total Mant."
                },
                nameof(DecommissioningReportDto) => new Dictionary<string, string>
                {
                    ["EquipmentId"] = "ID Equipo",
                    ["EquipmentName"] = "Nombre",
                    ["DecommissionCause"] = "Causa",
                    ["FinalDestination"] = "Destino Final",
                    ["ReceiverName"] = "Receptor",
                    ["DecommissionDate"] = "Fecha"
                },
                _ => null // Usar nombres de propiedades por defecto
            };
        }
    }
}
```

### Paso 4: Crear el ReportController

**Archivo:** `src/Web.API/Controllers/ReportController.cs`

```csharp
using Application.DTOs.Report;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers
{
    [Authorize]
    public class ReportController : BaseApiController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Genera reporte de inventario de equipos
        /// </summary>
        [HttpGet("inventory")]
        public async Task<ActionResult<IEnumerable<DeviceReportDto>>> GetInventoryReport(
            [FromQuery] DeviceReportFilterDto filter)
        {
            var report = await _reportService.GenerateInventoryReportAsync(filter);
            return Ok(report);
        }

        /// <summary>
        /// Exporta reporte de inventario a PDF
        /// </summary>
        [HttpGet("inventory/pdf")]
        public async Task<IActionResult> ExportInventoryToPdf([FromQuery] DeviceReportFilterDto filter)
        {
            var data = await _reportService.GenerateInventoryReportAsync(filter);
            var pdf = await _reportService.ExportToPdf(data);

            return File(
                pdf.PdfBytes,
                "application/pdf",
                $"Inventario_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        /// <summary>
        /// Genera reporte de bajas técnicas
        /// </summary>
        [HttpGet("decommissioning")]
        public async Task<ActionResult<IEnumerable<DecommissioningReportDto>>> GetDecommissioningReport(
            [FromQuery] DecommissioningReportFilterDto filter)
        {
            var report = await _reportService.GenerateDischargeReportAsync(filter);
            return Ok(report);
        }

        /// <summary>
        /// Exporta reporte de bajas a PDF
        /// </summary>
        [HttpGet("decommissioning/pdf")]
        public async Task<IActionResult> ExportDecommissioningToPdf(
            [FromQuery] DecommissioningReportFilterDto filter)
        {
            var data = await _reportService.GenerateDischargeReportAsync(filter);
            var pdf = await _reportService.ExportToPdf(data);

            return File(
                pdf.PdfBytes,
                "application/pdf",
                $"Bajas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        /// <summary>
        /// Genera reporte de efectividad de personal
        /// </summary>
        [HttpGet("personnel-effectiveness")]
        public async Task<ActionResult<IEnumerable<PersonnelEffectivenessReportDto>>> GetPersonnelReport(
            [FromQuery] PersonnelReportFilterDto filter)
        {
            var report = await _reportService.GeneratePersonnelEffectivenessReportAsync(filter);
            return Ok(report);
        }

        /// <summary>
        /// Exporta reporte de personal a PDF
        /// </summary>
        [HttpGet("personnel-effectiveness/pdf")]
        public async Task<IActionResult> ExportPersonnelToPdf([FromQuery] PersonnelReportFilterDto filter)
        {
            var data = await _reportService.GeneratePersonnelEffectivenessReportAsync(filter);
            var pdf = await _reportService.ExportToPdf(data);

            return File(
                pdf.PdfBytes,
                "application/pdf",
                $"Efectividad_Personal_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        /// <summary>
        /// Genera reporte de equipos a reemplazar
        /// </summary>
        [HttpGet("equipment-replacement")]
        public async Task<ActionResult<IEnumerable<DeviceReplacementReportDto>>> GetReplacementReport()
        {
            var report = await _reportService.GenerateEquipmentReplacementReportAsync();
            return Ok(report);
        }

        /// <summary>
        /// Exporta reporte de reemplazo a PDF
        /// </summary>
        [HttpGet("equipment-replacement/pdf")]
        public async Task<IActionResult> ExportReplacementToPdf()
        {
            var data = await _reportService.GenerateEquipmentReplacementReportAsync();
            var pdf = await _reportService.ExportToPdf(data);

            return File(
                pdf.PdfBytes,
                "application/pdf",
                $"Reemplazo_Equipos_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        /// <summary>
        /// Genera reporte de transferencias por departamento
        /// </summary>
        [HttpGet("transfers/{departmentId}")]
        public async Task<ActionResult<IEnumerable<DepartmentTransferReportDto>>> GetTransferReport(
            string departmentId)
        {
            var report = await _reportService.GenerateDepartmentTransferReportAsync(departmentId);
            return Ok(report);
        }

        /// <summary>
        /// Exporta reporte de transferencias a PDF
        /// </summary>
        [HttpGet("transfers/{departmentId}/pdf")]
        public async Task<IActionResult> ExportTransferToPdf(string departmentId)
        {
            var data = await _reportService.GenerateDepartmentTransferReportAsync(departmentId);
            var pdf = await _reportService.ExportToPdf(data);

            return File(
                pdf.PdfBytes,
                "application/pdf",
                $"Transferencias_{departmentId}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        /// <summary>
        /// Genera análisis de correlación
        /// </summary>
        [HttpGet("correlation-analysis")]
        public async Task<ActionResult<IEnumerable<CorrelationAnalysisReportDto>>> GetCorrelationReport()
        {
            var report = await _reportService.GenerateCorrelationAnalysisReportAsync();
            return Ok(report);
        }

        /// <summary>
        /// Genera reporte de bonificaciones
        /// </summary>
        [HttpGet("bonus-determination")]
        public async Task<ActionResult<IEnumerable<BonusDeterminationReportDto>>> GetBonusReport(
            [FromQuery] BonusReportCriteria criteria)
        {
            var report = await _reportService.GenerateBonusDeterminationReportAsync(criteria);
            return Ok(report);
        }
    }
}
```

### Paso 5: Registrar Servicios en DI

**Modificar:** `src/Web.API/Program.cs`

Agregar en el método `InjectInfrastructure` o `InjectApplication`:

```csharp
// En InjectInfrastructure o método similar
builder.Services.AddScoped<IPdfGeneratorService, QuestPdfGeneratorService>();

// En InjectApplication o método similar  
builder.Services.AddScoped<IReportService, ReportService>();
```

---

## 4. Estructura de DTOs de Filtros

### DeviceReportFilterDto

```csharp
public class DeviceReportFilterDto
{
    public int? DepartmentId { get; set; }
    public int? SectionId { get; set; }
    public DeviceType? DeviceType { get; set; }
    public OperationalState? OperationalState { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
```

### DecommissioningReportFilterDto

```csharp
public class DecommissioningReportFilterDto
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public DecommissioningReason? Reason { get; set; }
    public DecommissioningStatus? Status { get; set; }
}
```

---

## 5. Consideraciones de QuestPDF

### 5.1 Licenciamiento

QuestPDF usa el modelo **Community License** que es gratuito para:
- Empresas/individuos con ingresos anuales < $1M USD
- Proyectos educativos y de código abierto

Configuración obligatoria al iniciar la aplicación:
```csharp
QuestPDF.Settings.License = LicenseType.Community;
```

### 5.2 Características Útiles

| Característica | Uso |
|----------------|-----|
| `Document.Create()` | Punto de entrada para crear PDFs |
| `Page.Size()` | A4, Letter, o tamaños personalizados |
| `Table()` | Tablas con columnas dinámicas |
| `Row()` / `Column()` | Layout flexible |
| `Image()` | Insertar logos e imágenes |
| `GeneratePdf()` | Devuelve `byte[]` |
| `GeneratePdfAndShow()` | Debug: abre el PDF |

### 5.3 Optimización de Rendimiento

```csharp
// Para reportes grandes, usar streaming
using var stream = new MemoryStream();
document.GeneratePdf(stream);
return stream.ToArray();

// Cachear fuentes si se usan personalizadas
QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;
```

---

## 6. Endpoints API Resultantes

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/reports/inventory` | Datos de inventario (JSON) |
| GET | `/api/reports/inventory/pdf` | Inventario en PDF |
| GET | `/api/reports/decommissioning` | Datos de bajas (JSON) |
| GET | `/api/reports/decommissioning/pdf` | Bajas en PDF |
| GET | `/api/reports/personnel-effectiveness` | Efectividad personal (JSON) |
| GET | `/api/reports/personnel-effectiveness/pdf` | Efectividad en PDF |
| GET | `/api/reports/equipment-replacement` | Equipos a reemplazar (JSON) |
| GET | `/api/reports/equipment-replacement/pdf` | Reemplazo en PDF |
| GET | `/api/reports/transfers/{deptId}` | Transferencias (JSON) |
| GET | `/api/reports/transfers/{deptId}/pdf` | Transferencias en PDF |

---

## 7. Pruebas Recomendadas

### 7.1 Pruebas Unitarias

```csharp
[Fact]
public void GenerateTableReport_WithValidData_ReturnsPdfBytes()
{
    // Arrange
    var service = new QuestPdfGeneratorService();
    var data = new List<DeviceReportDto> { /* ... */ };

    // Act
    var result = service.GenerateTableReport(data, "Test Report");

    // Assert
    Assert.NotNull(result);
    Assert.True(result.Length > 0);
    Assert.Equal(0x25, result[0]); // PDF magic number '%'
    Assert.Equal(0x50, result[1]); // 'P'
    Assert.Equal(0x44, result[2]); // 'D'
    Assert.Equal(0x46, result[3]); // 'F'
}
```

### 7.2 Pruebas de Integración

```csharp
[Fact]
public async Task ExportInventoryToPdf_ReturnsValidPdf()
{
    // Arrange
    var client = _factory.CreateClient();
    client.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", _token);

    // Act
    var response = await client.GetAsync("/api/reports/inventory/pdf");

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.Equal("application/pdf", response.Content.Headers.ContentType?.MediaType);
}
```

---

## 8. Checklist de Implementación

- [ ] Agregar `IPdfGeneratorService` a `ExternalServices.cs`
- [ ] Crear `QuestPdfGeneratorService.cs` en Infrastructure/Services
- [ ] Crear `ReportService.cs` en Application/Services/Implementations
- [ ] Crear `ReportController.cs` en Web.API/Controllers
- [ ] Completar DTOs de filtros faltantes
- [ ] Registrar servicios en DI (Program.cs)
- [ ] Agregar métodos de repositorio necesarios (`GetAllWithDetailsAsync`)
- [ ] Escribir pruebas unitarias
- [ ] Escribir pruebas de integración
- [ ] Documentar endpoints en Swagger

---

## 9. Referencias

- [QuestPDF Documentation](https://www.questpdf.com/documentation/)
- [QuestPDF GitHub](https://github.com/QuestPDF/QuestPDF)
- [Clean Architecture - Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

**Última actualización:** Diciembre 2024  
**Versión:** 1.0
