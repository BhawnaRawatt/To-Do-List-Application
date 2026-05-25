using PracticeMachine.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
namespace PracticeMachine.Controllers
{
    [ApiController]
    [Route ("api/[controller]")]
    public class LoginController: ControllerBase
    {
        private readonly string _connectionString;

        public LoginController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection"); 

        }

        [HttpPost]
        public async Task <IActionResult> Login(LoginRequest request)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("PRC_LOGIN_PAGE_PRACTICE", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username",request.Username);
                cmd.Parameters.AddWithValue("@Password", request.Password);

                await con.OpenAsync();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }             

            }

            
            if(dt.Rows.Count > 0)
            {
                return Ok(new
                {
                    Message = "Login Successful",
                    Data = JsonConvert.SerializeObject(dt)
                });
            }

            return Unauthorized(new { Message = "Invaild user and password" });
        }

        
    }
}
