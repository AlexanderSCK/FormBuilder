using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.Models
{
    public class FormTemplate
    {
        [Key]
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public List<Field> Fields { get; set; }

        public FormTemplate() { }

        public FormTemplate(string templateName, List<Field> fields)
        {
            TemplateName = templateName;
            Fields = fields;
        }

        public double? GetFieldValue(Dictionary<string, double> userFieldValues, string fieldName)
        {
            var field = Fields.FirstOrDefault(f => f.Name == fieldName);

            if (field == null)
            {
                throw new ArgumentException($"Field '{fieldName}' does not exist in the form template.");
            }

            if (field.DataType != DataType.Numeric)
            {
                throw new InvalidOperationException($"Field '{fieldName}' is not of numeric type.");
            }

            return field.Type switch
            {
                FieldType.UserField => ((UserField)field).GetValue(userFieldValues),
                FieldType.CalculatedField => ((CalculatedField)field).GetValue(userFieldValues),
                _ => throw new InvalidOperationException($"Unsupported field type '{field.Type}'.")
            };
        }
    }
}
