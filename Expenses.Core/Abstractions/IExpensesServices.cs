using Expenses.Core.DTO;
using Expenses.DB;

namespace Expenses.Core.Abstractions
{
    public interface IExpensesServices
    {
        List<ExpenseDto> GetExpenses();
        ExpenseDto GetExpense(int id);
        ExpenseDto CreateExpense(ExpenseModel expense);
        void DeleteExpense(ExpenseDto expense);
        ExpenseDto EditExpense(ExpenseDto expense);

    }
}
