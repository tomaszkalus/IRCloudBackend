namespace IRCloudBackend.Domain.Models;
public class PostTag
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public required Post Post { get; set; }
    public string Tag { get; set; } = "";
}
