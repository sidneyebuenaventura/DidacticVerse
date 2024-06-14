using EfVueMantle;
using Newtonsoft.Json;

namespace DidacticVerse.Models;

public class TopicModel : ModelBase
{
    public string Title { get; set; }
    private string? _icon { get; set; }
    public string Icon { 
        get { return _icon ?? $"fa-solid fa-{Title[..1].ToLowerInvariant()}"; }
        set { _icon = value; }
    }

    [JsonIgnore]
    public List<DiscussionModel> Discussions { get; set; } = new();
}
