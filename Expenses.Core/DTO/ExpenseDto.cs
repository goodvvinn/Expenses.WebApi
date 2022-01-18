using Expenses.DB;

namespace Expenses.Core.DTO
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        public static explicit operator ExpenseDto(ExpenseModel e) => new()
        {
           Id = e.Id,
           Description = e.Description,
           Amount = e.Amount,
        };
    }
}
