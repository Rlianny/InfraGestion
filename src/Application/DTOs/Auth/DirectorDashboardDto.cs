using Domain.Enums;

namespace Application.DTOs.Auth;
/// <summary>
/// DTO que contiene toda la información necesaria para la vista del Dashboard del Director.
/// Este DTO es el que el backend debe proporcionar para poblar la vista estratégica.
/// </summary>
public class DirectorDashboardDto
{
    /// <summary>
    /// Estadísticas generales (tarjetas superiores)
    /// </summary>
    public DashboardSummaryDto Summary { get; set; } = new();

    /// <summary>
    /// Datos para la tarjeta "Visión General del Inventario"
    /// </summary>
    public InventoryOverviewDto InventoryOverview { get; set; } = new();

    /// <summary>
    /// Datos para la tarjeta "Análisis de Costos"
    /// </summary>
    public CostAnalysisDto CostAnalysis { get; set; } = new();

    /// <summary>
    /// Datos para la tarjeta "Análisis de Bajas"
    /// </summary>
    public DecommissionAnalysisDto DecommissionAnalysis { get; set; } = new();

    /// <summary>
    /// Datos para la tarjeta "Efectividad por Departamento"
    /// </summary>
    public DepartmentEffectivenessDto DepartmentEffectiveness { get; set; } = new();
}

/// <summary>
/// Estadísticas resumidas mostradas en las 4 tarjetas superiores
/// </summary>
public class DashboardSummaryDto
{
    /// <summary>
    /// Total de activos/equipos en el sistema
    /// </summary>
    public int TotalAssets { get; set; }

    /// <summary>
    /// Número total de departamentos
    /// </summary>
    public int TotalDepartments { get; set; }

    /// <summary>
    /// Número de bajas en el trimestre actual
    /// </summary>
    public int QuarterlyDecommissions { get; set; }

    /// <summary>
    /// Costo total de mantenimiento (en la moneda local)
    /// </summary>
    public double MaintenanceCost { get; set; }
}

/// <summary>
/// Datos de la tarjeta "Visión General del Inventario"
/// </summary>
public class InventoryOverviewDto
{
    /// <summary>
    /// Número de equipos operativos (funcionando correctamente)
    /// </summary>

    public int OperationalDevices { get; set; }

    /// <summary>
    /// Número de equipos actualmente en mantenimiento
    /// </summary>

    public int DevicesInMaintenance { get; set; }
}

/// <summary>
/// Datos de la tarjeta "Análisis de Costos"
/// </summary>
public class CostAnalysisDto
{
    /// <summary>
    /// Costo mensual promedio de mantenimiento
    /// </summary>

    public double MonthlyAverageCost { get; set; }

    /// <summary>
    /// Porcentaje de tendencia vs mes anterior (positivo = aumento, negativo = reducción)
    /// Ejemplo: -5.2 significa una reducción del 5.2%
    /// </summary>
    public double TrendPercentage { get; set; }
}

/// <summary>
/// Datos de la tarjeta "Análisis de Bajas"
/// </summary>
public class DecommissionAnalysisDto
{
    /// <summary>
    /// Lista de las principales causas de baja con su cantidad
    /// </summary>

    public List<DecommissionCauseDto> TopCauses { get; set; } = new();
}

/// <summary>
/// Representa una causa de baja con su cantidad
/// </summary>
public class DecommissionCauseDto
{
    /// <summary>
    /// Nombre/descripción de la causa de baja
    /// </summary>

    public DecommissioningReason Reason { get; set; }

    /// <summary>
    /// Cantidad de equipos dados de baja por esta causa
    /// </summary>

    public int Count { get; set; }
}

/// <summary>
/// Datos de la tarjeta "Efectividad por Departamento"
/// </summary>
public class DepartmentEffectivenessDto
{
    /// <summary>
    /// Lista de departamentos con su porcentaje de efectividad
    /// </summary>

    public List<DepartmentEffectivenessItemDto> Departments { get; set; } = new();
}

/// <summary>
/// Representa la efectividad de un departamento
/// </summary>
public class DepartmentEffectivenessItemDto
{
    /// <summary>
    /// Nombre del departamento
    /// </summary>

    public string DepartmentName { get; set; } = string.Empty;

    /// <summary>
    /// <summary>
    /// Cantidad de mantenimientos realizados por el departamento
    /// </summary>
    public int MaintenanceCount { get; set; }
}