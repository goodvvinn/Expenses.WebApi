using Expenses.Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsServices _statisticsService;

        public StatisticsController(IStatisticsServices statisticsService)
        {
            _statisticsService = statisticsService;
        }
        
        [HttpGet]
        public IActionResult GetExpenseAmountPerCategory() 
        {
            var result = _statisticsService.GetExpenseAmountPerCategory();
            return Ok(result);
        }
    }
}
