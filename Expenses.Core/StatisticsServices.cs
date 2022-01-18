using Expenses.Core.Abstractions;
using Expenses.DB;
using Microsoft.AspNetCore.Http;

namespace Expenses.Core
{
    public class StatisticsServices : IStatisticsServices
    {
        private readonly AppDbContext _appDbContext;
        private readonly User _user;

        public StatisticsServices(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _user = appDbContext.Users
                .First(u => u.Username == httpContextAccessor.HttpContext.User.Identity.Name);
        }
        public IEnumerable<KeyValuePair<string, double>> GetExpenseAmountPerCategory()
        {
            return _appDbContext.Expenses
                    .Where(e => e.User.Id == _user.Id)
                    .AsEnumerable()
                    .GroupBy(c => c.Description)
                    .ToDictionary(c => c.Key, c => c.Sum(s => s.Amount))
                    .Select(c => new KeyValuePair<string, double>(c.Key, c.Value));
        }
    }
}
