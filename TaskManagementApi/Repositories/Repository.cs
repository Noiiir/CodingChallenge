using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories{
    public class TaskRepository :  ITaskRepository{
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context){
            _context = context;
        }

        public async Task<IEnumerable<Models.Task>> GetAllTasksAsync(int page =1, int pageSize = 10){
            return await _context.Tasks
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page -1 ) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public async Task<int> GetTotalTaskCountAsync()
        {
            return await _context.Tasks.CountAsync();
        }

        public async Task<Models.Task> GetTaskByIdAsync(int id){
            return await _context.Tasks.FindAsync(id);
        }
        public async Task<Models.Task> CreateTaskAsync(Models.Task task)
        {
            task.CreatedAt = DateTime.Now;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Models.Task> UpdateTaskAsync(Models.Task task)
        {
            var existingTask = await _context.Tasks.FindAsync(task.Id);

            if (existingTask ==null){
                return null;
            }

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.Priority = task.Priority;
            existingTask.Status = task.Status;
            existingTask.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingTask;
        }

        public async Task<IEnumerable<Models.Task>> GetTaskDueNextWeekAsync(){
            DateTime today = DateTime.Today;
            DateTime nextWeek = today.AddDays(7);

            return await _context.Tasks
            .Where(t => t.DueDate.HasValue &&
            t.DueDate.Value.Date >= today &&
            t.DueDate.Value.Date <= nextWeek)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetTaskCountByPriorityAsync()
        {
            return await _context.Tasks
            .GroupBy(t => t.Priority)
            .Select(g => new {Priority = g.Key, Count = g.Count()})
            .ToDictionaryAsync(x => x.Priority, XmlConfigurationExtensions=> XmlConfigurationExtensions.Count);
        }
        public async Task<IEnumerable<Models.Task>> GetOverDueTaskAsync()
        {
            DateTime today = DateTime.Today;

            return await _context.Tasks
                .Where(t => t.DueDate.HasValue &&
                t.DueDate.Value.Date <today && 
                t.Status != "completed")
                .OrderBy(t=> t.DueDate)
                .ToListAsync();
        }
public async Task<bool> UpdateTaskStatusAsync(string currentStatus, string newStatus)
         {
            var tasks = await _context.Tasks
                .Where(t=> t.Status == currentStatus)
                .ToListAsync();

            if(!tasks.Any())
            {
            return false;
            }

            foreach (var task in tasks)
            {
                task.Status = newStatus;
                task.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
         if (task == null)
            {
            return false;
            }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
        }
    }
}