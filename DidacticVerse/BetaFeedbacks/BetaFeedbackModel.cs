using DidacticVerse.Enums;
using EfVueMantle;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidacticVerse.Models;

public class BetaFeedbackModel : ModelBase, ISoftDelete
{
    public string? Url { get; set; }
    public string Comment { get; set; }
    public long? ReportingUserId { get; set; }
    [ForeignKey("ReportingUserId")]
    [EfVuePropertyType("UserProfileModel")]

    public AccountModel? ReportingUser { get; set; }

    [JsonIgnore]
    public bool? Deleted { get; set; }
    [JsonIgnore]
    public long? DeletedByUserId { get; set; }
    [JsonIgnore]
    public DateTime? DeletedDateTime { get; set; }
}
