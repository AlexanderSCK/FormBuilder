
namespace FormBuilder.Core.Models
{
    public class UserField : Field
    {
        public UserField(string name)
            : base(name, FieldType.UserField, DataType.Numeric) { }

        public double? GetValue(Dictionary<string, double> userFieldValues)
        {
            if (!userFieldValues.TryGetValue(Name, out var value))
            {
                throw new KeyNotFoundException($"User field '{Name}' not provided.");
            }
            return value;
        }
    }
}
