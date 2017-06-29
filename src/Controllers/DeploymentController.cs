using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPITs.Models;

namespace WebAPITs.Controllers
{
    [Route("api/[controller]")]
    public class DeploymentController : Controller
    {
        [HttpPost]
        [Route("AllKeys")]
        public IActionResult GetAPIKeys([FromBody] APIAccount account) 
        {
            if (account.Username == "ADMIN" && account.Password == "123456") 
            {
                return new JsonResult(new string[] {"123344", "2423523", "2546456"});
            }
            return Unauthorized();
        }

        /// <summary>
        /// </summary>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPost]
        [Route("DestroyAllKeys")]
        [ProducesResponseType(401)]
        public IActionResult DestroyAllAPIKeys([FromBody] APIAccount account) 
        {
            if (account.Username == "ADMIN" && account.Password == "123456") 
            {
                return Ok();
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("CreateKey")]
        public IActionResult CreateAPIKey([FromBody] APIAccount account)
        {
            return new JsonResult(account.Username.GetHashCode().ToString() + account.Password.GetHashCode().ToString());
        }
    }
}