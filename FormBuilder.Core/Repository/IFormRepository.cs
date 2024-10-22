using FormBuilder.Core.Models;

namespace FormBuilder.Core.Repository
{
    public interface IFormRepository
    {
        Task AddFormTemplateAsync(FormTemplate formTemplate);
        Task<FormTemplate?> GetFormTemplateByIdAsync(int id);
    }
}