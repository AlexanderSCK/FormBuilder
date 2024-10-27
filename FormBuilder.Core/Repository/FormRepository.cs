using FormBuilder.Core.Database;
using FormBuilder.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FormBuilder.Core.Repository;

public class FormRepository : IFormRepository
{
    private readonly FormContext _context;

    public FormRepository(FormContext context)
    {
        _context = context;
    }

    public async Task AddFormTemplateAsync(FormTemplate formTemplate)
    {
        _context.FormTemplates.Add(formTemplate);
        await _context.SaveChangesAsync();
    }

    public async Task<FormTemplate?> GetFormTemplateByIdAsync(Guid id)
    {
        return await _context.FormTemplates
            .Include(t => t.Fields)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}