using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
        public interface ITaskRepository
        {
            Task<IEnumerable<Models.Task>> GetAllTasksAsync(int page = 1, int PageSize = 10);
            Task<int> GetTotalTaskCountAsync();
            Task<Models.Task> GetTaskByIdAsync(int id);
            Task<Models.Task> CreateTaskAsync(Models.Task task);
            Task<Models.Task> UpdateTaskAsync(Models.Task task);
            Task<bool> DeleteTaskAsync(int id);
            Task<IEnumerable<Models.Task>> GetTaskDueNextWeekAsync();
            Task<Dictionary<string,int>> GetTaskCountByPriorityAsync();
            Task<IEnumerable<Models.Task>> GetOverDueTaskAsync();
            Task<bool> UpdateTaskStatusAsync(string currentStatus, string newStatus);
        }
}