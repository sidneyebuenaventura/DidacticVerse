using EfVueMantle;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidacticVerse.Models;

public class DiscussionModel : ModelBase
{

    public long? LocationId { get; set; }
    public LocationModel? Location { get; set; }
    public string Content { get; set; }

    public long UserId { get; set; }
    [ForeignKey("UserId")]
    [EfVuePropertyType("UserProfileModel")]
    public AccountModel? User { get; set; }

    [NotMapped]
    public List<long> CommentsIds { get; set; } = new();
    public List<CommentModel> Comments { get; set; } = new();
    [NotMapped]
    public long? CommentsCount { get; set; } = 0;


    [NotMapped]
    public List<long> TopicsIds { get; set; } = new();
    public List<TopicModel> Topics { get; set; } = new();


    [NotMapped]
    public List<long> ImagesIds { get; set; } = new();
    public List<ImageModel> Images { get; set; } = new();

    [JsonIgnore]
    public List<DiscussionVoteModel> DiscussionVotes { get; set; } = new();

    [NotMapped]
    public long? DiscussionVoteCount { get; set; }

    [NotMapped]
    public bool? CurrentDiscussionVote { get; set; }

    [NotMapped]
    public float? Weight { get; set; }

}
