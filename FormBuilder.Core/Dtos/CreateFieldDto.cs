using FormBuilder.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Dtos;

public class CreateFieldDto
{
    [Required(ErrorMessage = "Field name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Field type is required.")]
    public FieldType Type { get; set; }

    public List<string>? DependentFieldNames { get; set; }
}