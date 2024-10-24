using FormBuilder.Core;
using FormBuilder.Dtos;
using FormBuilder.Exceptions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace FormBuilder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormTemplateController : ControllerBase
    {
        private readonly IFormService _formService;

        public FormTemplateController(IFormService formService)
        {
            _formService = formService;
        }

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFormTemplate(int id)
        {
            var formTemplate = await _formService.GetFormTemplateByIdAsync(id);
            if (formTemplate == null)
            {
                throw new NotFoundException(id);
            }

            return Ok(formTemplate);
        }

        [HttpPost("{id}/generate")]
        public async Task<IActionResult> GenerateFormInstance(int id, [FromBody] Dictionary<string, double> userFieldValues)
        {
            var formInstance = await _formService.GenerateFormInstanceAsync(id, userFieldValues);
            if (formInstance == null)
            {
                throw new NotFoundException(id);
            }

            return Ok(formInstance);
        }
    }
}
