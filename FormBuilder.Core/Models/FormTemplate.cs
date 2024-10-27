using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.Models;

public class FormTemplate
{
    [Key]
    public Guid Id { get; set; }
    public string TemplateName { get; set; }
    public List<Field> Fields { get; set; }

    public FormTemplate() { }

    public FormTemplate(string templateName, List<Field> fields)
    {
        Id = Guid.NewGuid();
        TemplateName = templateName;
        Fields = fields;
    }

    public double? GetFieldValue(Dictionary<string, double> userFieldValues, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            throw new ArgumentException("Field name cannot be null or whitespace.", nameof(fieldName));

        var field = Fields.FirstOrDefault(f => f.Name == fieldName);
        if (field == null)
        {
            throw new KeyNotFoundException($"Field '{fieldName}' does not exist in the form template.");
        }

        if (field is CalculatedField)
        {
            var calculatedFieldValues = new Dictionary<string, double?>();
                
            return field.GetValue(userFieldValues, calculatedFieldValues);
        }
        else
        {
            return field.GetValue(userFieldValues);
        }
    }
}