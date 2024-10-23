using FormBuilder.Core.Models;
using FormBuilder.Dtos;
using System.Linq;

namespace FormBuilder.Core.Extensions
{
    public static class Helpers
    {
        public static FormTemplate MapDtoToDomain(this CreateFormTemplateDto formTemplateDto)
        {
            if (formTemplateDto == null)
                throw new ArgumentNullException(nameof(formTemplateDto));

            if (formTemplateDto.Fields == null || !formTemplateDto.Fields.Any())
                throw new ArgumentException("Form template must contain at least one field.");

            var fields = formTemplateDto.Fields.Select<CreateFieldDto, Field>(f =>
            {
                switch (f.Type)
                {
                    case FieldType.UserField:
                        return new UserField(f.Name);

                    case FieldType.CalculatedField:
                        if (f.DependentFieldNames == null || !f.DependentFieldNames.Any())
                            throw new ArgumentException($"Calculated field '{f.Name}' must have at least one dependent field.");

                        return new CalculatedField(f.Name, f.DependentFieldNames);

                    default:
                        throw new InvalidOperationException($"Unsupported field type '{f.Type}' for field '{f.Name}'.");
                }
            }).ToList();

            return new FormTemplate(formTemplateDto.TemplateName, fields);
        }
    }
}
