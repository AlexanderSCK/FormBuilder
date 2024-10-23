
namespace FormBuilder.Core.Models
{
    public class UserField : Field
    {
        public UserField(string name)
            : base(name, FieldType.UserField, DataType.Numeric) { }

        public override double? GetValue(Dictionary<string, double> userFieldValues)
        {
            if (!userFieldValues.ContainsKey(Name))
            {
                throw new KeyNotFoundException($"User field '{Name}' is missing.");
            }

            return userFieldValues[Name];
        }
    }
}
