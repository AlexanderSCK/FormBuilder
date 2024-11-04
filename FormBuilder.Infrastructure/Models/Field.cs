using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Infrastructure.Models;

public abstract class Field
{
    [Key]
    public int Id { get; set; }


    public string Name { get; set; }
    public FieldType Type { get; set; }

    public Guid FormTemplateId { get; set; }
    protected Field(string name, FieldType type)
    {
        Name = name;
        Type = type;
    }

    public abstract double GetValue(Dictionary<string, double> userFieldValues, Dictionary<string, double> calculatedFieldValues, FormTemplate formTemplate);
}