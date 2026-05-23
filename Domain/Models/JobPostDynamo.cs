using Amazon.DynamoDBv2.DataModel;

namespace Domain.Models;

[DynamoDBTable("Jobs")]
public class JobPostDynamo
{
    [DynamoDBHashKey] 
    public string Id { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public string Requirements { get; set; }
    
    
    public DateTime PostedDate { get; set; }
    
    public bool IsActive { get; set; }
}

