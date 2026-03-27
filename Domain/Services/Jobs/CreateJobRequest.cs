namespace Domain.Services.Jobs;

public class CreateJobRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Requirements { get; set; }
}