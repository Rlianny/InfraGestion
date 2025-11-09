# Complete Guide to Role-Based Authentication & Authorization System
## Technical Equipment Management System for Telecommunications Company
## .NET Clean Architecture + Next.js TypeScript

**Author:** Senior Full-Stack Engineer  
**Target Audience:** Junior Developers  
**Last Updated:** October 2025  
**User:** Izengard

---

## 📋 Table of Contents

1. [Overview & Learning Objectives](#overview)
2. [Business Requirements Analysis](#business-analysis)
3. [Conceptual Foundation](#conceptual-foundation)
4. [Backend Implementation (.NET)](#backend-implementation)
5. [Frontend Implementation (Next.js)](#frontend-implementation)
6. [End-to-End Flow Example](#end-to-end-flow)
7. [Best Practices & Security](#security)
8. [Testing Strategy](#testing)
9. [Common Pitfalls & Troubleshooting](#troubleshooting)

---

<a name="overview"></a>
## 🎯 Overview & Learning Objectives

Welcome! In this comprehensive tutorial, we'll build a production-ready **Role-Based Access Control (RBAC)** system specifically designed for a **Technical Equipment Management System** for a telecommunications company. By the end, you'll understand:

- How to structure a .NET backend using Clean Architecture
- How to implement JWT-based authentication
- How to create role-based authorization tailored to business needs
- How to build secure, scalable authentication flows
- Real-world security considerations for enterprise applications

**What we're building:** A system that manages technical equipment inventory, maintenance, transfers, and downgrades with specific user roles matching the business requirements.

**Tech Stack:**
- **Backend:** .NET 8.0, Clean Architecture, Entity Framework Core, JWT
- **Frontend:** Next.js 14 (App Router), TypeScript, React 18
- **Database:** SQL Server (easily adaptable to PostgreSQL)

---

<a name="business-analysis"></a>
## 📊 Business Requirements Analysis

### 🏢 System Overview

The Technical Equipment Management System automates the management of technical downgrades, equipment inventory, maintenance processes, transfers, and personnel for a telecommunications company.

**Core Business Problems Solved:**
- ✅ Eliminates manual paper-based processes
- ✅ Prevents data loss from high data volume
- ✅ Automates equipment lifecycle tracking
- ✅ Provides accountability and audit trails
- ✅ Enables performance-based technician evaluation

---

### 👥 User Roles & Permissions Matrix

Based on the business description, we identify **5 distinct roles**:

| Role | Spanish Name | Key Responsibilities | Access Level |
|------|--------------|---------------------|--------------|
| **Director** | Director del Centro | Full system control, generate reports, view all data | **Full Access** |
| **Administrator** | Administrador | Manage inventory, equipment, downgrades, maintenance data | **High Access** |
| **Section Manager** | Responsable de Sección | Request equipment transfers, review area inventory | **Medium Access** |
| **Technician** | Técnico | Register maintenance, define equipment downgrades | **Operational Access** |
| **Equipment Receiver** | Receptor de Equipos | Receive equipment, register transfers, approve downgrades | **Specialized Access** |

---

### 🔐 Detailed Permission Mapping

Let's map business requirements to specific permissions:

#### **Director Permissions**
```
✓ View all system data
✓ Generate inventory reports
✓ Generate downgrade reports
✓ Generate technician performance reports
✓ View audit logs
✓ Manage all users
✓ Override any operation
```

#### **Administrator Permissions**
```
✓ Full CRUD on equipment inventory
✓ Full CRUD on downgrades
✓ Full CRUD on maintenance records
✓ Modify existing data
✓ Add new equipment
✓ View all departments
```

#### **Section Manager Permissions**
```
✓ Request equipment transfers
✓ View inventory in their section
✓ View equipment history in their area
✓ Approve maintenance requests
✗ Cannot modify equipment data directly
```

#### **Technician Permissions**
```
✓ Register maintenance interventions
✓ Create downgrade proposals
✓ View assigned equipment
✓ Update maintenance status
✗ Cannot approve downgrades
✗ Cannot transfer equipment
```

#### **Equipment Receiver Permissions**
```
✓ Register equipment reception
✓ Approve/reject downgrade proposals
✓ Register equipment transfers
✓ Assign receiving department
✓ View pending receptions
✗ Cannot modify inventory
```

💡 **Key Point:** Notice how permissions follow the **Principle of Least Privilege** - each role can only perform actions necessary for their job function.

---

### 📦 Core Domain Entities

Based on business requirements, we'll need these main entities:

```
┌─────────────────────────────────────────────────────────┐
│                     DOMAIN MODEL                        │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  User (Usuario)                                         │
│  ├── Role (Rol)                                         │
│  └── Department (Departamento)                          │
│                                                         │
│  Equipment (Equipo)                                     │
│  ├── EquipmentType (Tipo: Informático, Comunicaciones) │
│  ├── EquipmentStatus (Estado: Operativo, Baja)         │
│  ├── Location (Ubicación)                              │
│  └── AcquisitionDate (Fecha de Adquisición)            │
│                                                         │
│  Maintenance (Mantenimiento)                            │
│  ├── MaintenanceType (Tipo de Mantenimiento)           │
│  ├── Cost (Costo)                                       │
│  ├── Technician (Técnico Responsable)                  │
│  └── Equipment (Equipo)                                 │
│                                                         │
│  Downgrade (Baja Técnica)                               │
│  ├── Reason (Causa)                                     │
│  ├── Destination (Destino)                              │
│  ├── Receiver (Receptor)                                │
│  └── Approver (Quien Aprueba)                          │
│                                                         │
│  Transfer (Traslado)                                    │
│  ├── Origin (Origen)                                    │
│  ├── Destination (Destino)                              │
│  ├── Sender (Remitente)                                 │
│  └── Receiver (Receptor)                                │
│                                                         │
│  Technician Performance (Rendimiento Técnico)           │
│  ├── Rating (Valoración)                                │
│  ├── Bonus/Penalty (Bonificación/Penalización)         │
│  └── Evaluator (Evaluador)                             │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

---

<a name="conceptual-foundation"></a>
## 1️⃣ Conceptual Foundation

### 🔐 Authentication vs Authorization

Think of a telecommunications facility with multiple security checkpoints:

**Authentication** = Proving who you are at the entrance  
*"Hi, I'm Juan the Technician. Here's my employee badge."*  
You verify your identity with credentials (username/password).

**Authorization** = What rooms and systems you can access  
*"Juan is a Technician, so he can access the equipment room and maintenance logs, but NOT the director's office or financial reports."*  
Once authenticated, the system checks what permissions your role grants.

💡 **Key Point:** Authentication comes first, authorization comes second. You must know WHO someone is before deciding WHAT they can do.

---

### 🎭 Role-Based Access Control (RBAC)

RBAC is a security model where permissions are grouped into **roles**, and users are assigned roles that match their job function.

**Why RBAC for this system?**
- ✅ **Clear organizational hierarchy:** Matches the company structure (Director → Administrator → Section Manager → Technician)
- ✅ **Audit compliance:** Required for telecommunications regulatory compliance
- ✅ **Reduced errors:** Technicians can't accidentally delete inventory, receivers can't modify equipment specs
- ✅ **Scalability:** Add new technicians easily by assigning the "Technician" role
- ✅ **Security:** Sensitive operations (reports, deletions) restricted to authorized personnel

---

### 🏛️ Why Clean Architecture Matters

Clean Architecture (by Uncle Bob) organizes code into layers with clear dependencies, perfect for complex business systems like ours:

```
┌─────────────────────────────────────────────────────────┐
│              Presentation Layer (API)                   │
│     Controllers: Auth, Equipment, Maintenance, etc.     │
├─────────────────────────────────────────────────────────┤
│           Application Layer (Use Cases)                 │
│   RegisterDowngrade, TransferEquipment, GenerateReport  │
├─────────────────────────────────────────────────────────┤
│              Domain Layer (Entities)                    │
│   Equipment, User, Maintenance, Downgrade, Transfer     │
└─────────────────────────────────────────────────────────┘
                     ↑ depends on ↑

        Infrastructure Layer (Database, JWT, Email)
```

**Benefits for our Equipment Management System:**
- 🔒 **Business rules protected:** Equipment downgrade rules can't be bypassed by infrastructure changes
- 🧪 **Testability:** Test maintenance cost calculations without database
- 🔄 **Flexibility:** Replace email service without touching business logic
- 📖 **Maintainability:** New developers understand where to add features
- 🌍 **Compliance:** Easy to demonstrate separation of concerns for audits

---

### 🛡️ Security Principles for Telecommunications Systems

1. **Principle of Least Privilege:** Technicians see only their assigned equipment
2. **Defense in Depth:** Frontend AND backend validate role permissions
3. **Never Trust the Client:** Always validate on server (malicious technician could modify JS)
4. **Secure by Default:** New equipment starts as "restricted access"
5. **Fail Securely:** System errors deny access, don't grant it
6. **Audit Everything:** Log who did what, when (regulatory requirement)
7. **Data Integrity:** Equipment history cannot be modified, only appended

⚠️ **Warning:** In telecommunications, equipment data is often regulated. NEVER bypass authorization checks. A technician should NEVER see equipment outside their section, even if they know the equipment Id!

---

<a name="backend-implementation"></a>
## 2️⃣ Backend Implementation (.NET Clean Architecture)

### 📁 Project Structure

Create a solution with 4 projects:

```
EquipmentManagement/
├── src/
│   ├── EquipmentManagement.Domain/              # Core entities
│   ├── EquipmentManagement.Application/         # Use cases
│   ├── EquipmentManagement.Infrastructure/      # Database, JWT
│   └── EquipmentManagement.API/                 # Controllers
├── tests/
│   ├── EquipmentManagement.UnitTests/
│   └── EquipmentManagement.IntegrationTests/
└── EquipmentManagement.sln
```

**Create the solution:**

```bash
# Create solution
dotnet new sln -n EquipmentManagement

# Create projects
dotnet new classlib -n EquipmentManagement.Domain -o src/EquipmentManagement.Domain
dotnet new classlib -n EquipmentManagement.Application -o src/EquipmentManagement.Application
dotnet new classlib -n EquipmentManagement.Infrastructure -o src/EquipmentManagement.Infrastructure
dotnet new webapi -n EquipmentManagement.API -o src/EquipmentManagement.API

# Add to solution
dotnet sln add src/EquipmentManagement.Domain/EquipmentManagement.Domain.csproj
dotnet sln add src/EquipmentManagement.Application/EquipmentManagement.Application.csproj
dotnet sln add src/EquipmentManagement.Infrastructure/EquipmentManagement.Infrastructure.csproj
dotnet sln add src/EquipmentManagement.API/EquipmentManagement.API.csproj

# Add project references
cd src/EquipmentManagement.Application
dotnet add reference ../EquipmentManagement.Domain/EquipmentManagement.Domain.csproj

cd ../EquipmentManagement.Infrastructure
dotnet add reference ../EquipmentManagement.Domain/EquipmentManagement.Domain.csproj
dotnet add reference ../EquipmentManagement.Application/EquipmentManagement.Application.csproj

cd ../EquipmentManagement.API
dotnet add reference ../EquipmentManagement.Application/EquipmentManagement.Application.csproj
dotnet add reference ../EquipmentManagement.Infrastructure/EquipmentManagement.Infrastructure.csproj
```

---

### 🏗️ Domain Layer

**Purpose:** Contains enterprise-wide business rules and entities. No dependencies on other layers.

#### **File: `src/EquipmentManagement.Domain/Enums/RoleEnum.cs`**

```csharp
namespace EquipmentManagement.Domain.Enums;





public enum RoleEnum
{
    
    
    
    Technician = 1,
    
    
    
    
    EquipmentReceiver = 2,
    
    
    
    
    SectionManager = 3,
    
    
    
    
    Administrator = 4,
    
    
    
    
    Director = 5
}
```

#### **File: `src/EquipmentManagement.Domain/Enums/Permissions.cs`**

```csharp
namespace EquipmentManagement.Domain.Enums;




public static class Permissions
{
    // Equipment Inventory Permissions
    public const string ViewEquipment = "equipment:view";
    public const string ViewAllEquipment = "equipment:view-all";
    public const string ViewSectionEquipment = "equipment:view-section";
    public const string CreateEquipment = "equipment:create";
    public const string EditEquipment = "equipment:edit";
    public const string DeleteEquipment = "equipment:delete";
    
    // Maintenance Permissions
    public const string ViewMaintenance = "maintenance:view";
    public const string RegisterMaintenance = "maintenance:register";
    public const string EditMaintenance = "maintenance:edit";
    public const string ApproveMaintenance = "maintenance:approve";
    
    // Downgrade (Baja) Permissions
    public const string ViewDowngrades = "downgrade:view";
    public const string ProposeDowngrade = "downgrade:propose";
    public const string ApproveDowngrade = "downgrade:approve";
    public const string RejectDowngrade = "downgrade:reject";
    public const string EditDowngrade = "downgrade:edit";
    
    // Transfer Permissions
    public const string ViewTransfers = "transfer:view";
    public const string RequestTransfer = "transfer:request";
    public const string ApproveTransfer = "transfer:approve";
    public const string RegisterReception = "transfer:register-reception";
    
    // Technician Performance Permissions
    public const string ViewPerformance = "performance:view";
    public const string EvaluateTechnician = "performance:evaluate";
    
    // Reporting Permissions
    public const string GenerateInventoryReport = "report:inventory";
    public const string GenerateDowngradeReport = "report:downgrade";
    public const string GeneratePerformanceReport = "report:performance";
    public const string ViewAllReports = "report:view-all";
    
    // User Management Permissions
    public const string ManageUsers = "users:manage";
    public const string ViewUsers = "users:view";
    
    // Audit Permissions
    public const string ViewAuditLogs = "audit:view";
}
```

#### **File: `src/EquipmentManagement.Domain/Enums/EquipmentStatus.cs`**

```csharp
namespace EquipmentManagement.Domain.Enums;




public enum EquipmentStatus
{
    
    
    
    Operational = 1,
    
    
    
    
    UnderMaintenance = 2,
    
    
    
    
    Downgraded = 3,
    
    
    
    
    InTransfer = 4
}
```

#### **File: `src/EquipmentManagement.Domain/Enums/EquipmentType.cs`**

```csharp
namespace EquipmentManagement.Domain.Enums;




public enum EquipmentType
{
    
    
    
    Computer = 1,
    
    
    
    
    Communications = 2,
    
    
    
    
    Electrical = 3,
    
    
    
    
    Other = 4
}
```

#### **File: `src/EquipmentManagement.Domain/Entities/User.cs`**

```csharp
namespace EquipmentManagement.Domain.Entities;




public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    // Employee identification (número de identificación)
    public string EmployeeId { get; set; } = string.Empty;
    
    // Role relationship
    public Role Role { get; set; } = null!;
    public int RoleId { get; set; }
    
    // Department assignment (for Section Managers)
    public Department? Department { get; set; }
    public int? DepartmentId { get; set; }
    
    // Technician-specific fields
    public int? YearsOfExperience { get; set; }
    public string? Specialty { get; set; }
    
    // Audit fields
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Refresh token for JWT
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    
    // Navigation properties
    public ICollection<Maintenance> MaintenancesPerformed { get; set; } = new List<Maintenance>();
    public ICollection<Downgrade> DowngradesProposed { get; set; } = new List<Downgrade>();
    public ICollection<TechnicianPerformance> PerformanceRecords { get; set; } = new List<TechnicianPerformance>();

    // Helper properties
    public string FullName => $"{FirstName} {LastName}";
    public bool IsTechnician => RoleId == (int)Enums.RoleEnum.Technician;
}
```

#### **File: `src/EquipmentManagement.Domain/Entities/Role.cs`**

```csharp
namespace EquipmentManagement.Domain.Entities;




public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Permissions stored as JSON array
    public List<string> Permissions { get; set; } = new();
    
    // Navigation property
    public ICollection<User> Users { get; set; } = new List<User>();
}
```

#### **File: `src/EquipmentManagement.Domain/Entities/Department.cs`**

```csharp
namespace EquipmentManagement.Domain.Entities;




public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // Section manager
    public Guid? ManagerId { get; set; }
    public User? Manager { get; set; }
    
    // Navigation properties
    public ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
    public ICollection<User> Staff { get; set; } = new List<User>();
}
```

#### **File: `src/EquipmentManagement.Domain/Entities/Equipment.cs`**

```csharp
using EquipmentManagement.Domain.Enums;

namespace EquipmentManagement.Domain.Entities;





public class Equipment
{
    public Guid Id { get; set; }
    
    // Identificador único del equipo
    public string EquipmentCode { get; set; } = string.Empty;
    
    // Nombre del equipo
    public string Name { get; set; } = string.Empty;
    
    // Tipo (informático, de comunicaciones, eléctrico, etc.)
    public EquipmentType Type { get; set; }
    
    // Estado (operativo, en mantenimiento, dado de baja)
    public EquipmentStatus Status { get; set; }
    
    // Ubicación actual
    public Department CurrentLocation { get; set; } = null!;
    public int CurrentLocationId { get; set; }
    
    // Fecha de adquisición
    public DateTime AcquisitionDate { get; set; }
    
    // Additional details
    public string? SerialNumber { get; set; }
    public string? Model { get; set; }
    public string? Manufacturer { get; set; }
    public decimal? PurchasePrice { get; set; }
    
    // Audit fields
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    
    // Navigation properties - Historial
    public ICollection<Maintenance> MaintenanceHistory { get; set; } = new List<Maintenance>();
    public ICollection<Transfer> TransferHistory { get; set; } = new List<Transfer>();
    public ICollection<Downgrade> Downgrades { get; set; } = new List<Downgrade>();
}
```

#### **File: `src/EquipmentManagement.Domain/Entities/Maintenance.cs`**

```csharp
namespace EquipmentManagement.Domain.Entities;





public class Maintenance
{
    public Guid Id { get; set; }
    
    // Equipment being maintained
    public Equipment Equipment { get; set; } = null!;
    public Guid EquipmentId { get; set; }
    
    // Technician responsible
    public User Technician { get; set; } = null!;
    public Guid TechnicianId { get; set; }
    
    // Fecha del mantenimiento
    public DateTime MaintenanceDate { get; set; }
    
    // Tipo de mantenimiento
    public string MaintenanceType { get; set; } = string.Empty; // Preventive, Corrective, Emergency
    
    // Descripción de la intervención
    public string Description { get; set; } = string.Empty;
    
    // Costo asociado
    public decimal Cost { get; set; }
    
    // Status
    public string Status { get; set; } = string.Empty; // Pending, In Progress, Completed
    
    // Approval (if required by Section Manager)
    public bool RequiresApproval { get; set; }
    public bool? IsApproved { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    
    // Parts used
    public string? PartsUsed { get; set; }
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

#### **File: `src/EquipmentManagement.Domain/Entities/Downgrade.cs`**

```csharp
namespace EquipmentManagement.Domain.Entities;




public class Downgrade
{
    public Guid Id { get; set; }
    
    // Equipment being downgraded
    public Equipment Equipment { get; set; } = null!;
    public Guid EquipmentId { get; set; }
    
    // Causa de la baja (fallo técnico irreparable, obsolescencia, etc.)
    public string Reason { get; set; } = string.Empty;
    
    // Fecha de la baja
    public DateTime DowngradeDate { get; set; }
    
    // Destino final del equipo (almacén, desecho, traslado a otra unidad)
    public string Destination { get; set; } = string.Empty;
    
    // Proposed by (typically a Technician)
    public User ProposedBy { get; set; } = null!;
    public Guid ProposedById { get; set; }
    
    // Approved by (Equipment Receiver or Administrator)
    public User? ApprovedBy { get; set; }
    public Guid? ApprovedById { get; set; }
    public DateTime? ApprovedAt { get; set; }
    
    // Status
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    
    // Persona que recibe el equipo en su destino
    public User? ReceivedBy { get; set; }
    public Guid? ReceivedById { get; set; }
    public DateTime? ReceivedAt { get; set; }
    
    // Receiving department
    public Department? ReceivingDepartment { get; set; }
    public int? ReceivingDepartmentId { get; set; }
    
    // Additional notes
    public string? Notes { get; set; }
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

#### **File: `src/EquipmentManagement.Domain/Entities/Transfer.cs`**

```csharp
namespace EquipmentManagement.Domain.Entities;





public class Transfer
{
    public Guid Id { get; set; }
    
    // Equipment being transferred
    public Equipment Equipment { get; set; } = null!;
    public Guid EquipmentId { get; set; }
    
    // Fecha del traslado
    public DateTime TransferDate { get; set; }
    
    // Origen
    public Department OriginDepartment { get; set; } = null!;
    public int OriginDepartmentId { get; set; }
    
    // Destino
    public Department DestinationDepartment { get; set; } = null!;
    public int DestinationDepartmentId { get; set; }
    
    // Personal responsable del envío (Section Manager)
    public User RequestedBy { get; set; } = null!;
    public Guid RequestedById { get; set; }
    
    // Receptor del equipo (Equipment Receiver)
    public User? ReceivedBy { get; set; }
    public Guid? ReceivedById { get; set; }
    public DateTime? ReceivedAt { get; set; }
    
    // Status
    public string Status { get; set; } = "Requested"; // Requested, In Transit, Completed
    
    // Reason for transfer
    public string Reason { get; set; } = string.Empty;
    
    // Approval
    public bool RequiresApproval { get; set; } = true;
    public bool? IsApproved { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

#### **File: `src/EquipmentManagement.Domain/Entities/TechnicianPerformance.cs`**

```csharp
namespace EquipmentManagement.Domain.Entities;





public class TechnicianPerformance
{
    public Guid Id { get; set; }
    
    // Technician being evaluated
    public User Technician { get; set; } = null!;
    public Guid TechnicianId { get; set; }
    
    // Evaluator (typically Section Manager or Director)
    public User EvaluatedBy { get; set; } = null!;
    public Guid EvaluatedById { get; set; }
    
    // Evaluation period
    public DateTime EvaluationDate { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    
    // Valoración de superiores (1-5 scale)
    public int Rating { get; set; }
    
    // Performance metrics
    public int MaintenancesCompleted { get; set; }
    public decimal AverageMaintenanceTime { get; set; } // hours
    public int DowngradesProposed { get; set; }
    public int DowngradesApproved { get; set; }
    
    // Bonificaciones o penalizaciones
    public decimal SalaryAdjustment { get; set; } // Can be positive (bonus) or negative (penalty)
    public string SalaryAdjustmentReason { get; set; } = string.Empty;
    
    // Comments
    public string? EvaluatorComments { get; set; }
    
    // Audit
    public DateTime CreatedAt { get; set; }
}
```

💡 **Key Point:** Notice how domain entities match exactly with business requirements. Each entity represents a real-world concept from the telecommunications company.

---

### 🎯 Application Layer

#### **File: `src/EquipmentManagement.Application/DTOs/Auth/LoginRequest.cs`**

```csharp
using System.ComponentModel.DataAnnotations;

namespace EquipmentManagement.Application.DTOs.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = string.Empty;
}
```

#### **File: `src/EquipmentManagement.Application/DTOs/Auth/RegisterRequest.cs`**

```csharp
using System.ComponentModel.DataAnnotations;

namespace EquipmentManagement.Application.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    public string EmployeeId { get; set; } = string.Empty;
    
    // Role assignment (only Director/Admin can set this)
    public int? RoleId { get; set; }
    
    // Department assignment (for Section Managers)
    public int? DepartmentId { get; set; }
    
    // Technician-specific fields
    public int? YearsOfExperience { get; set; }
    public string? Specialty { get; set; }
}
```

#### **File: `src/EquipmentManagement.Application/DTOs/Auth/UserDto.cs`**

```csharp
namespace EquipmentManagement.Application.DTOs.Auth;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public List<string> Permissions { get; set; } = new();
    public string? DepartmentName { get; set; }
    public int? DepartmentId { get; set; }
    public int? YearsOfExperience { get; set; }
    public string? Specialty { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
```

#### **File: `src/EquipmentManagement.Application/DTOs/Auth/LoginResponse.cs`**

```csharp
namespace EquipmentManagement.Application.DTOs.Auth;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = null!;
}
```

#### **File: `src/EquipmentManagement.Application/DTOs/Auth/RefreshTokenRequest.cs`**

```csharp
using System.ComponentModel.DataAnnotations;

namespace EquipmentManagement.Application.DTOs.Auth;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
```

#### **File: `src/EquipmentManagement.Application/Common/Result.cs`**

```csharp
namespace EquipmentManagement.Application.Common;




public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string ErrorMessage { get; }
    public List<string> Errors { get; }

    private Result(bool isSuccess, T? data, string errorMessage, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
        Errors = errors ?? new List<string>();
    }

    public static Result<T> Success(T data) => new(true, data, string.Empty);
    
    public static Result<T> Failure(string error) => new(false, default, error);
    
    public static Result<T> Failure(List<string> errors) => 
        new(false, default, "Multiple errors occurred", errors);
}
```

#### **File: `src/EquipmentManagement.Application/Interfaces/IAuthService.cs`**

```csharp
using EquipmentManagement.Application.DTOs.Auth;
using EquipmentManagement.Application.Common;

namespace EquipmentManagement.Application.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result<LoginResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result<bool>> RevokeTokenAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
```

#### **File: `src/EquipmentManagement.Application/Interfaces/ITokenService.cs`**

```csharp
using EquipmentManagement.Domain.Entities;
using System.Security.Claims;

namespace EquipmentManagement.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
}
```

---

### 🔧 Infrastructure Layer

#### **NuGet Packages:**

```bash
cd src/EquipmentManagement.Infrastructure

dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package System.IdentityModel.Tokens.Jwt --version 7.0.3
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add package BCrypt.Net-Next --version 4.0.3
```

#### **File: `src/EquipmentManagement.Infrastructure/Data/ApplicationDbContext.cs`**

```csharp
using EquipmentManagement.Domain.Entities;
using EquipmentManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EquipmentManagement.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<Maintenance> Maintenances => Set<Maintenance>();
    public DbSet<Downgrade> Downgrades => Set<Downgrade>();
    public DbSet<Transfer> Transfers => Set<Transfer>();
    public DbSet<TechnicianPerformance> TechnicianPerformances => Set<TechnicianPerformance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.EmployeeId).IsUnique();
            
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.EmployeeId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PasswordHash).IsRequired();
            
            // Relationship with Role
            entity.HasOne(e => e.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Relationship with Department (optional, for Section Managers)
            entity.HasOne(e => e.Department)
                .WithMany(d => d.Staff)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Role entity
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            
            // Store permissions as JSON
            entity.Property(e => e.Permissions)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>()
                );
        });

        // Configure Department entity
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
            
            // Manager relationship
            entity.HasOne(e => e.Manager)
                .WithMany()
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Equipment entity
        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.EquipmentCode).IsUnique();
            
            entity.Property(e => e.EquipmentCode).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            
            // Location relationship
            entity.HasOne(e => e.CurrentLocation)
                .WithMany(d => d.Equipment)
                .HasForeignKey(e => e.CurrentLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Maintenance entity
        modelBuilder.Entity<Maintenance>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Equipment relationship
            entity.HasOne(e => e.Equipment)
                .WithMany(eq => eq.MaintenanceHistory)
                .HasForeignKey(e => e.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Technician relationship
            entity.HasOne(e => e.Technician)
                .WithMany(u => u.MaintenancesPerformed)
                .HasForeignKey(e => e.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.Property(e => e.MaintenanceType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Cost).HasPrecision(18, 2);
        });

        // Configure Downgrade entity
        modelBuilder.Entity<Downgrade>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Equipment relationship
            entity.HasOne(e => e.Equipment)
                .WithMany(eq => eq.Downgrades)
                .HasForeignKey(e => e.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // ProposedBy relationship
            entity.HasOne(e => e.ProposedBy)
                .WithMany(u => u.DowngradesProposed)
                .HasForeignKey(e => e.ProposedById)
                .OnDelete(DeleteBehavior.Restrict);
            
            // ApprovedBy relationship (optional)
            entity.HasOne(e => e.ApprovedBy)
                .WithMany()
                .HasForeignKey(e => e.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);
            
            // ReceivedBy relationship (optional)
            entity.HasOne(e => e.ReceivedBy)
                .WithMany()
                .HasForeignKey(e => e.ReceivedById)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Receiving Department relationship (optional)
            entity.HasOne(e => e.ReceivingDepartment)
                .WithMany()
                .HasForeignKey(e => e.ReceivingDepartmentId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.Property(e => e.Reason).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Destination).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
        });

        // Configure Transfer entity
        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Equipment relationship
            entity.HasOne(e => e.Equipment)
                .WithMany(eq => eq.TransferHistory)
                .HasForeignKey(e => e.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Origin department
            entity.HasOne(e => e.OriginDepartment)
                .WithMany()
                .HasForeignKey(e => e.OriginDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Destination department
            entity.HasOne(e => e.DestinationDepartment)
                .WithMany()
                .HasForeignKey(e => e.DestinationDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // RequestedBy relationship
            entity.HasOne(e => e.RequestedBy)
                .WithMany()
                .HasForeignKey(e => e.RequestedById)
                .OnDelete(DeleteBehavior.Restrict);
            
            // ReceivedBy relationship (optional)
            entity.HasOne(e => e.ReceivedBy)
                .WithMany()
                .HasForeignKey(e => e.ReceivedById)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.Property(e => e.Reason).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
        });

        // Configure TechnicianPerformance entity
        modelBuilder.Entity<TechnicianPerformance>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Technician relationship
            entity.HasOne(e => e.Technician)
                .WithMany(u => u.PerformanceRecords)
                .HasForeignKey(e => e.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // EvaluatedBy relationship
            entity.HasOne(e => e.EvaluatedBy)
                .WithMany()
                .HasForeignKey(e => e.EvaluatedById)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.Property(e => e.Rating).IsRequired();
            entity.Property(e => e.SalaryAdjustment).HasPrecision(18, 2);
        });

        // Seed data
        SeedRoles(modelBuilder);
        SeedDepartments(modelBuilder);
    }

    private void SeedRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = 1,
                Name = "Technician",
                Description = "Técnico - Registers maintenance and proposes downgrades",
                Permissions = new List<string>
                {
                    Permissions.ViewEquipment,
                    Permissions.ViewMaintenance,
                    Permissions.RegisterMaintenance,
                    Permissions.ProposeDowngrade,
                    Permissions.ViewDowngrades,
                    Permissions.ViewPerformance
                }
            },
            new Role
            {
                Id = 2,
                Name = "EquipmentReceiver",
                Description = "Receptor de Equipos - Receives equipment, approves downgrades",
                Permissions = new List<string>
                {
                    Permissions.ViewEquipment,
                    Permissions.ViewMaintenance,
                    Permissions.ViewDowngrades,
                    Permissions.ApproveDowngrade,
                    Permissions.RejectDowngrade,
                    Permissions.RegisterReception,
                    Permissions.ViewTransfers
                }
            },
            new Role
            {
                Id = 3,
                Name = "SectionManager",
                Description = "Responsable de Sección - Requests transfers, reviews section inventory",
                Permissions = new List<string>
                {
                    Permissions.ViewSectionEquipment,
                    Permissions.ViewMaintenance,
                    Permissions.ApproveMaintenance,
                    Permissions.RequestTransfer,
                    Permissions.ViewTransfers,
                    Permissions.ViewDowngrades
                }
            },
            new Role
            {
                Id = 4,
                Name = "Administrator",
                Description = "Administrador - Full inventory, maintenance, and downgrade management",
                Permissions = new List<string>
                {
                    Permissions.ViewAllEquipment,
                    Permissions.CreateEquipment,
                    Permissions.EditEquipment,
                    Permissions.DeleteEquipment,
                    Permissions.ViewMaintenance,
                    Permissions.RegisterMaintenance,
                    Permissions.EditMaintenance,
                    Permissions.ViewDowngrades,
                    Permissions.ApproveDowngrade,
                    Permissions.RejectDowngrade,
                    Permissions.EditDowngrade,
                    Permissions.ViewTransfers,
                    Permissions.ApproveTransfer,
                    Permissions.ViewUsers
                }
            },
            new Role
            {
                Id = 5,
                Name = "Director",
                Description = "Director del Centro - Full system control, generates reports",
                Permissions = new List<string>
                {
                    // All permissions
                    Permissions.ViewAllEquipment,
                    Permissions.CreateEquipment,
                    Permissions.EditEquipment,
                    Permissions.DeleteEquipment,
                    Permissions.ViewMaintenance,
                    Permissions.RegisterMaintenance,
                    Permissions.EditMaintenance,
                    Permissions.ViewDowngrades,
                    Permissions.ApproveDowngrade,
                    Permissions.RejectDowngrade,
                    Permissions.EditDowngrade,
                    Permissions.ViewTransfers,
                    Permissions.RequestTransfer,
                    Permissions.ApproveTransfer,
                    Permissions.ViewPerformance,
                    Permissions.EvaluateTechnician,
                    Permissions.GenerateInventoryReport,
                    Permissions.GenerateDowngradeReport,
                    Permissions.GeneratePerformanceReport,
                    Permissions.ViewAllReports,
                    Permissions.ManageUsers,
                    Permissions.ViewUsers,
                    Permissions.ViewAuditLogs
                }
            }
        );
    }

    private void SeedDepartments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "IT Department", Code = "IT", Description = "Information Technology" },
            new Department { Id = 2, Name = "Communications", Code = "COM", Description = "Communications Department" },
            new Department { Id = 3, Name = "Electrical", Code = "ELEC", Description = "Electrical Department" },
            new Department { Id = 4, Name = "Warehouse", Code = "WH", Description = "Equipment Warehouse" }
        );
    }
}
```

💡 **Key Point:** The DbContext is configured with all relationships and includes seed data for roles and departments based on business requirements.

#### **File: `src/EquipmentManagement.Infrastructure/Services/TokenService.cs`**

```csharp
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EquipmentManagement.Application.Interfaces;
using EquipmentManagement.Domain.Entities;
using EquipmentManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EquipmentManagement.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpirationMinutes;

    public TokenService(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
        
        _secretKey = _configuration["Jwt:SecretKey"] 
            ?? throw new InvalidOperationException("JWT SecretKey not configured");
        _issuer = _configuration["Jwt:Issuer"] ?? "EquipmentManagement";
        _audience = _configuration["Jwt:Audience"] ?? "EquipmentManagementClient";
        _accessTokenExpirationMinutes = int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "60");
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim("EmployeeId", user.EmployeeId),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim("RoleId", user.RoleId.ToString()),
        };

        // Add department if assigned (for Section Managers)
        if (user.DepartmentId.HasValue)
        {
            claims.Add(new Claim("DepartmentId", user.DepartmentId.Value.ToString()));
        }

        // Add permissions as claims
        foreach (var permission in user.Role.Permissions)
        {
            claims.Add(new Claim("permission", permission));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
            ValidateLifetime = false,
            ValidIssuer = _issuer,
            ValidAudience = _audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var user = await _context.Users.FindAsync(userId);
        
        if (user == null || user.RefreshToken != refreshToken)
            return false;

        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return false;

        return true;
    }
}
```

#### **File: `src/EquipmentManagement.Infrastructure/Services/AuthService.cs`**

```csharp
using EquipmentManagement.Application.Common;
using EquipmentManagement.Application.DTOs.Auth;
using EquipmentManagement.Application.Interfaces;
using EquipmentManagement.Domain.Entities;
using EquipmentManagement.Domain.Enums;
using EquipmentManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace EquipmentManagement.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthService(ApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
        {
            return Result<LoginResponse>.Failure("Invalid email or password");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Result<LoginResponse>.Failure("Invalid email or password");
        }

        if (!user.IsActive)
        {
            return Result<LoginResponse>.Failure("Account is deactivated");
        }

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        user.LastLoginAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var response = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = MapToUserDto(user)
        };

        return Result<LoginResponse>.Success(response);
    }

    public async Task<Result<LoginResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
        {
            return Result<LoginResponse>.Failure("User with this email already exists");
        }

        if (await _context.Users.AnyAsync(u => u.EmployeeId == request.EmployeeId, cancellationToken))
        {
            return Result<LoginResponse>.Failure("Employee Id already exists");
        }

        // Default role is Technician unless specified
        var roleId = request.RoleId ?? (int)RoleEnum.Technician;
        
        var role = await _context.Roles.FindAsync(new object[] { roleId }, cancellationToken);
        if (role == null)
        {
            return Result<LoginResponse>.Failure("Invalid role specified");
        }

        // Validate department assignment for Section Managers
        Department? department = null;
        if (request.DepartmentId.HasValue)
        {
            department = await _context.Departments.FindAsync(new object[] { request.DepartmentId.Value }, cancellationToken);
            if (department == null)
            {
                return Result<LoginResponse>.Failure("Invalid department specified");
            }
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = passwordHash,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmployeeId = request.EmployeeId,
            RoleId = roleId,
            Role = role,
            DepartmentId = request.DepartmentId,
            Department = department,
            YearsOfExperience = request.YearsOfExperience,
            Specialty = request.Specialty,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new