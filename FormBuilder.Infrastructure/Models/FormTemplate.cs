using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Infrastructure.Models;

public class FormTemplate
{
    [Key]
    public Guid Id { get; set; }

    public string TemplateName { get; set; } = string.Empty;
    public List<Field> Fields { get; set; }

    public FormTemplate() { }

    public FormTemplate(string templateName, List<Field> fields)
    {
        Id = Guid.NewGuid();
        TemplateName = templateName;
        Fields = fields;
    }

    public double? GetFieldValue(Dictionary<string, double> userFieldValues, string fieldName, Dictionary<string, double> calculatedFieldValues)
    {
        var field = Fields.FirstOrDefault(f => f.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
        if (field == null)
        {
            throw new KeyNotFoundException($"Field '{fieldName}' does not exist in the form template.");
        }

        if (calculatedFieldValues.TryGetValue(fieldName, out var fieldValue))
        {
            return fieldValue;
        }

        // Calculate the field value
        var value = field.GetValue(userFieldValues, calculatedFieldValues, this);

        // Store the calculated value
            calculatedFieldValues[fieldName] = value;

        return value;
    }
}