using CRUD_ASP.NETCore_WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_ASP.NETCore_WebApi.Services
{
    public interface IMauiLoginServices
    {
        Response Get();
        
        IEnumerable<User> GetAllUsers();

        List<User> GetUser(string UserId, string Password);

        FileContentResult DownloadFile(byte[] bytes, string fileName);

        Response SaveUser(User userdata);

        Response UpdateUser(User userdata);

        Response DeleteUser(int id);

    }
}
