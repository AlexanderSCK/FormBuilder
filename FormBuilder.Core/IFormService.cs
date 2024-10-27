using FormBuilder.Dtos;
using FormBuilder.Infrastructure.Models;

namespace FormBuilder.Core;

public interface IFormService
{
    Task<Guid> CreateFormTemplateAsync(CreateFormTemplateDto formTemplateDto);
    Task<FormTemplate?> GetFormTemplateByIdAsync(Guid id);
    Task<FormInstanceDto> GenerateFormInstanceAsync(Guid templateId, Dictionary<string, double> userFieldValues);
}