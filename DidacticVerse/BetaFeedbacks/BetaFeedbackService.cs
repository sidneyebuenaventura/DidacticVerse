using DidacticVerse.Models;
using EfVueMantle;
using Microsoft.EntityFrameworkCore;

namespace DidacticVerse.Services;

public class BetaFeedbackService : ServiceBase<BetaFeedbackModel>
{
    private readonly DidacticVerseContext _context;
    private readonly DbSet<BetaFeedbackModel> _betaFeedbackss;

    public BetaFeedbackService(DidacticVerseContext context) : base(context.BetaFeedbacks, context)
    {
        _context = context;
        _betaFeedbackss = context.BetaFeedbacks;
    }
}
