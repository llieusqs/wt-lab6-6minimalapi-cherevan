namespace wt_lab6_6minimalapi_cherevan.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "To Do";      // To Do, In Progress, Done
    public string Priority { get; set; } = "Medium";   // Low, Medium, High
    public int ProjectId { get; set; }
    public int AssigneeId { get; set; }
    public DateTime DueDate { get; set; }
}
