using TaskManagementStystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementStystem.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskModel>> GetAllTasksAsync();
        Task<(IEnumerable<TaskModel> tasks, int totalRecords)> GetAllTasksAsync(string search, int page, int pageSize);
        Task<TaskModel> GetTaskByIdAsync(int id);
        Task<int> AddTaskAsync(TaskModel task);
        Task<int> UpdateTaskAsync(TaskModel task);
        Task<int> DeleteTaskAsync(int id);
    }
}
