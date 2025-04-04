using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApi.DTOs;
using TaskManagementApi.Repositories;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // GET: api/tasks
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult<PaginatedResponse<TaskDto>>> GetTasks(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1 || pageSize > 50)
            {
                return BadRequest("Invalid pagination parameters. Page must be >= 1, PageSize must be between 1 and 50.");
            }

            var tasks = await _taskRepository.GetAllTasksAsync(page, pageSize);
            var totalCount = await _taskRepository.GetTotalTaskCountAsync();

            var taskDtos = tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            });

            var response = new PaginatedResponse<TaskDto>
            {
                Items = taskDtos,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };

            return Ok(response);
        }

        // GET: api/tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            var taskDto = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };

            return Ok(taskDto);
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto createTaskDto)
        {
            var task = new Models.Task
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                DueDate = createTaskDto.DueDate,
                Priority = createTaskDto.Priority,
                Status = createTaskDto.Status
            };

            var createdTask = await _taskRepository.CreateTaskAsync(task);

            var taskDto = new TaskDto
            {
                Id = createdTask.Id,
                Title = createdTask.Title,
                Description = createdTask.Description,
                DueDate = createdTask.DueDate,
                Priority = createdTask.Priority,
                Status = createdTask.Status,
                CreatedAt = createdTask.CreatedAt,
                UpdatedAt = createdTask.UpdatedAt
            };

            return CreatedAtAction(nameof(GetTask), new { id = taskDto.Id }, taskDto);
        }

        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto updateTaskDto)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(id);

            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = updateTaskDto.Title;
            existingTask.Description = updateTaskDto.Description;
            existingTask.DueDate = updateTaskDto.DueDate;
            existingTask.Priority = updateTaskDto.Priority;
            existingTask.Status = updateTaskDto.Status;

            var updatedTask = await _taskRepository.UpdateTaskAsync(existingTask);

            if (updatedTask == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await _taskRepository.DeleteTaskAsync(id);
            
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/tasks/due-next-week
        [HttpGet("due-next-week")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTaskDueNextWeek()
        {
            var tasks = await _taskRepository.GetTaskDueNextWeekAsync();
            
            var taskDtos = tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            });

            return Ok(taskDtos);
        }

        // GET: api/tasks/by-priority
        [HttpGet("by-priority")]
        public async Task<ActionResult<Dictionary<string, int>>> GetTaskCountByPriority()
        {
            var taskCountByPriority = await _taskRepository.GetTaskCountByPriorityAsync();
            return Ok(taskCountByPriority);
        }

        // GET: api/tasks/overdue
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetOverdueTasks()
        {
            var tasks = await _taskRepository.GetOverDueTaskAsync();
            
            var taskDtos = tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            });

            return Ok(taskDtos);
        }

        // PUT: api/tasks/update-status
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateTasksStatus(
            [FromQuery] string currentStatus, 
            [FromQuery] string newStatus)
        {
            if (string.IsNullOrEmpty(currentStatus) || string.IsNullOrEmpty(newStatus))
            {
                return BadRequest("Both currentStatus and newStatus are required.");
            }

            if (!new[] { "todo", "in-progress", "completed" }.Contains(currentStatus) ||
                !new[] { "todo", "in-progress", "completed" }.Contains(newStatus))
            {
                return BadRequest("Status must be one of: todo, in-progress, completed");
            }

            var result = await _taskRepository.UpdateTaskStatusAsync(currentStatus, newStatus);
            
            if (!result)
            {
                return NotFound($"No tasks found with status '{currentStatus}'");
            }

            return NoContent();
        }
    }
}