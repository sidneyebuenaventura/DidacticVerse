using DidacticVerse.Accounts;
using DidacticVerse.Enums;
using DidacticVerse.Models;
using DidacticVerse.Services;
using EfVueMantle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace DidacticVerse.Controllers;

[Authorize]
public class DiscussionController : ControllerBase<DiscussionModel, DiscussionService, long>
{
    public DiscussionService _discussionService;
    public AccountProvider _accountProvider;
    private DidacticVerseContext _context;
    public DiscussionController(
        DiscussionService discussionService, 
        AccountProvider accountProvider,
        DidacticVerseContext context) : base(discussionService)
    {
        _discussionService = discussionService;
        _accountProvider = accountProvider;
        _context = context;
    }

    public override IActionResult Save(DiscussionModel model)
    {
        model.UserId = _accountProvider.GetAccountId();

        foreach(var topicId in model.TopicsIds)
        {
            model.Topics.Add(_context.Topics.Where(x => x.Id == topicId).First());
        }
        return base.Save(model);
    }

    [HttpPost("ToggleVote/{id}")]
    public IActionResult ToggleVote(long id)
    {
        return Ok(_discussionService.ToggleVote(id, _accountProvider.GetAccountId()));
    }

    [HttpPost("Report/{id}")]
    public IActionResult Report(long id, ReportDTO data)
    {
        var userId = _accountProvider.GetAccountId();
        return Ok(_discussionService.Report(data, userId));
    }

    [HttpPost("Hide/{id}")]
    public IActionResult Hide(long id)
    {
        var userId = _accountProvider.GetAccountId();
        return Ok(_discussionService.Hide(id, userId));
    }

    [HttpGet("Daily")]
    public IActionResult Daily()
    {
        return Ok(_discussionService.GetDaily());
    }

    [HttpGet("Futures")]
    public IActionResult Futures()
    {
        return Ok(_discussionService.GetFutures());
    }
}
