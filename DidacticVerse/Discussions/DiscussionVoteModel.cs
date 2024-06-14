using EfVueMantle;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidacticVerse.Models;

public class DiscussionVoteModel
{
    public long UserId { get; set; }
    [ForeignKey("UserId")]
    [EfVuePropertyType("UserProfileModel")]
    public AccountModel? User { get; set; }
    public long DiscussionId { get; set; }
}