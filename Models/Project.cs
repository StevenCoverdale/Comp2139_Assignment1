using System.ComponentModel.DataAnnotations;

namespace assignment1.Models;

public class Project
{
    /// <summary>
    /// The unique identifier for the project
    /// </summary>
    public int ProjectId { get; set; }
    
    /// <summary>
    /// Required field describing a projets name
    /// </summary>
    [Required]
    public required string Name { get; set; }
    
    public string Description { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    public string? Status { get; set; }
}