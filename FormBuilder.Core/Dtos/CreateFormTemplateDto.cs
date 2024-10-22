namespace FormBuilder.Dtos
{
    public class CreateFormTemplateDto
    {
        public string TemplateName { get; set; }
        public List<CreateFieldDto> Fields { get; set; }
    }
}
