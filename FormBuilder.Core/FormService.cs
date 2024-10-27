using FormBuilder.Core.Extensions;
using FormBuilder.Dtos;
using FormBuilder.Infrastructure.Models;
using FormBuilder.Infrastructure.Repository;

namespace FormBuilder.Core;

public class FormService : IFormService
{
    private readonly IFormRepository _formRepository;

    public FormService(IFormRepository formRepository)
    {
        _formRepository = formRepository;
    }

    public async Task<Guid> CreateFormTemplateAsync(CreateFormTemplateDto formTemplateDto)
    {
        var formTemplate = Helpers.MapDtoToDomain(formTemplateDto);
        await _formRepository.AddFormTemplateAsync(formTemplate);
        return formTemplate.Id;
    }

    public async Task<FormTemplate?> GetFormTemplateByIdAsync(Guid id)
    {
        return await _formRepository.GetFormTemplateByIdAsync(id);
    }

    public async Task<FormInstanceDto> GenerateFormInstanceAsync(Guid templateId, Dictionary<string, double> userFieldValues)
    {
        var formTemplate = await _formRepository.GetFormTemplateByIdAsync(templateId);
        if (formTemplate == null)
        {
            return null;
        }

        var formInstance = new FormInstanceDto
        {
            TemplateName = formTemplate.TemplateName,
            FieldValues = formTemplate.Fields.ToDictionary(
                field => field.Name,
                field => formTemplate.GetFieldValue(userFieldValues, field.Name)
            )
        };

        return formInstance;
    }
    public static double? GetFieldValue(FormTemplate formTemplate, Dictionary<string, double> userFieldValues, string fieldName)
    {
        return formTemplate.GetFieldValue(userFieldValues, fieldName);
    }
}