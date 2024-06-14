using DidacticVerse.Models;
using EfVueMantle;
using Microsoft.EntityFrameworkCore;

namespace DidacticVerse.Services;

public class DiscussionReportService : ServiceBase<DiscussionReportModel>
{
    private readonly DidacticVerseContext _context;
    private readonly DbSet<DiscussionReportModel> _discussionReportss;

    public DiscussionReportService(DidacticVerseContext context) : base(context.DiscussionReports, context)
    {
        _context = context;
        _discussionReportss = context.DiscussionReports;
    }
}
