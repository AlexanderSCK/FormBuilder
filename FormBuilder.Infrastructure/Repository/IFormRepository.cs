using FormBuilder.Infrastructure.Models;

namespace FormBuilder.Infrastructure.Repository;

public interface IFormRepository
{
    Task AddFormTemplateAsync(FormTemplate formTemplate);
    Task<FormTemplate?> GetFormTemplateByIdAsync(Guid id);
}