
using System.Linq.Expressions;
namespace FormBuilder.Core.Models
{
    public class CalculatedField : Field
    {
        public string Formula { get; private set; }

        public CalculatedField(string name, string formula)
            : base(name, FieldType.CalculatedField, DataType.Numeric)
        {
            Formula = formula;
        }

        public double? GetValue(Dictionary<string, double> userFieldValues)
        {
            // Create an instance of the NCalc expression parser
            NCalc.Expression expression = new NCalc.Expression(Formula);

            // Set the user field values as parameters in the expression
            foreach (var field in userFieldValues)
            {
                expression.Parameters[field.Key] = field.Value;
            }

            // Evaluate the expression and return the result as double
            var result = expression.Evaluate();
            return Convert.ToDouble(result);
        }
    }
}
