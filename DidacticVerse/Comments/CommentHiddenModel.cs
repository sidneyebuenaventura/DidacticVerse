using EfVueMantle;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidacticVerse.Models;

public class CommentHiddenModel
{
    public long UserId { get; set; }
    [ForeignKey("UserId")]
    [EfVuePropertyType("UserProfileModel")]

    public AccountModel? User { get; set; }
    public long CommentId { get; set; }
}
