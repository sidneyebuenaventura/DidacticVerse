using DidacticVerse.Accounts;
using DidacticVerse.Models;
using DidacticVerse.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DidacticVerse.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserProfileController : ControllerBase
{
    public AccountService _accountService;
    public AccountProvider _accountProvider;

    public UserProfileController(
        AccountService accountService,
        AccountProvider accountProvider)
    {
        _accountService = accountService;
        _accountProvider = accountProvider;
    }

    [HttpPost("List")]
    public IActionResult GetList(List<long> ids)
    {
        var accountList = _accountService.GetList(ids);
        var userList = accountList.Select(x => new UserProfileModel(x)).ToList();
        return Ok(userList);
    }

    [HttpPost("Hide/{id}")]
    public IActionResult Hide(long id)
    {
        return Ok(_accountService.Hide(id));
    }

    [HttpGet("GetHidden")]
    public IActionResult GetHidden()
    {
        return Ok(_accountService.GetHidden());
    }

    [HttpPost("Unhide/{id}")]
    public IActionResult Unhide(long id)
    {
        return Ok(_accountService.Unhide(id));
    }
}
