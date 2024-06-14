using DidacticVerse.Accounts;
using DidacticVerse.Models;
using EfVueMantle;
using Microsoft.EntityFrameworkCore;

namespace DidacticVerse.Services;

public class DiscussionService : ServiceBase<DiscussionModel, long>
{
    private readonly DidacticVerseContext _context;
    private readonly DbSet<DiscussionModel> _discussions;
    private readonly AccountProvider _accountProvider;

    public DiscussionService(DidacticVerseContext context, AccountProvider accountProvider) : base(context.Discussions, context)
    {
        _context = context;
        _discussions = context.Discussions;
        _accountProvider = accountProvider;
    }

    public override DiscussionModel Save(DiscussionModel data)
    {
        data.UserId = _accountProvider.GetAccountId();
        return base.Save(data);
    }

    public bool ToggleVote(long discussionId, long accountId)
    {
        //TODO real user ID
        var vote = _context.DiscussionVotes.Where(x => x.DiscussionId == discussionId && x.UserId == accountId).FirstOrDefault();
        if (vote == null)
        {            
            _context.DiscussionVotes.Add(new DiscussionVoteModel() { DiscussionId = discussionId, UserId = accountId});
            _context.SaveChanges();
            return true;
        } else
        {
            _context.DiscussionVotes.Remove(vote);
            _context.SaveChanges();
            return false;
        }
    }

    public bool Report(ReportDTO reportDTO, long userId)
    {
        if (reportDTO == null) return false; //TODO or error out?
        var discussionReport = new DiscussionReportModel() { 
            DiscussionId = reportDTO.Id, 
            ReportingUserId = userId,
            ReportReason = reportDTO.ReportReason
        };
        _context.DiscussionReports.Add(discussionReport);
        _context.SaveChanges();
        return true;
    }

    public bool Hide(long dicussionId, long userId)
    {
        var hide = new DiscussionHiddenModel()
        {
            DiscussionId = dicussionId,
            UserId = userId,
        };
        _context.DiscussionHides.Add(hide);
        _context.SaveChanges();
        return true;
    }

    public long? GetDaily()
    {
        return _context.DailyDiscussion
            .Where(x => x.StartDate < DateTime.Now)
            .OrderByDescending(x => x.StartDate)
            .FirstOrDefault()?
            .DiscussionId;
    }

    public List<long> GetFutures()
    {
        return _context.DailyDiscussion
            .Where(x => x.StartDate > DateTime.Now.AddDays(-1))
            .Select(x => x.DiscussionId)
            .ToList();
    }
}
