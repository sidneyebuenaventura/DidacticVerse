using DidacticVerse.Accounts;
using DidacticVerse.Models;
using EfVueMantle;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace DidacticVerse.Services;

public class CommentService : ServiceBase<CommentModel>
{
    private readonly DidacticVerseContext _context;
    private readonly DbSet<CommentModel> _comments;
    private readonly AccountProvider _accountProvider;

    public CommentService(DidacticVerseContext context, AccountProvider accountProvider) : base(context.Comments, context)
    {
        _context = context;
        _comments = context.Comments;
        _accountProvider = accountProvider;
    }

    public override CommentModel Save(CommentModel data)
    {
        data.UserId = _accountProvider.GetAccountId();
        return base.Save(data);
    }

    public bool Report(ReportDTO reportDTO, long userId)
    {
        if (reportDTO == null) return false; //TODO or error out?
        var commentReport = new CommentReportModel()
        {
            CommentId = reportDTO.Id,
            ReportingUserId = userId,
            ReportReason = reportDTO.ReportReason
        };
        _context.CommentReports.Add(commentReport);
        _context.SaveChanges();
        return true;
    }

    public bool Hide(long commentId, long userId)
    {
        var hide = new CommentHiddenModel()
        {
            CommentId = commentId,
            UserId = userId,
        };
        _context.CommentHides.Add(hide);
        _context.SaveChanges();
        return true;
    }
}
