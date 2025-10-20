namespace IRCloudBackend.Domain.Models;
public class IrFile
{
    public int Id { get; set; }
    public string Path { get; set; } = "";
    public int PostId { get; set; }
    public required Post Post { get; set; }
}
