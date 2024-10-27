namespace FormBuilder.Dtos;

public class FormInstanceDto
{
    public string TemplateName { get; set; } = string.Empty;
    public Dictionary<string, double?> FieldValues { get; set; } = [];
}