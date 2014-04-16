using System.ComponentModel.DataAnnotations;

namespace ARK.Model
{
    public class DamageDescription
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string NeededMaterials { get; set; }
    }
}