using Mezl.Result.ExampleApp.Application.Requests;
using Mezl.Result.Handler;
using Microsoft.AspNetCore.Mvc;

namespace Mezl.Result.ExampleApp.Controllers
{
    public record UserModel(string Name, string UserName, string Email, int Age);

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IRequestExecutor _requestExecutor;

        public UserController(IRequestExecutor requestExecutor)
        {
            _requestExecutor = requestExecutor;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _requestExecutor.ExecuteAsync(new GetUserByIdRequest(id), CancellationToken.None);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserModel model)
        {
            var (name, userName, email, age) = model;
            var request = new CreateUserRequest(name, userName, email, age);

            var result = await _requestExecutor.ExecuteAsync(request, CancellationToken.None);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }
}