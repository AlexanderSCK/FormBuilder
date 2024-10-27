using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Infrastructure.Models;

public class FormInstance
{
    [Key]
    public int Id { get; set; }
    public int TemplateId { get; set; }
    public Dictionary<string, double?> FieldValues { get; set; } = new();

    [ForeignKey("TemplateId")]
    public FormTemplate Template { get; set; }
}