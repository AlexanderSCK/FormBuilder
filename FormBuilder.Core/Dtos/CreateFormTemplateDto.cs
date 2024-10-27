using System.ComponentModel.DataAnnotations;
using FormBuilder.Infrastructure.Models;

namespace FormBuilder.Core.Dtos;

public class CreateFormTemplateDto : IValidatableObject
{
    [Required(ErrorMessage = "TemplateName is required.")]
    [StringLength(100, MinimumLength = 2)]
    public string TemplateName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Fields are required.")]
    [MinLength(1, ErrorMessage = "At least one field is required.")]
    public List<CreateFieldDto> Fields { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var fieldNames = Fields.Select(f => f.Name.ToLower()).ToList();
        var duplicateNames = fieldNames.GroupBy(name => name)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (duplicateNames.Any())
        {
            yield return new ValidationResult(
                $"Duplicate field names detected: {string.Join(", ", duplicateNames)}.",
                new[] { nameof(Fields) });
        }

        foreach (var field in Fields.Where(field => field.Type == FieldType.CalculatedField))
        {
            if (field.DependentFieldNames == null || !field.DependentFieldNames.Any())
            {
                yield return new ValidationResult(
                    $"Calculated field '{field.Name}' must have at least one dependent field.",
                    new[] { nameof(Fields) });
            }

            foreach (var dependency in field.DependentFieldNames)
            {
                if (!Fields.Any(f => f.Name.Equals(dependency, StringComparison.OrdinalIgnoreCase)))
                {
                    yield return new ValidationResult(
                        $"Calculated field '{field.Name}' has an invalid dependency '{dependency}'.",
                        new[] { nameof(Fields) });
                }
            }
        }
    }
}