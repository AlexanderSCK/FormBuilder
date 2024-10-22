namespace FormBuilder.Core.Models
{
    public abstract class Field
    {
        public string Name { get; set; }
        public FieldType Type { get; set; }
        public DataType DataType { get; set; }

        protected Field(string name, FieldType type, DataType dataType)
        {
            Name = name;
            Type = type;
            DataType = dataType; 
        }
    }
}
