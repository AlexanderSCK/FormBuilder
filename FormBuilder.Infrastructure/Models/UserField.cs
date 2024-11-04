
namespace FormBuilder.Infrastructure.Models;

public class UserField : Field
{
    public UserField(string name)
        : base(name, FieldType.UserField) { }

    public override double GetValue(Dictionary<string, double> userFieldValues, Dictionary<string, double> calculatedFieldValues, FormTemplate formTemplate)
    {
        if (!userFieldValues.TryGetValue(Name, out var value))
        {
            throw new KeyNotFoundException($"User field '{Name}' is missing.");
        }

        return value;
    }
}