using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Dtos
{
    public class CreateFormTemplateDto
    {
        [Required(ErrorMessage = "TemplateName is required.")]
        public string TemplateName { get; set; }

        [MinLength(1, ErrorMessage = "At least one field is required.")]
        public List<CreateFieldDto> Fields { get; set; }
    }
}
