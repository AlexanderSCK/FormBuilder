using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.Dtos;

public class FieldValueDto
{
    [Required(ErrorMessage = "Field name is required.")]
    public string FieldName { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative.")]
    public double Value { get; set; }
}