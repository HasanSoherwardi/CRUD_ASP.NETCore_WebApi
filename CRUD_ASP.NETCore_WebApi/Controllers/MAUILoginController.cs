using CRUD_ASP.NETCore_WebApi.Models;
using CRUD_ASP.NETCore_WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CRUD_ASP.NETCore_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MAUILoginController : ControllerBase
    {
        private readonly IMauiLoginServices _IMauiLoginServices;

        public MAUILoginController(IMauiLoginServices mauiLoginServices)
        {
            _IMauiLoginServices = mauiLoginServices;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _IMauiLoginServices.Get();
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var result = _IMauiLoginServices.GetAllUsers();
            if (result.Count() > 0)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser(string UserId, string Password)
        {
            var result = _IMauiLoginServices.GetUser(UserId, Password);
            if (result.Count() > 0)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost("DownloadFile")]
        public async Task<IActionResult> DownloadFile([FromBody] byte[] bytes, string fileName)
        {
            return _IMauiLoginServices.DownloadFile(bytes, fileName);
        }

        [HttpPost]
        public Response SaveUser(User userdata)
        {
            Response response = new Response();
            response = _IMauiLoginServices.SaveUser(userdata);
            return response;
        }

        [HttpPut]
        public Response UpdateUser(User userdata)
        {
            Response response = new Response();
            response = _IMauiLoginServices.UpdateUser(userdata);
            return response;
        }

        [HttpDelete]
        public Response DeleteUser(int id)
        {
            Response response = new Response();
            response = _IMauiLoginServices.DeleteUser(id);
            return response;
        }
    }
}
