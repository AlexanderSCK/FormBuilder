using System.Net.Http.Json;
using System.Net;
using FormBuilder.Core.Dtos;
using FormBuilder.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;

namespace FormBuilder.Tests;

[TestFixture]
public class IntegrationTests : WebApplicationFactory<Program>
{
    private HttpClient _client;

    [SetUp]
    public void SetUp()
    {
        _client = CreateClient();
    }

    [TearDown]
    public void TearDown() 
    { 
        _client.Dispose(); 
    }  

    [Test]
    public async Task CreateFormTemplate_ShouldReturnCreated_WhenValidInput()
    {
        // Arrange
        var formTemplateDto = new CreateFormTemplateDto
        {
            TemplateName = "Employee Feedback Form",
            Fields = new List<CreateFieldDto>
            {
                new CreateFieldDto { Name = "WorkHours", Type = FieldType.UserField },
                new CreateFieldDto { Name = "PerformanceScore", Type = FieldType.UserField },
                new CreateFieldDto { Name = "TotalScore", Type = FieldType.CalculatedField, DependentFieldNames = new List<string> { "WorkHours", "PerformanceScore" } }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/FormTemplate", formTemplateDto);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        var createdResponse = await response.Content.ReadFromJsonAsync<object>();
        Assert.That(createdResponse, Is.Not.Null);
    }

    [Test]
    public async Task CreateFormTemplate_ShouldReturnBadRequest_WhenInvalidInput()
    {
        // Arrange
        var formTemplateDto = new CreateFormTemplateDto
        {
            TemplateName = "",
            Fields = new List<CreateFieldDto>()
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/FormTemplate", formTemplateDto);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var errorResponse = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.That(errorResponse, Is.Not.Null);
        Assert.That(errorResponse.Errors.ContainsKey("TemplateName"), Is.True);
        Assert.That(errorResponse.Errors.ContainsKey("Fields"), Is.True);
    }

    [Test]
    public async Task GetFormTemplate_ShouldReturnFormTemplate_WhenValidId()
    {
        // Arrange
        var formTemplateDto = new CreateFormTemplateDto
        {
            TemplateName = "Employee Feedback Form",
            Fields = new List<CreateFieldDto>
            {
                new CreateFieldDto { Name = "WorkHours", Type = FieldType.UserField },
                new CreateFieldDto { Name = "PerformanceScore", Type = FieldType.UserField },
                new CreateFieldDto { Name = "TotalScore", Type = FieldType.CalculatedField, DependentFieldNames = new List<string> { "WorkHours", "PerformanceScore" } }
            }
        };

        var createResponse = await _client.PostAsJsonAsync("/api/FormTemplate", formTemplateDto);
        var createdForm = await createResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        var formTemplateId = createdForm["id"].ToString();

        // Act
        var response = await _client.GetAsync($"/api/FormTemplate/{formTemplateId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var formTemplate = await response.Content.ReadFromJsonAsync<object>();
        Assert.That(formTemplate, Is.Not.Null);
    }

    [Test]
    public async Task GetFormTemplate_ShouldReturnNotFound_WhenInvalidId()
    {
        // Act
        var id = Guid.NewGuid();
        var response = await _client.GetAsync($"/api/FormTemplate/{id}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GenerateFormInstance_ShouldReturnFormInstance_WhenValidInput()
    {
        // Arrange
        var formTemplateDto = new CreateFormTemplateDto
        {
            TemplateName = "Employee Feedback Form",
            Fields = new List<CreateFieldDto>
            {
                new CreateFieldDto { Name = "WorkHours", Type = FieldType.UserField },
                new CreateFieldDto { Name = "PerformanceScore", Type = FieldType.UserField },
                new CreateFieldDto { Name = "TotalScore", Type = FieldType.CalculatedField, DependentFieldNames = new List<string> { "WorkHours", "PerformanceScore" } }
            }
        };

        var createResponse = await _client.PostAsJsonAsync("/api/FormTemplate", formTemplateDto);
        var createdForm = await createResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        var formTemplateId = createdForm["id"].ToString();

        var userFieldValues = new Dictionary<string, double>
        {
            { "WorkHours", 40 },
            { "PerformanceScore", 85 }
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/FormTemplate/{formTemplateId}/generate", userFieldValues);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var formInstance = await response.Content.ReadFromJsonAsync<object>();
        Assert.That(formInstance, Is.Not.Null);
    }

    [Test]
    public async Task GenerateFormInstance_ShouldReturnNotFound_WhenInvalidId()
    {
        // Act
        var id = Guid.NewGuid();
        var response = await _client.PostAsJsonAsync($"/api/FormTemplate/{id}/generate", new Dictionary<string, double>());

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task CreateFormTemplate_ShouldReturnBadRequest_WhenCalculatedFieldHasDuplicateDependencies()
    {
        // Arrange
        var formTemplateDto = new CreateFormTemplateDto
        {
            TemplateName = "Duplicate Dependencies Form",
            Fields = new List<CreateFieldDto>
            {
                new CreateFieldDto { Name = "Field1", Type = FieldType.UserField },
                new CreateFieldDto { Name = "Field2", Type = FieldType.UserField },
                new CreateFieldDto { Name = "CalculatedField1", Type = FieldType.CalculatedField, DependentFieldNames = new List<string> { "Field1", "Field1" } }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/FormTemplate", formTemplateDto);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var errorResponse = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.That(errorResponse, Is.Not.Null);

        var hasDuplicateDependenciesError = errorResponse.Errors
            .Any(e => e.Key.EndsWith("DependentFieldNames") && e.Value.Contains("Dependent field names must be unique."));

        Assert.That(hasDuplicateDependenciesError, Is.True, "Expected a validation error for duplicate dependent field names.");
    }

    [Test]
    public async Task CreateFormTemplate_ShouldReturnBadRequest_WhenUserFieldHasDependencies()
    {
        // Arrange
        var formTemplateDto = new CreateFormTemplateDto
        {
            TemplateName = "User Field with Dependencies Form",
            Fields = new List<CreateFieldDto>
            {
                new CreateFieldDto { Name = "Field1", Type = FieldType.UserField, DependentFieldNames = new List<string> { "Field2" } },
                new CreateFieldDto { Name = "Field2", Type = FieldType.UserField }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/FormTemplate", formTemplateDto);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var errorResponse = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.That(errorResponse, Is.Not.Null);

        var hasUserFieldDependenciesError = errorResponse.Errors
            .Any(e => e.Key.EndsWith("DependentFieldNames") && e.Value.Contains("User fields cannot have dependent fields."));

        Assert.That(hasUserFieldDependenciesError, Is.True, "Expected a validation error for user fields having dependencies.");
    }
}