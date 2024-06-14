using DidacticVerse.Models;
using DidacticVerse.Services;
using EfVueMantle;
using Microsoft.AspNetCore.Authorization;

namespace DidacticVerse.Controllers;

[Authorize]
public class CommentReportController : ControllerBase<CommentReportModel, CommentReportService>
{
    public CommentReportService _commentReportsService;
    public CommentReportController(CommentReportService commentReportsService) : base(commentReportsService)
    {
        _commentReportsService = commentReportsService;
    }
}
