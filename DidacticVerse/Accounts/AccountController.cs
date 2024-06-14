using DidacticVerse.Models;
using DidacticVerse.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using Google.Apis.Auth;
using System.Security.Principal;
using Microsoft.Extensions.Options;
using DidacticVerse.AWS;
using DidacticVerse.Accounts;
using RTools_NTS.Util;
using System.IdentityModel.Tokens.Jwt;
using MailKit;
using Newtonsoft.Json;

namespace DidacticVerse.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]")]
public class AccountController: ControllerBase
{
    public AccountService _accountsService;
    private readonly GoogleConfig _credentials;
    public AccountController(AccountService accountsService, IOptions<GoogleConfig> options)
    {
        _accountsService = accountsService;
        _credentials = options.Value;

    }



    public class SendResetEmailPayload
    {
        public string Email { get; set; }
    }
    [HttpPost("ForgotPassword")]
    public IActionResult ForgotPassword(SendResetEmailPayload data)
    {
        return Ok(_accountsService.SendResetEmail(data.Email));
    }

    public class ValidateResetEmailPayload
    {
        public string Token { get; set; }
    }
    [HttpPost("ValidateEmailResetToken")]
    public IActionResult ValidateEmailResetToken (ValidateResetEmailPayload data)
    {
        return Ok(_accountsService.ValidateEmailResetToken(data.Token));
    }

    public class ResetPasswordPayload
    {
        public string Token { get; set; }
        public string Password { get; set; }
    }
    [HttpPost("ResetPassword")]
    public IActionResult ResetPassword(ResetPasswordPayload data)
    {
        return Ok(_accountsService.PasswordReset(data.Token, data.Password));
    }



    [HttpPost("Save")]
    public IActionResult Save(AccountModel account)
    {
        try
        {
            return Ok(_accountsService.Save(account));
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    [HttpPost("Create")]
    public IActionResult Create(AccountCreateDTO newAccount)
    {
        try
        {
            return Ok(_accountsService.Create(new AccountModel(newAccount), newAccount.Password));
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    /**
     * Confirms/sets account details for non-email registration
     */
    [Authorize]
    [HttpPost("SaveDetail")]
    public IActionResult SaveDetail(AccountDetailDTO accountDetail)
    {
        return Ok(_accountsService.SaveDetail(accountDetail));
    }

    [Authorize]
    [HttpPost("SaveProfile")]
    public IActionResult SaveProfile(AccountProfileDTO accountProfile)
    {
        return Ok(_accountsService.SaveAccountProfile(accountProfile));
    }

    [HttpPost("Login")]
    public IActionResult Login(AccountLoginDTO login)
    {
        var account = _accountsService.Login(login);
        if (account == null)
        {
            return Unauthorized("User name or password do not match");
        }
        
        return Ok(account);
    }


    [HttpPost("Signup")]
    public IActionResult Signup(SignupModel data) {
        return Ok(_accountsService.Signup(data));
    }

    public class GoogleConfirmPayload
    {
        public string Credentials { get; set; }
    }
    [HttpPost("GoogleConfirm")]
    public async Task<IActionResult> GoogleConfirm(GoogleConfirmPayload payload)
    {
        //TODO error handling for bad auth
        try
        {
            var validated = await GoogleJsonWebSignature.ValidateAsync(payload.Credentials);
            var account = _accountsService.AuthenticateGoogle(validated);
            return Ok(account);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }


    public class RefreshTokenPayload
    {
        public string RefreshToken { get; set; }
    }
    public class RefreshTokenResponse
    {
        public string RefreshToken { get; set; }
        public string Token { get; set; }
    }
    [HttpPost("RefreshToken")]
    public IActionResult RefreshToken(RefreshTokenPayload data)
    {
        var handler = new JwtSecurityTokenHandler();
        if (data.RefreshToken == null
            || handler.ReadToken(data.RefreshToken) is not JwtSecurityToken jsonToken)
        {
            throw new UnauthorizedAccessException();
        }

        var valid = _accountsService.ValidateRefreshToken(data.RefreshToken, jsonToken, HttpContext);

        if (!valid)
        {
            throw new UnauthorizedAccessException();
        }

        var accountId = long.Parse(jsonToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value);

        return Ok(new RefreshTokenResponse
        {
            RefreshToken = _accountsService.IssueRefreshToken(accountId),
            Token = _accountsService.IssueAuthToken(accountId)
        });
    }

    [HttpPost("List")]
    public virtual IActionResult GetList(List<long> ids)
    {
        List<AccountModel> list = _accountsService.GetList(ids);
        return Ok(list);
    }

    [HttpGet("Get")]
    public virtual IActionResult Get(long id)
    {
        return Ok(_accountsService.Get());
    }
}

