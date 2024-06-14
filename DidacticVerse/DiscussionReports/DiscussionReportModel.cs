using DidacticVerse.Enums;
using EfVueMantle;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidacticVerse.Models;

public class DiscussionReportModel : ModelBase
{

    public long DiscussionId { get; set; }
    [ForeignKey("DiscussionId")]
    public DiscussionModel? Discussion { get; set; }

    public ReportReasons ReportReason { get; set; }

    public long ReportingUserId { get; set; }
    [ForeignKey("ReportingUserId")]
    [EfVuePropertyType("UserProfileModel")]

    public AccountModel? ReportingUser { get; set; }


}
