using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Investment.Models.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount  { get; set; }
        public string Description { get; set; }
        
    }
    

}