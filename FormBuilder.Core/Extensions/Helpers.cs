using FormBuilder.Core.Models;
using FormBuilder.Dtos;

namespace FormBuilder.Core.Extensions
{
    public static class Helpers
    {
        public static FormTemplate MapDtoToDomain(this CreateFormTemplateDto formTemplateDto)
        {
            var fields = formTemplateDto.Fields.Select(f =>
            {
                if (f.Type == FieldType.UserField)
                {
                    return new UserField(f.Name);
                }
                else if (f.Type == FieldType.CalculatedField)
                {
                    if (string.IsNullOrWhiteSpace(f.Formula))
                        throw new ArgumentException($"Calculated field '{f.Name}' must have a formula.");

                    return new CalculatedField(f.Name, f.Formula) as Field;
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported field type '{f.Type}' for field '{f.Name}'.");
                }
            }).ToList();

            return new FormTemplate(formTemplateDto.TemplateName, fields);
        }
    }
}
