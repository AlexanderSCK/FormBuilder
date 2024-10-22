namespace FormBuilder.Dtos
{
    public class FormInstanceDto
    {
        public string TemplateName { get; set; }
        public Dictionary<string, double?> FieldValues { get; set; }
    }
}
