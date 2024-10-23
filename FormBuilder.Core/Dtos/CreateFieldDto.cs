using FormBuilder.Core.Models;

namespace FormBuilder.Dtos
{
    public class CreateFieldDto
    {
        public string Name { get; set; }
        public FieldType Type { get; set; }
        public List<string>? DependentFieldNames { get; set; }
    }
}
