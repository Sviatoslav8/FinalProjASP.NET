namespace Domain.Models;

public class JobPost
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Requirements { get; set; }
    public DateTime PostedDate { get; set; }
    public bool IsActive { get; set; }

    public List<Application> Applications { get; set; } = new();
}