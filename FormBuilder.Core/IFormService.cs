using FormBuilder.Core.Models;
using FormBuilder.Dtos;

namespace FormBuilder.Core
{
    public interface IFormService
    {
        Task<int> CreateFormTemplateAsync(CreateFormTemplateDto formTemplateDto);
        Task<FormTemplate?> GetFormTemplateByIdAsync(int id);
        Task<FormInstanceDto> GenerateFormInstanceAsync(int templateId, Dictionary<string, double> userFieldValues);
    }
}