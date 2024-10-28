using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.Dtos;

public class GenerateFormInstanceRequest
{
    [Required(ErrorMessage = "Fields are required.")]
    [MinLength(1, ErrorMessage = "At least one field is required.")]
    public List<FieldValueDto> Fields { get; set; } = new List<FieldValueDto>();
}