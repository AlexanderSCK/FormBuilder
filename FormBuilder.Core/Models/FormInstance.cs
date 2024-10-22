using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.Models
{
    public class FormInstance
    {
        [Key]
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public Dictionary<string, double?> FieldValues { get; set; } 

        [ForeignKey("TemplateId")]
        public FormTemplate Template { get; set; }
    }
}
