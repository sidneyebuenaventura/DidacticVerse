using EfVueMantle;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidacticVerse.Models;

public class CommentModel : ModelBase
{
    public string Content { get; set; }

    public long? DiscussionId { get; set; }
    [ForeignKey("DiscussionId")]
    public DiscussionModel? Discussion { get; set; }

    public long? ParentCommentId { get; set; }
    [ForeignKey("ParentCommentId")]
    public CommentModel? ParentComment { get; set; }

    public long UserId { get; set; }
    [ForeignKey("UserId")]
    [EfVuePropertyType("UserProfileModel")]

    public AccountModel? User { get; set; }


    [NotMapped]
    public List<long> ChildCommentsIds { get; set; } = new();
    public List<CommentModel> ChildComments { get; set; } = new();


}
