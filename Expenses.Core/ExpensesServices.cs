using Expenses.Core.Abstractions;
using Expenses.Core.DTO;
using Expenses.DB;
using Microsoft.AspNetCore.Http;

namespace Expenses.Core
{
    public class ExpensesServices : IExpensesServices
    {
        private readonly AppDbContext _context;
        private readonly User _user;

        public ExpensesServices(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = _context.Users
                .First(u => u.Username == httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public ExpenseDto CreateExpense(ExpenseModel expense)
        {
            expense.User = _user;
            _context.Add(expense);
            _context.SaveChanges();

            return (ExpenseDto)expense;
        }

        public void DeleteExpense(ExpenseDto expense)
        {
            var dbExpense = _context.Expenses.First(x => x.User.Id == _user.Id && x.Id == expense.Id);
            _context.Remove(dbExpense);
            _context.SaveChanges();
        }

        public ExpenseDto GetExpense(int id)
        {
            return _context.Expenses
                .Where(e => e.User.Id == _user.Id && e.Id == id)
                .Select(e => (ExpenseDto)e)
                .First();
        }

        public List<ExpenseDto> GetExpenses()
        {
            return _context.Expenses
                .Where(x => x.User.Id == _user.Id)
                .Select(x => (ExpenseDto)x)
                .ToList();
        }

        public ExpenseDto EditExpense(ExpenseDto expense)
        {
            var dbExpense = _context.Expenses
                .Where(x => x.User.Id == _user.Id && x.Id == expense.Id)
                .First();
            
                dbExpense.Description = expense.Description;
                dbExpense.Amount = expense.Amount;
                _context.SaveChanges();
                return expense;
            
             
        }
    }
}
