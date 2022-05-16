// This object represents an incoming update.
// At most one of the optional parameters can be present in any given update.
namespace TgModels;

public record Update
{
    public int UpdateId { get; set; }
    public string? Message { get; set; }
    public string? Edited_message { get; set; }
}
