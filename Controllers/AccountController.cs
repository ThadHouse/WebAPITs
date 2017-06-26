using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPITs.Models;

namespace WebAPITs.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly AccountContext _context;

        public AccountController(AccountContext context)
        {
            _context = context;

            if (!_context.Accounts.Any())
            {
                _context.Accounts.Add(new Account()
                {
                    Username = "Bob",
                    Password = "123",
                    Money = 0
                });
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Get a list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        [HttpGet]
        public IEnumerable<Account> GetAll()
        {
            return _context.Accounts.ToList();
        }

        [HttpGet("{id}", Name = "GetAccount")]
        public IActionResult GetById(int id)
        {
            var item = _context.Accounts.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] Account param)
        {
            if (param == null)
            {
                return BadRequest();
            }

            _context.Accounts.Add(param);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetAccount", new { id = param.Id}, param);
        }
    }
}
