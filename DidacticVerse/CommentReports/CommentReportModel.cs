using DidacticVerse.Enums;
using EfVueMantle;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidacticVerse.Models;

public class CommentReportModel : ModelBase
{

    public long CommentId { get; set; }
    [ForeignKey("CommentId")]
    public CommentModel? Comment { get; set; }

    public ReportReasons ReportReason { get; set; }

    public long ReportingUserId { get; set; }
    [ForeignKey("ReportingUserId")]
    [EfVuePropertyType("UserProfileModel")]

    public AccountModel? ReportingUser { get; set; }


}
