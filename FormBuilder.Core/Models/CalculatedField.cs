
using System.Linq.Expressions;
namespace FormBuilder.Core.Models
{
    public class CalculatedField : Field
    {
        public List<string> DependentFieldNames { get; set; } 

        public CalculatedField(string name, List<string> dependentFieldNames)
            : base(name, FieldType.CalculatedField)
        {
            DependentFieldNames = dependentFieldNames;
        }

        public override double? GetValue(Dictionary<string, double> userFieldValues, Dictionary<string, double?> calculatedFieldValues)
        {
            double sum = 0;

            foreach (var fieldName in DependentFieldNames)
            {
                if (userFieldValues.ContainsKey(fieldName))
                {
                    sum += userFieldValues[fieldName];
                }
                else if (calculatedFieldValues.ContainsKey(fieldName) && calculatedFieldValues[fieldName].HasValue)
                {
                    sum += calculatedFieldValues[fieldName].Value;
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
}
