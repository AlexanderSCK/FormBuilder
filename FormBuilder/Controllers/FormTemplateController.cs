using FormBuilder.Core;
using FormBuilder.Core.Dtos;
using FormBuilder.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FormBuilder.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FormTemplateController : ControllerBase
{
    private readonly IFormService _formService;

    public FormTemplateController(IFormService formService)
    {
        _formService = formService;
    }

    /// <summary>
    /// Creates a new form template.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateFormTemplate([FromBody] CreateFormTemplateDto formTemplateDto)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException(ModelState);
        }
        var templateId = await _formService.CreateFormTemplateAsync(formTemplateDto);
        return CreatedAtAction(nameof(GetFormTemplate), new { id = templateId }, new { id = templateId });
    }

    /// <summary>
    /// Retrieves a form template.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFormTemplate(Guid id)
    {
        var formTemplate = await _formService.GetFormTemplateByIdAsync(id);
        if (formTemplate == null)
        {
            throw new NotFoundException(id);
        }

        return Ok(formTemplate);
    }

    /// <summary>
    /// Creates a new form instance from a from template and additional user field values.
    /// </summary>
    [HttpPost("{id}/generate")]
    public async Task<IActionResult> GenerateFormInstance(Guid id, [FromBody] GenerateFormInstanceRequest request)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException(ModelState);
        }

        var userFieldValues = request.Fields.ToDictionary(f => f.FieldName, f => f.Value, StringComparer.OrdinalIgnoreCase);

        var formInstance = await _formService.GenerateFormInstanceAsync(id, userFieldValues);
        if (formInstance == null)
        {
            throw new NotFoundException(id);
        }

        return Ok(formInstance);
    }
}