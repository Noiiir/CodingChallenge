using System;
using System.ComponentModel.DataAnnotations;


namespace TaskManagementApi.Models{

public class Task{

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public DateTime? DueDate {get; set;}

    [Required]
    [StringLength(20)]
    public string Priority {get;set;} ="medium";

    [Required]
    [StringLength(20)]
    public string Status {get; set;} = "todo";

    public DateTime CreatedAt {get;set;} = DateTime.Now;

    public DateTime? UpdatedAt {get;set;}
    }
}