using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Web.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }= string.Empty;
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
        [Required]
        public string Category { get; set; }= string.Empty;
        public DateTime Date { get; set; }= DateTime.Now;
        public int AccountId { get; set; }   // Foreign Key

        // Navigation Property: Allows you to access account details from an expense
        public Account? Account { get; set; }
    }
}
