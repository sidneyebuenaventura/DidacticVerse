using DidacticVerse.Models;
using DidacticVerse.Services;
using EfVueMantle;
using Microsoft.AspNetCore.Authorization;

namespace DidacticVerse.Controllers;

[Authorize]
public class DiscussionReportController : ControllerBase<DiscussionReportModel, DiscussionReportService>
{
    public DiscussionReportService _discussionReportsService;
    public DiscussionReportController(DiscussionReportService discussionReportsService) : base(discussionReportsService)
    {
        _discussionReportsService = discussionReportsService;
    }
}
