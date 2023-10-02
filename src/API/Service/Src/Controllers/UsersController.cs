using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Service.Src.Controllers
{
    [ApiController, Route("api/users")]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> RegisterUser()
        {
            return Ok("Registered");
        }

    }
}
