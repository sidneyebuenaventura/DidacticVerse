using EfVueMantle;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidacticVerse.Models;

public class UserProfileHiddenModel
{
    public long UserId { get; set; }
    [ForeignKey("UserId")]
    [EfVuePropertyType("UserProfileModel")]
    public AccountModel? User { get; set; }

    public long HiddenUserId { get; set; }
    [ForeignKey("HiddenUserId")]
    [EfVuePropertyType("UserProfileModel")]
    public AccountModel? HiddenUser { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;
}

