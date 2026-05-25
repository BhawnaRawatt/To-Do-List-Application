using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using PracticeMachine.Model;
using System.Data;

namespace PracticeMachine.Controllers
{
    [ApiController]
    [Route ("api/[controller]/[Action]")]
    public class TodoList:ControllerBase
    {
        private readonly string _connectionString;

        public TodoList(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateToDoRequest request)
        {
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("PRC_CREATE_TODOLIST", con);
            
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Data", request.Data);
            cmd.Parameters.AddWithValue("@Subject", request.Subject);

            await con.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            await con.CloseAsync();

            if (rows > 0)
            {
                return Ok(new
                {
                    Message = "Create Successful"
                });
            }

            return BadRequest(new
            {
                Message = "Failed to create todo "
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            DataTable dt = new DataTable();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("PRC_GET_TODOLIST", con);
           
            cmd.CommandType = CommandType.StoredProcedure;
            await con.OpenAsync();

            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                  sda.Fill(dt);
            }
            

            if(dt.Rows.Count>0)
            {
                return Ok(new
                {
                    data = JsonConvert.SerializeObject(dt)
                });
            }

            return Ok(new
            {
                data = new object[] {},
                Message = "No record found"

            });
        }

        [HttpGet]
        
        public async Task<IActionResult> GetData([FromQuery] string SearchText)
        {
            DataTable dt = new DataTable();

            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("PRC_SEARCH_TODOLIST", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SearchText", SearchText);
            await con.OpenAsync();

            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                sda.Fill(dt);
            }

                return Ok(new
                {
                    data = JsonConvert.SerializeObject(dt)

                });
            
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdataTodoRequest request)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("PRC_UPDATE_TODOLIST", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", request.Id);
            cmd.Parameters.AddWithValue("@Subject", request.Subject);
            cmd.Parameters.AddWithValue("@Data", request.Data);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();

            if (rows > 0)
            {
                return Ok(new
                {
                    Message = "Data Updated"
                });
            }

            return BadRequest(new
            {
                Message = "Data is not Updated"
            });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("PRC_DELETE_TODOLIST", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            await con.OpenAsync();

            var rows = await cmd.ExecuteNonQueryAsync();

            if (rows > 0) {

                return Ok(new { 
                    Message = "Row is Deleted"
                });
            }

            return BadRequest(new
            {
                Message = "Not Deleted"
            });

        }
    }
}
