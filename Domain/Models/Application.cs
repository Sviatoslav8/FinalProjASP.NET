namespace Domain.Models;

public class Application
{
    public int Id { get; set; }

    public int JobPostId { get; set; }
    public JobPost JobPost { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime AppliedDate { get; set; }
    public string CoverLetter { get; set; }
    public string Status { get; set; } = "Pending";
}