# LifeCare HMS - Patient Registration Service

A modern, production-ready patient registration and management system built for Kenyan healthcare facilities, featuring SHIF (Social Health Insurance Fund) integration and comprehensive patient journey tracking.

## 🏥 Overview

The Patient Registration Service is the core identity management module of LifeCare Hospital Management System. It handles patient demographics, guardian management for minors, and complete patient lifecycle tracking with status history.

## ✨ Key Features

### Patient Management
- **SHIF-First Registration** - Primary identification via SHIF number (Kenya's universal health insurance)
- **Smart ID Validation** - National ID required only for patients 18+ years
- **Guardian Support** - Automatic guardian requirement for patients under 13 years
- **Unique Identifiers** - Auto-generated Medical Record Numbers (MRN) with year-based sequencing

### Search & Retrieval
- Search by SHIF Number (primary)
- Search by Medical Record Number (MRN)
- Search by National ID
- Search by Phone Number
- Search by Name or County
- Recent patients view

### Patient Journey Tracking
- Real-time status updates (Awaiting Triage → In Triage → In Consultation → In Lab → Discharged)
- Complete status history timeline
- Audit trail with timestamps and user tracking
- Soft delete (deactivation) with history preservation

### Analytics
- Total patient count
- New registrations (today/week/month)
- Status distribution
- Active vs inactive patients
- Guardian statistics

## 🚀 API Endpoints

### Registration
```http
POST /api/Patients/register
```

**Request:**
```json
{
  "shifNumber": "SHF001234567",
  "nationalId": "37961960",
  "firstName": "John",
  "lastName": "Kamau",
  "dateOfBirth": "1995-03-15",
  "gender": "Male",
  "phoneNumber": "0712345678",
  "email": "john.kamau@email.com",
  "county": "Nairobi",
  "subCounty": "Westlands",
  "country": "Kenya",
  "zipCode": "00100",
  "guardian": null
}
```

**Guardian Example (for minors):**
```json
{
  "shifNumber": "SHF888888888",
  "nationalId": null,
  "firstName": "Little",
  "lastName": "Kid",
  "dateOfBirth": "2018-01-01",
  "gender": "Male",
  "phoneNumber": "0788888888",
  "county": "Nairobi",
  "subCounty": "Westlands",
  "country": "Kenya",
  "zipCode": "00100",
  "guardian": {
    "firstName": "Jane",
    "lastName": "Doe",
    "relationship": "Mother",
    "phoneNumber": "0799999999"
  }
}
```

### Search & Retrieval

**Get by SHIF Number:**
```http
GET /api/Patients/shif/{shifNumber}
```

**Get by MRN:**
```http
GET /api/Patients/mrn/LC-2026-0001
```

**Get by ID:**
```http
GET /api/Patients/{id}
```

**Get by Phone:**
```http
GET /api/Patients/phone/0712345678
```

**Search by Name/County:**
```http
GET /api/Patients/search?name=John&county=Nairobi
```

**Recent Patients:**
```http
GET /api/Patients/recent?count=10
```

**All Patients:**
```http
GET /api/Patients
```

### Updates

**Update Patient Info:**
```http
PUT /api/Patients/{id}
```

**Update Patient Status:**
```http
PATCH /api/Patients/{id}/status
```

**Request:**
```json
{
  "newStatus": "InConsultation",
  "notes": "Patient transferred to Dr. Mwangi",
  "changedBy": "Nurse Jane"
}
```

### Status History

**Get Patient Journey:**
```http
GET /api/Patients/{id}/status-history
```

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "...",
      "patientId": "...",
      "status": "InConsultation",
      "changedAt": "2026-02-16T10:30:00Z",
      "changedBy": "Nurse Jane",
      "notes": "Patient transferred to Dr. Mwangi"
    },
    {
      "status": "InTriage",
      "changedAt": "2026-02-16T09:15:00Z",
      "changedBy": "System",
      "notes": null
    }
  ]
}
```

### Analytics

**Dashboard Statistics:**
```http
GET /api/Patients/statistics
```

**Response:**
```json
{
  "success": true,
  "data": {
    "totalPatients": 37,
    "newToday": 5,
    "newThisWeek": 12,
    "newThisMonth": 37,
    "byStatus": {
      "AwaitingTriage": 15,
      "InTriage": 8,
      "InConsultation": 10,
      "Discharged": 3,
      "Inactive": 1
    },
    "activePatients": 36,
    "inactivePatients": 1,
    "withGuardians": 8
  }
}
```

### Deactivation

**Soft Delete:**
```http
DELETE /api/Patients/{id}
```

## 📊 Data Model

### Patient Entity
```
- Id: Guid (Primary Key)
- MRN: string (Auto-generated, Unique)
- ShifNumber: string (Required, Unique)
- NationalId: string? (Optional for <18, Unique)
- FirstName: string
- LastName: string
- DateOfBirth: DateTime
- Gender: string
- PhoneNumber: string
- Email: string?
- County: string?
- SubCounty: string?
- Country: string (Default: "Kenya")
- ZipCode: string?
- Status: PatientStatus
- GuardianName: string?
- GuardianRelationship: string?
- GuardianPhone: string?
- CreatedAt: DateTime
- CreatedBy: string
```

### Patient Status Enum
```csharp
- Unknown = 0
- AwaitingTriage = 1
- InTriage = 2
- InConsultation = 3
- InLab = 4
- AwaitingDischarge = 5
- Discharged = 6
- Inactive = 7
```

## 🔒 Business Rules

1. **SHIF Number**: Always required, must be unique
2. **National ID**:
    - Optional for patients < 18 years
    - Required for patients ≥ 18 years
    - Must be unique when provided
3. **Guardian**:
    - Required for patients < 13 years
    - Forbidden for patients ≥ 18 years
4. **MRN Generation**: Format `LC-YYYY-NNNN` (e.g., LC-2026-0001)
5. **Soft Delete**: Patients are never physically deleted, only set to Inactive status

## 🛠️ Technology Stack

- **.NET 9.0** - Framework
- **PostgreSQL** - Database
- **Entity Framework Core 9.0** - ORM
- **MediatR** - CQRS pattern
- **Docker** - Containerization
- **Swagger/OpenAPI** - API documentation

## 🏗️ Architecture

Clean Architecture with Domain-Driven Design:

```
LifeCare.API/           # Controllers, DTOs, HTTP layer
LifeCare.Application/   # Commands, Queries, Handlers
LifeCare.Domain/        # Entities, Value Objects, Business Logic
LifeCare.Infrastructure/# Repositories, Database, Persistence
```

## 🚦 Getting Started

### Prerequisites
- Docker & Docker Compose
- .NET 9.0 SDK (for local development)

### Running with Docker

```bash
# Start all services
docker-compose up

# API available at
http://localhost:8080

# Swagger UI at
http://localhost:8080/swagger
```

### Database Migrations

```bash
# Create migration
docker exec -it life-care-api dotnet ef migrations add MigrationName \
  --project /src/LifeCare.Infrastructure \
  --startup-project /src/LifeCare.API

# Apply migrations
docker exec -it life-care-api dotnet ef database update \
  --project /src/LifeCare.Infrastructure \
  --startup-project /src/LifeCare.API
```

## 📝 Example Usage

### Register Adult Patient
```bash
curl -X POST http://localhost:8080/api/Patients/register \
  -H "Content-Type: application/json" \
  -d '{
    "shifNumber": "SHF001234567",
    "nationalId": "37961960",
    "firstName": "John",
    "lastName": "Kamau",
    "dateOfBirth": "1995-03-15",
    "gender": "Male",
    "phoneNumber": "0712345678",
    "email": "john.kamau@email.com",
    "county": "Nairobi",
    "subCounty": "Westlands",
    "country": "Kenya",
    "zipCode": "00100"
  }'
```

### Register Child with Guardian
```bash
curl -X POST http://localhost:8080/api/Patients/register \
  -H "Content-Type: application/json" \
  -d '{
    "shifNumber": "SHF888888888",
    "firstName": "Little",
    "lastName": "Kid",
    "dateOfBirth": "2018-01-01",
    "gender": "Male",
    "phoneNumber": "0788888888",
    "county": "Nairobi",
    "subCounty": "Westlands",
    "country": "Kenya",
    "zipCode": "00100",
    "guardian": {
      "firstName": "Jane",
      "lastName": "Doe",
      "relationship": "Mother",
      "phoneNumber": "0799999999"
    }
  }'
```

### Update Patient Status
```bash
curl -X PATCH http://localhost:8080/api/Patients/{id}/status \
  -H "Content-Type: application/json" \
  -d '{
    "newStatus": "InConsultation",
    "notes": "Transferred to Dr. Mwangi",
    "changedBy": "Nurse Jane"
  }'
```

## 🧪 Testing

Currently: 37+ test patients with realistic Kenyan data

Generate more test patients:
```bash
./create_patients.sh
```

## 📈 Future Enhancements

- [ ] Medical Records integration (allergies, blood group, chronic conditions)
- [ ] Appointment scheduling
- [ ] SHIF claims integration
- [ ] Biometric capture (fingerprint/photo)
- [ ] Next of kin management
- [ ] Patient portal (self-service)
- [ ] SMS notifications
- [ ] Export to Excel/PDF
- [ ] Bulk import from CSV



Built as part of LifeCare HMS development journey

---

**Note**: This is a working MVP. The system is production-ready for patient registration and management in Kenyan healthcare facilities.