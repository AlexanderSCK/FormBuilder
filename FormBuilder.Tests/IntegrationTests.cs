using FormBuilder.Dtos;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Net;
using FormBuilder.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FormBuilder.Tests
{
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
            _client?.Dispose(); 
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
            var response = await _client.GetAsync("/api/FormTemplate/9999");

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
            var response = await _client.PostAsJsonAsync("/api/FormTemplate/9999/generate", new Dictionary<string, double>());

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
