using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TaskManagementStystem.Interfaces;
using TaskManagementStystem.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementStystem.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<TaskRepository> _logger;

        public TaskRepository(IConfiguration configuration, ILogger<TaskRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);
        public async Task<(IEnumerable<TaskModel> tasks, int totalRecords)> GetAllTasksAsync(string search = "", int page = 1, int pageSize = 5)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
        WITH TaskCTE AS (
            SELECT 
                Id, Title, Status, DueDate, 
                COUNT(*) OVER() AS TotalRecords
            FROM Tasks
            WHERE (@search = '' OR Title LIKE '%' + @search + '%' OR Status LIKE '%' + @search + '%')
        )
        SELECT * FROM TaskCTE
        ORDER BY DueDate DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            var tasks = await connection.QueryAsync<TaskModel>(query, new
            {
                search,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });

            int totalRecords = tasks.Any() ? tasks.First().TotalRecords : 0;

            return (tasks, totalRecords);
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
        {
            try
            {
                using (var db = CreateConnection())
                {
                    string sql = "SELECT * FROM Tasks ORDER BY CreatedAt DESC";
                    return await db.QueryAsync<TaskModel>(sql);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all tasks");
                throw new Exception("Error retrieving all tasks", ex);
            }
        }

        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            try
            {
                using (var db = CreateConnection())
                {
                    string sql = "SELECT * FROM Tasks WHERE Id = @Id";
                    return await db.QueryFirstOrDefaultAsync<TaskModel>(sql, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task with Id {TaskId}", id);
                throw new Exception($"Error retrieving task with Id {id}", ex);
            }
        }

        public async Task<int> AddTaskAsync(TaskModel task)
        {
            try
            {
                using (var db = CreateConnection())
                {
                    string sql = "INSERT INTO Tasks (Title, Description, Status, DueDate) VALUES (@Title, @Description, @Status, @DueDate)";
                    return await db.ExecuteAsync(sql, task);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new task: {Task}", task);
                throw new Exception("Error adding new task", ex);
            }
        }

        public async Task<int> UpdateTaskAsync(TaskModel task)
        {
            try
            {
                using (var db = CreateConnection())
                {
                    string sql = "UPDATE Tasks SET Title = @Title, Description = @Description, Status = @Status, DueDate = @DueDate WHERE Id = @Id";
                    return await db.ExecuteAsync(sql, task);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task with Id {TaskId}", task.Id);
                throw new Exception($"Error updating task with Id {task.Id}", ex);
            }
        }

        public async Task<int> DeleteTaskAsync(int id)
        {
            try
            {
                using (var db = CreateConnection())
                {
                    string sql = "DELETE FROM Tasks WHERE Id = @Id";
                    return await db.ExecuteAsync(sql, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task with Id {TaskId}", id);
                throw new Exception($"Error deleting task with Id {id}", ex);
            }
        }
    }
}
