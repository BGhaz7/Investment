﻿using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Investment.Models.Entities
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; }
        [Required]
        public int UserId { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount  { get; set; }
        public string Description { get; set; }
        
    }
    

}