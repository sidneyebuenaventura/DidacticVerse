using EfVueMantle;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidacticVerse.Models;

public class AccountModel : ModelBase, ISoftDelete
{
    public AccountModel() { }
    public AccountModel(AccountCreateDTO newAccount)
    {
        FirstName = newAccount.FirstName;
        LastName = newAccount.LastName;
        EmailAddress = newAccount.EmailAddress;
        Birthdate = newAccount.Birthdate;
        TermsConditions = newAccount.TermsConditions;
        TermsConditionsDate = new DateTime();
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? EmailAddress { get; set; }
    public DateTime? Birthdate { get; set; }

    [JsonIgnore]
    public string? HashedPassword { get; set; }

    [JsonIgnore]
    public string? Subject { get; set; }

    [JsonIgnore]
    public bool? Suspended { get; set; } = false;
    [JsonIgnore]
    public DateTime? SuspendedDate { get; set; }

    //[JsonIgnore]
    //public List<AuthenticationScheme>? ExternalLogins { get; set; }

    public bool TermsConditions { get; set; } = false;
    [JsonIgnore]
    public DateTime TermsConditionsDate { get; set; }
    [NotMapped]
    public string? Token { get; set; }
    [NotMapped]
    public string? RefreshToken { get; set; }

    [JsonIgnore]
    public bool? Deleted { get; set; }
    [JsonIgnore]
    public long? DeletedByUserId { get; set; }
    [JsonIgnore]
    public DateTime? DeletedDateTime { get; set; }

    [JsonIgnore]
    public string? ResetToken { get; set; }
    [JsonIgnore]
    public DateTime? ResetTime { get; set; }

    public string DisplayName { get; set; } = "";
    public string? Description { get; set; }


    public long? AvatarId { get; set; }
    [ForeignKey("AvatarId")]
    public ImageModel? Avatar { get; set; }

    [NotMapped]
    public List<long> HiddenUsersIds { get; set; } = new();
    [NotMapped]
    public List<UserProfileModel> HiddenUsers { get; set; } = new();

    [NotMapped]
    public List<long> DiscussionTopicsIds { get; set; } = new();
    [JsonIgnore]
    public string DiscussionTopics { get; set; } = string.Empty;
}
