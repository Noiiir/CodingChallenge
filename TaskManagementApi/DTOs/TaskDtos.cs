using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs{


public class TaskDto
{
    public int Id {get;set;}
    public string Title {get;set;} =string.Empty;
    public string Description {get;set;} = string.Empty;
    public DateTime? DueDate {get;set;}
    public string Priority {get;set;} = string.Empty;
    public string Status {get;set;} = string.Empty;
    public DateTime CreatedAt {get;set;}
    public DateTime? UpdatedAt {get;set;}
}

public class CreateTaskDto{

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title {get;set;} = string.Empty;

    [StringLength(500)]
    public string Description {get;set;} = string.Empty;

    public DateTime? DueDate {get;set;}

    [Required]
    [RegularExpression("low|medium|high",ErrorMessage = "Priority must be one of the following: low, medium, high")]
    public string Priority {get; set;} = "medium";

    [Required]
    [RegularExpression("todo|in-progress|completed", ErrorMessage = "Status must be one of the following: todo, in-progress, completed")]
     public string Status {get; set;} = "todo";

}
public class UpdateTaskDto{

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title {get;set;} = string.Empty;

    [StringLength(500)]
    public string Description {get;set;} = string.Empty;

    public DateTime? DueDate {get;set;}

    [Required]
    [RegularExpression("low|medium|high",ErrorMessage = "Priority must be one of the following: low, medium, high")]
    public string Priority {get; set;}  = string.Empty;

    [Required]
    [RegularExpression("todo|in-progress|completed", ErrorMessage = "Status must be one of the following: todo, in-progress, completed")]
     public string Status {get; set;} = string.Empty;

     
}

public class PaginatedResponse<T>
     {
        public IEnumerable<T> Items {get;set;} = new List<T>();
        public int TotalCount {get;set;}

        public int PageNumber {get;set;}

        public int PageSize {get;set;}

        public int TotalPages => (int)Math.Ceiling((double)TotalCount/PageSize);
     }


}