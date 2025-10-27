# Guía Rápida - Entity Framework Core

## 🚀 Inicio Rápido

### 1. Crear Migración Inicial
```powershell
cd "c:\Users\JL\Desktop\3er Año\IS\Proyecto\Prject Mirror\InfraGestion\src\Infrastructure"
dotnet ef migrations add InitialCreate
```

O ejecutar el script:
```powershell
.\CreateMigration.ps1
```

### 2. Actualizar Base de Datos
```powershell
dotnet ef database update
```

O ejecutar el script:
```powershell
.\UpdateDatabase.ps1
```

## 📊 Estructura de Base de Datos

### Tablas Principales
- **Users** - Con discriminador TPH (UserType)
  - Administrator
  - Director
  - Technician
  - SectionManager
  - DeviceReceiver
- **Departments**
- **Sections**
- **Devices**

### Tablas de Agregación
- **Transfers**
- **Mainteinances**
- **Decommissionings**
- **DecommissioningRequests**
- **ReceivingInspectionRequests**
- **Rejections**
- **Assessments**

## 💻 Uso en Código

### Inyección de Dependencias
```csharp
// En Program.cs o Startup.cs
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
```

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InfraGestionDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Consultas Básicas
```csharp
// Obtener todos los equipos con su departamento
var Devices = await _context.Devices
    .Include(e => e.Department)
        .ThenInclude(d => d.Section)
    .ToListAsync();

// Crear un nuevo equipo
var Device = new Device
{
    DeviceID = Guid.NewGuid(),
    Name = "Laptop Dell",
    Type = DeviceType.Informatical,
    OperationalState = OperationalState.Operational,
    DepartmentID = departmentId,
    AcquisitionDate = DateTime.Now
};
await _context.Devices.AddAsync(Device);
await _context.SaveChangesAsync();

// Actualizar un equipo
var Device = await _context.Devices.FindAsync(id);
Device.Name = "New Name";
await _context.SaveChangesAsync();

// Eliminar un equipo
var Device = await _context.Devices.FindAsync(id);
_context.Devices.Remove(Device);
await _context.SaveChangesAsync();
```

### Consultas con Joins
```csharp
// Mantenimientos con técnico y equipo
var maintenances = await _context.Mainteinances
    .Include(m => m.Technician)
    .Include(m => m.Device)
    .Where(m => m.Date >= startDate)
    .OrderByDescending(m => m.Date)
    .ToListAsync();
```

## 🔧 Comandos Útiles

### Ver migraciones aplicadas
```powershell
dotnet ef migrations list
```

### Generar script SQL
```powershell
dotnet ef migrations script
```

### Revertir migración
```powershell
dotnet ef database update PreviousMigrationName
```

### Eliminar última migración
```powershell
dotnet ef migrations remove
```

### Actualizar EF Core Tools
```powershell
dotnet tool update --global dotnet-ef
```

## 📝 Notas Importantes

1. **Restricciones de eliminación**: Todas las FK usan `DeleteBehavior.Restrict`
2. **Herencia TPH**: User y sus derivadas en una sola tabla
3. **Enums como String**: DeviceType y OperationalState se guardan como texto
4. **Navigation Properties**: Todas marcadas como `virtual` y nullable
5. **Claves compuestas**: En Mainteinance, DecommissioningRequest, ReceivingInspectionRequest, Rejection, Assessments

## 🎯 Próximos Pasos Recomendados

1. ✅ Crear migración inicial
2. ✅ Aplicar migración a DB
3. ⬜ Implementar Repository Pattern (opcional)
4. ⬜ Crear servicios de aplicación
5. ⬜ Implementar Unit of Work (opcional)
6. ⬜ Agregar validaciones de negocio
7. ⬜ Configurar logging
8. ⬜ Implementar auditoría de cambios
