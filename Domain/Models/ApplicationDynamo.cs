namespace Domain.Models;
using Amazon.DynamoDBv2.DataModel;
[DynamoDBTable("Applications")]
public class ApplicationDynamo
{
    [DynamoDBHashKey]
    public string Id { get; set; }

    public string JobPostId { get; set; }
    public string UserId { get; set; }

    public string Status { get; set; }
    public string CoverLetter { get; set; }

    public DateTime AppliedDate { get; set; }
}