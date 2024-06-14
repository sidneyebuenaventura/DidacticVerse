using EfVueMantle;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidacticVerse.Models;

public class UserProfileModel : ModelBase
{
    public UserProfileModel() { }
    public UserProfileModel(AccountModel account)
    {
        Id = account.Id;
        DisplayName = account.DisplayName;
        AvatarId = account.AvatarId;
        Description = account.Description;
        HiddenUsersIds = account.HiddenUsersIds;
    }

    public string DisplayName { get; set; } = "";

    public long? AvatarId { get; set; }
    [ForeignKey("AvatarId")]
    public ImageModel? Avatar { get; set; }

    public string? Description { get; set; }

    [NotMapped]
    public List<long> HiddenUsersIds { get; set; } = new();
    [NotMapped]
    public List<UserProfileModel> HiddenUsers { get; set; } = new();

}

