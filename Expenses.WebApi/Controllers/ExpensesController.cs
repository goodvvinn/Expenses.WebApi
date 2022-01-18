using Expenses.Core.Abstractions;
using Expenses.Core.DTO;
using Expenses.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpensesServices _expensesService;

        public ExpensesController(IExpensesServices expensesService)
        {
            this._expensesService = expensesService;
        }
        [HttpGet]
        public IActionResult GetExpenses() 
        {
            return Ok (_expensesService.GetExpenses());
        }

        [HttpGet("{id}", Name ="GetExpense")]
        public IActionResult GetExpense(int id) 
        {
            return Ok(_expensesService.GetExpense(id));
        }

        [HttpPost]
        public IActionResult CreateExpense(ExpenseModel expense) 
        {
            var newExpense = _expensesService.CreateExpense(expense);
            return Ok(CreatedAtRoute("GetExpense", new { newExpense.Id }, newExpense));
        }

        [HttpDelete]
        public IActionResult RemoveExpense(ExpenseDto expense) 
        {
            _expensesService.DeleteExpense(expense);

            return Ok();
        }
        [HttpPut]
        public IActionResult EditExpense(ExpenseDto expense) 
        {
            return Ok(_expensesService.EditExpense(expense));
        }
    }
}
