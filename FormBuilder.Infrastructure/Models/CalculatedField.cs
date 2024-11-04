
namespace FormBuilder.Infrastructure.Models;

public class CalculatedField : Field
{
    public List<string> DependentFieldNames { get; set; } 

    public CalculatedField(string name, List<string> dependentFieldNames)
        : base(name, FieldType.CalculatedField)
    {
        DependentFieldNames = dependentFieldNames;
    }

    public override double GetValue(Dictionary<string, double> userFieldValues, Dictionary<string, double> calculatedFieldValues, FormTemplate formTemplate)
    {
        if (calculatedFieldValues == null)
        {
            throw new ArgumentNullException(nameof(calculatedFieldValues));
        }

        double sum = 0;

        foreach (var fieldName in DependentFieldNames)
        {
            if (userFieldValues.TryGetValue(fieldName, out var value))
            {
                sum += value;
            }
            else if (calculatedFieldValues.TryGetValue(fieldName, out var calcValue))
            {
                sum += calcValue;
            }
            else
            {
                throw new ArgumentException($"Dependent field '{fieldName}' is missing from both user and calculated values.");
            }
        }

        calculatedFieldValues[Name] = sum;

        return sum;
    }
}