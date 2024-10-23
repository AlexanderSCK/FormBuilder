using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.Models
{
    public abstract class Field
    {
        [Key]
        public int Id { get; set; }


        public string Name { get; set; }
        public FieldType Type { get; set; }
        public DataType DataType { get; set; }

        protected Field(string name, FieldType type, DataType dataType)
        {
            Name = name;
            Type = type;
            DataType = dataType; 
        }

        public virtual double? GetValue(Dictionary<string, double> userFieldValues)
        {
            return GetValue(userFieldValues, new Dictionary<string, double?>());
        }

        public virtual double? GetValue(Dictionary<string, double> userFieldValues, Dictionary<string, double?> calculatedFieldValues)
        {
            return GetValue(userFieldValues); 
        }
    }
}
