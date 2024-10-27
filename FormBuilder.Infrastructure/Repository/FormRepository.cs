using FormBuilder.Infrastructure.Database;
using FormBuilder.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormBuilder.Infrastructure.Repository;

public class FormRepository : IFormRepository
{
    private readonly FormContext _context;
    private readonly ILogger<FormRepository> _logger;
    public FormRepository(FormContext context, ILogger<FormRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddFormTemplateAsync(FormTemplate formTemplate)
    {
        _logger.LogInformation("Attempting to add a new FormTemplate with ID: {FormTemplateId} and Name: {TemplateName}", formTemplate.Id, formTemplate.TemplateName);

        try
        {
            await _context.FormTemplates.AddAsync(formTemplate);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully added FormTemplate with ID: {FormTemplateId}", formTemplate.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while adding FormTemplate with ID: {FormTemplateId}", formTemplate.Id);
            throw; 
        }
    }

    public async Task<FormTemplate?> GetFormTemplateByIdAsync(Guid id)
    {
        _logger.LogInformation("Retrieving FormTemplate with ID: {FormTemplateId}", id);

        try
        {
            var formTemplate = await _context.FormTemplates
                .Include(t => t.Fields)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (formTemplate != null)
            {
                _logger.LogInformation("Successfully retrieved FormTemplate with ID: {FormTemplateId}", id);
            }
            else
            {
                _logger.LogWarning("FormTemplate with ID: {FormTemplateId} was not found.", id);
            }

            return formTemplate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving FormTemplate with ID: {FormTemplateId}", id);
            throw; 
        }
    }
}