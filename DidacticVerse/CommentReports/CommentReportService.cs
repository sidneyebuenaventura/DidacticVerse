using DidacticVerse.Models;
using EfVueMantle;
using Microsoft.EntityFrameworkCore;

namespace DidacticVerse.Services;

public class CommentReportService : ServiceBase<CommentReportModel>
{
    private readonly DidacticVerseContext _context;
    private readonly DbSet<CommentReportModel> _commentReportss;

    public CommentReportService(DidacticVerseContext context) : base(context.CommentReports, context)
    {
        _context = context;
        _commentReportss = context.CommentReports;
    }
}
