using DidacticVerse.Accounts;
using DidacticVerse.Models;
using DidacticVerse.Services;
using EfVueMantle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DidacticVerse.Controllers;

public class BetaFeedbackController : ControllerBase<BetaFeedbackModel, BetaFeedbackService>
{
    private BetaFeedbackService _betaFeedbackService;
    private AccountProvider _accountProvider;
    private DidacticVerseContext _context;

    public BetaFeedbackController(
        BetaFeedbackService betaFeedbackService, 
        AccountProvider accountProvider,
        DidacticVerseContext didacticVerseContext) : base(betaFeedbackService)
    {
        _betaFeedbackService = betaFeedbackService;
        _accountProvider = accountProvider;
        _context = didacticVerseContext;
    }
    public override IActionResult Save(BetaFeedbackModel model)
    {
        model.ReportingUserId = _context.Accounts.Where(x => x.Id == _accountProvider.GetAccountId()).First().Id;
        return base.Save(model);
    }

    [Authorize]
    public override IActionResult Delete(long id)
    {
        return base.Delete(id);
    }
}
