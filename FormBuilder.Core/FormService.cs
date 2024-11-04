using FormBuilder.Core.Dtos;
using FormBuilder.Core.Extensions;
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
        var formTemplate = formTemplateDto.MapDtoToDomain();
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

        var calculatedFieldValues = new Dictionary<string, double>();
        var fieldValues = new Dictionary<string, double>();

        foreach (var field in formTemplate.Fields)
        {
            var value = formTemplate.GetFieldValue(userFieldValues, field.Name, calculatedFieldValues);
            if (value.HasValue)
            {
                fieldValues[field.Name] = value.Value;
            }
        }

        var formInstance = new FormInstanceDto
        {
            TemplateName = formTemplate.TemplateName,
            FieldValues = fieldValues
        };

        return formInstance;
    }
    public static double? GetFieldValue(FormTemplate formTemplate, Dictionary<string, double> userFieldValues, string fieldName)
    {
        var calculatedFieldValues = new Dictionary<string, double>();
        return formTemplate.GetFieldValue(userFieldValues, fieldName, calculatedFieldValues);
    }
}