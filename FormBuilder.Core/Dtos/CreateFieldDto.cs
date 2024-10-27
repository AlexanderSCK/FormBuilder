using System.ComponentModel.DataAnnotations;
using FormBuilder.Infrastructure.Models;

namespace FormBuilder.Core.Dtos;

public class CreateFieldDto : IValidatableObject
{
    [Required(ErrorMessage = "Field name is required.")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Field type is required.")]
    public FieldType Type { get; set; }

    public List<string>? DependentFieldNames { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Type == FieldType.CalculatedField)
        {
            if (DependentFieldNames == null || DependentFieldNames.Count < 2)
            {
                yield return new ValidationResult(
                    "Calculated fields must depend on at least two other fields.",
                    new[] { nameof(DependentFieldNames) }
                );
            }
            else
            {
                if (DependentFieldNames.Distinct(StringComparer.OrdinalIgnoreCase).Count() != DependentFieldNames.Count)
                {
                    yield return new ValidationResult(
                        "Dependent field names must be unique.",
                        new[] { nameof(DependentFieldNames) }
                    );
                }
            }
        }
        else if (Type == FieldType.UserField && DependentFieldNames != null && DependentFieldNames.Any())
        {
            yield return new ValidationResult(
                "User fields cannot have dependent fields.",
                new[] { nameof(DependentFieldNames) }
            );
        }
    }
}