# FormBuilder

This is a backend system in C# that enables users to manage
forms by generating instances from templates and user-entered fields.

Built on .NET 8

Leveraging PostgreSQL database deployed in AWS using EF Core migrations

## Endpoints

### 1. Create Price Calculation Template

- **Endpoint**: POST /api/FormTemplate
- **Description**: Allows you to create a new from template template with either field values or calculated values.
- **Headers**:
  - `Content-Type`: application/json
- **Request Body** (sample):

```json
{
  "templateName": "Test Form",
  "fields": [
    {
      "name": "Price",
      "type": 0  
    },
    {
      "name": "Tax",
      "type": 0  
    },
    {
      "name": "Total",
      "type": 1,  
      "dependentFieldNames": ["Price", "Tax"]
    }
  ]
}
```
- **Response**: (sample):
```json
{
  "id": "d6e7d47e-8ec6-4d4b-be11-0ade5e9875c4"
}
```
- **Endpoint**: GET /api/FromTemplate/{id}
- **Description**: Retrieves a specific form template by its ID.
- **Headers**:
  - `Content-Type`: application/json
- **Response**: (sample): (Optional) Use this id for testing: f8a4cebf-94d0-4af9-a6df-dbdd95b02745

```json
{
  "id": "f8a4cebf-94d0-4af9-a6df-dbdd95b02745",
  "templateName": "Tax Form",
  "fields": [
    {
      "id": 26,
      "name": "Total",
      "type": 1,
      "formTemplateId": "f8a4cebf-94d0-4af9-a6df-dbdd95b02745"
    },
    {
      "id": 27,
      "name": "Price",
      "type": 0,
      "formTemplateId": "f8a4cebf-94d0-4af9-a6df-dbdd95b02745"
    },
    {
      "id": 28,
      "name": "Tax",
      "type": 0,
      "formTemplateId": "f8a4cebf-94d0-4af9-a6df-dbdd95b02745"
    }
  ]
}
```

- **Endpoint**: POST /api/FromTemplate/{id}/generate
- **Description**: Generates a form instance based on template and the provided values.
- **Headers**:
  - `Content-Type`: application/json
- **Request Body** (sample):  (Optional) Use this id for testing: f8a4cebf-94d0-4af9-a6df-dbdd95b02745

```json
{
  "fields": [
    {
      "fieldName": "Price",
      "value": 100
    },
    {
      "fieldName": "Tax",
      "value": 2
    }
  ]
}
```

- **Response** (sample):

```json
{
  "templateName": "Tax Form",
  "fieldValues": {
    "Total": 102,
    "Price": 100,
    "Tax": 2
  }
}
```
