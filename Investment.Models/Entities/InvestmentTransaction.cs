using System;
using System.ComponentModel.DataAnnotations;

namespace Investment.Models.Entities
{
    public class InvestmentTransaction
    {
        [Key]
        public Guid InvestmentId { get; set; } = Guid.NewGuid();
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
    
}