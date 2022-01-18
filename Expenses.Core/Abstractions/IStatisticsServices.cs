namespace Expenses.Core.Abstractions
{
    public interface IStatisticsServices
    {
        IEnumerable<KeyValuePair<string, double>> GetExpenseAmountPerCategory();
    }
}
