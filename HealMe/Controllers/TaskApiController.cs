using HealMe.DAO;
using HealMe.DTO;
using HealMe.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealMe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskApiController : Controller
    {
        private readonly ITaskService _taskService;
        public TaskApiController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("GetAllTask")]
        public async Task<ActionResult<TaskPagedResult>> GetAllTask(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] long? userId = null,
            [FromQuery] bool? completed = null)
        {
            try
            {
                var result = await _taskService.GetAllTasksAsync(page, pageSize, userId, completed);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy dữ liệu", error = ex.Message });
            }
        }
    }
}
