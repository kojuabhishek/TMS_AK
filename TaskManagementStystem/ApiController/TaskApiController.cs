using Microsoft.AspNetCore.Mvc;
using TaskManagementStystem.Interfaces;
using TaskManagementStystem.Models;

namespace TaskManagementSystem.Controllers.Api
{
    [Route("api/TaskApi")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        /*  [HttpGet("GetAllTasks")]
          public async Task<IActionResult> GetAllTasks()
          {
              var tasks = await _taskRepository.GetAllTasksAsync();
              return Ok(tasks);
          }
  */

        [HttpGet("GetAllTasks")]
        public async Task<IActionResult> GetAllTasks(string search = "", int page = 1, int pageSize = 5)
        {
            var (tasks, totalRecords) = await _taskRepository.GetAllTasksAsync(search, page, pageSize);
            return Ok(new { data = tasks, totalRecords });
        }


        [HttpGet("GetTaskById/{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask([FromBody] TaskModel task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _taskRepository.AddTaskAsync(task);
            return Ok(new { message = "Task added successfully" });
        }

        [HttpPut("UpdateTask/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskModel task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _taskRepository.UpdateTaskAsync(task);
            return Ok(new { message = "Task updated successfully" });
        }

        [HttpDelete("DeleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskRepository.DeleteTaskAsync(id);
            return Ok(new { message = "Task deleted successfully" });
        }
    }
}
