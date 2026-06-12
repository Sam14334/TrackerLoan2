using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Principal;

namespace TrackerLoanAPI.Controllers
{
    [Route("api/loans")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly AppService.AppService _appservice; 

        public LoanController()
        {
            _appservice = new AppService.AppService();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Models.Account>> GetAllAccounts()
        {
            var accounts = _appservice.GetAllAccounts();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public ActionResult<Models.Account> GetAccountByReference(string id)
        {
            var account = _appservice.GetAccountByReference(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        public IActionResult CreateAccount([FromBody] Models.AccountViewModel account)
        {
            if (account == null)
            {
                return BadRequest("Account data is required.");
            }

            var newAccount = new Models.Account
            {
                accountReference = account.accountReference,
                duration = account.duration,
                daysPassed = account.daysPassed,
                interestRate = account.interestRate,
                penaltyRate = account.penaltyRate,
                amount = account.amount

            };

            var created = _appservice.RegisterAccount(newAccount);

            if (!created)
            {
                return Conflict("Account could not be created.");
            }

            return CreatedAtAction(
                nameof(GetAccountByReference),
                new { id = newAccount.accountReference },
                newAccount);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateAccount(string id, [FromBody] Models.AccountViewModel account)
        {
            if (account == null)
            {
                return BadRequest("Account data is required.");
            }

            var existingAccount = _appservice.GetAccountByReference(id);

            if (existingAccount == null)
            {
                return NotFound();
            }

            var updatedAccount = new Models.Account
            {
                accountReference = account.accountReference,
                duration = account.duration,
                daysPassed = account.daysPassed,
                interestRate = account.interestRate,
                penaltyRate = account.penaltyRate,
                amount = account.amount
            };

            _appservice.UpdateAccount(existingAccount, updatedAccount);

            return CreatedAtAction(
                nameof(GetAccountByReference),
                new { id = updatedAccount.accountReference },
                updatedAccount);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(string id)
        {
            var existingAccount = _appservice.GetAccountByReference(id);

            if (existingAccount == null)
            {
                return NotFound();
            }

            _appservice.DeleteAccount(existingAccount);

            return NoContent();
        }

        [HttpPost("reset")]
        public IActionResult ResetAllAccounts()
        {            
            bool isReset = _appservice.ResetAccounts();

            if (isReset)
            {
                
                return Ok(new { message = "All accounts have been successfully reset to their default values." });
            }

            
            return StatusCode(500, new { error = "An error occurred while resetting the accounts." });
        }
    }
}
