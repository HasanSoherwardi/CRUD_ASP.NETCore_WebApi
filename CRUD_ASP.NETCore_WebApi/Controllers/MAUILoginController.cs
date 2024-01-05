using CRUD_ASP.NETCore_WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CRUD_ASP.NETCore_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MAUILoginController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ILogger<MAUILoginController> _logger;

        public MAUILoginController(IConfiguration configuration, ILogger<MAUILoginController> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Use the connection to execute SQL commands
            }
            _logger.LogInformation("Get Connection Success...");
            return Ok("Connection Success...");
        }

        [HttpGet("GetAllUsers")]
        public IEnumerable<User> GetAllUsers()
        {
            List<User> fileData = new List<User>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * From TblUser", connection);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    User file1 = new User();
                    file1.Id = Convert.ToInt32(rdr["Id"]);
                    file1.Name = rdr["Name"].ToString();
                    file1.DOB = Convert.ToDateTime(rdr["DOB"].ToString());
                    file1.POB = rdr["POB"].ToString();
                    file1.Email = rdr["Email"].ToString();
                    file1.UserId = rdr["UserId"].ToString();
                    file1.Password = rdr["Password"].ToString();
                    file1.myArray = (byte[])rdr["myArray"];
                    fileData.Add(file1);
                }
            }
            _logger.LogInformation("Get and Fetch All User data Successfully...");
            return fileData;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(string UserId, string Password)
        {
            List<User> fileData = new List<User>();
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * From TblUser Where UserId=@UId and Password=@Pass", connection);
                cmd.Parameters.AddWithValue("@UId", UserId.ToString());
                cmd.Parameters.AddWithValue("@Pass", Password.ToString());
                SqlDataReader rdr =  cmd.ExecuteReader();
                while (rdr.Read())
                {
                    User file1 = new User();
                    file1.Id = Convert.ToInt32(rdr["Id"]);
                    file1.Name = rdr["Name"].ToString();
                    file1.DOB = Convert.ToDateTime(rdr["DOB"].ToString());
                    file1.POB = rdr["POB"].ToString();
                    file1.Email = rdr["Email"].ToString();
                    file1.UserId = rdr["UserId"].ToString();
                    file1.Password = rdr["Password"].ToString();
                    file1.myArray = (byte[])rdr["myArray"];
                    fileData.Add(file1);
                }
            }
            _logger.LogInformation("Get and Fetch Logged-in User data Successfully...");
            return Ok(fileData);
        }

        [HttpPost("DownloadFile")]
        public async Task<IActionResult> DownloadFile([FromBody] byte[] bytes, string fileName)
        {
            MemoryStream memory = new MemoryStream(bytes);
            return File(memory, "image/png", Path.GetFileName(fileName));
        }

        [HttpPost]
        public Response SaveUser(User userdata)
        {
            Response response = new Response();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    string sSQL = "insert into TblUser (Name,DOB,POB,Email,UserId,Password,myArray)values(@Name,@DOB,@POB,@Email,@UserId,@Password,@myArray)";
                    var cmd = new SqlCommand(sSQL, connection);
                    cmd.Parameters.AddWithValue("@Name", userdata.Name.ToString());
                    cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(userdata.DOB.ToString()));
                    cmd.Parameters.AddWithValue("@POB", userdata.POB.ToString());
                    cmd.Parameters.AddWithValue("@Email", userdata.Email.ToString());
                    cmd.Parameters.AddWithValue("@UserId", userdata.UserId.ToString());
                    cmd.Parameters.AddWithValue("@Password", userdata.Password.ToString());
                    cmd.Parameters.AddWithValue("@myArray", (byte[])userdata.myArray);

                    int temp = 0;

                    temp = cmd.ExecuteNonQuery();
                    if (temp > 0)
                    {
                        _logger.LogInformation("User Registration Successfully...");
                        response.GetMessage = "Register successfully";
                        response.Status = 1;
                    }
                    else
                    {
                        _logger.LogInformation("User Registration Failed... Try with different UserId");
                        response.GetMessage = "Registration Failed. Try with different UserId.";
                        response.Status = 0;
                    }

                }
                catch (Exception ex)
                {
                    response.GetMessage = ex.Message;
                    response.Status = 0;
                }
                finally
                {
                    connection.Close();
                }
            }
            return response;
        }

        [HttpPut]
        public Response UpdateUser(User userdata)
        {
            Response response = new Response();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    string sSQL = "Update TblUser set Name=@Name,DOB=@DOB,POB=@POB,Email=@Email,UserId=@UserId,Password=@Password,myArray=@myArray Where Id=@Id";
                    var cmd = new SqlCommand(sSQL, connection);
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(userdata.Id.ToString()));
                    cmd.Parameters.AddWithValue("@Name", userdata.Name.ToString());
                    cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(userdata.DOB.ToString()));
                    cmd.Parameters.AddWithValue("@POB", userdata.POB.ToString());
                    cmd.Parameters.AddWithValue("@Email", userdata.Email.ToString());
                    cmd.Parameters.AddWithValue("@UserId", userdata.UserId.ToString());
                    cmd.Parameters.AddWithValue("@Password", userdata.Password.ToString());
                    cmd.Parameters.AddWithValue("@myArray", (byte[])userdata.myArray);

                    int temp = 0;

                    temp = cmd.ExecuteNonQuery();
                    if (temp > 0)
                    {
                        _logger.LogInformation("User Update Successfully...");
                        response.GetMessage = "Update successfully";
                        response.Status = 1;
                    }
                    else
                    {
                        _logger.LogInformation("User Updation Failed... Try with different UserId");
                        response.GetMessage = "Updation Failed. Try with different UserId.";
                        response.Status = 0;
                    }
                }
                catch (Exception ex)
                {
                    response.GetMessage = ex.Message;
                    response.Status = 0;
                }
                finally
                {
                    connection.Close();
                }
            }
            return response;
        }

        [HttpDelete]
        public Response DeleteUser(int id)
        {
            Response response = new Response();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    string sSQL = "Delete from TblUser Where Id=@Id";
                    var cmd = new SqlCommand(sSQL, connection);
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id.ToString()));

                    int temp = 0;

                    temp = cmd.ExecuteNonQuery();
                    if (temp > 0)
                    {
                        _logger.LogInformation("User Delete Successfully... ");
                        response.GetMessage = "Delete successfully";
                        response.Status = 1;
                    }
                    else
                    {
                        _logger.LogInformation("User Deletion Failed... ");
                        response.GetMessage = "Deletion Failed.";
                        response.Status = 0;
                    }

                }
                catch (Exception ex)
                {
                    response.GetMessage = ex.Message;
                    response.Status = 0;
                }
                finally
                {
                    connection.Close();
                }
            }
            return response;
        }
    }
}
