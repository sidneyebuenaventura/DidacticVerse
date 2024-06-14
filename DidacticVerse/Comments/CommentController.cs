using DidacticVerse.Accounts;
using DidacticVerse.Models;
using DidacticVerse.Services;
using EfVueMantle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DidacticVerse.Controllers;

[Authorize]
public class CommentController : ControllerBase<CommentModel, CommentService>
{
    public CommentService _commentService;
    public AccountProvider _accountProvider;
    public DidacticVerseContext _context;
    public CommentController(
        CommentService commentService, 
        AccountProvider accountProvider,
        DidacticVerseContext context
        ) : base(commentService)
    {
        _commentService = commentService;
        _accountProvider = accountProvider;
        _context = context;
    }

    public override IActionResult Save(CommentModel model)
    {
        model.UserId = _accountProvider.GetAccountId();
        return base.Save(model);
    }

    [HttpPost("Report/{id}")]
    public IActionResult Report(int id, ReportDTO data)
    {
        var userId = _accountProvider.GetAccountId();
        return Ok(_commentService.Report(data, userId));
    }

    [HttpPost("Hide/{id}")]
    public IActionResult Hide(int id)
    {
        var userId =_accountProvider.GetAccountId();
        return Ok(_commentService.Hide(id, userId));
    }
}
