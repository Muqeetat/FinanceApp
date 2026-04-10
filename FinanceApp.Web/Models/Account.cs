using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Web.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty; // e.g., "Personal Savings"

        [Required]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; } // Current calculated balance

        // Navigation Property: Link to all expenses for this account
        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
