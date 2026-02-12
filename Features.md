# LifeCare Project Features

## Search Patients
**Module:** Patients  
**Date Implemented:** 2026-02-10  
**Implemented By:** Berry Mundia

### Feature Description
Allows users to search patients by first/last name or city. Supports partial matches and returns a list of patients.

### Technical Details
- **CQRS Implementation:** `SearchPatientsQuery` + `SearchPatientsQueryHandler`
- **Repository Method:** `IPatientRepository.SearchPatientsAsync(name, city)`
- **Database Access:** EF Core (`HospitalDbContext`)
- **Return Type:** `IReadOnlyList<Patient>`

### API Endpoint
| Method | Endpoint | Query Params | Response |
|--------|----------|--------------|----------|
| GET    | `/api/patients/search` | `name` (optional), `city` (optional) | `{ success: true, data: [PatientDto] }` |

### Sample Usage
```http
GET /api/patients/search?name=John&city=Nairobi
