using Bogus.DataSets;
using DidacticVerse.Accounts;
using DidacticVerse.AWS;
using DidacticVerse.Models;
using EfVueMantle;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;

namespace DidacticVerse.Services;

public class AccountService : ServiceBase<AccountModel>
{
    private readonly DidacticVerseContext _context;
    private readonly DbSet<AccountModel> _accounts;
    private readonly IConfiguration _configuration;
    private readonly AccountProvider _accountProvider;
    private readonly Secrets _secrets;
    public AccountService(
        DidacticVerseContext context, 
        IConfiguration configuration, 
        AccountProvider accountProvider,
        Secrets secrets) : base(context.Accounts, context)
    {
        _context = context;
        _accounts = context.Accounts;
        _configuration = configuration;
        _accountProvider = accountProvider;
        _secrets = secrets;
    }

    public override AccountModel Save(AccountModel data)
    {
        var record = _accounts.FirstOrDefault(x => x.Id == _accountProvider.GetAccountId());
        if (record == null) throw new UnauthorizedAccessException("No account found.");
        data.DiscussionTopics = JsonConvert.SerializeObject(data.DiscussionTopicsIds);
        _context.Entry(record).CurrentValues.SetValues(data);
        _context.SaveChanges();

        return data;
    }
    public AccountModel Create(AccountModel data, string password)
    {
        data.TermsConditionsDate = DateTime.Now;
        _accounts.Add(data);
        //first save so we get our new Id for hashing
        _context.SaveChanges();

        var hasher = new PasswordHasher<IdentityUser>();
        IdentityUser identityUser = new IdentityUser(data.Id.ToString());
        data.HashedPassword = hasher.HashPassword(identityUser, password);
        //second save to store hashed password
        _context.SaveChanges();

        return data;
    }

    public bool Signup(SignupModel signup)
    {
        try
        {
            _context.Signups.Add(signup);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            return true;
        }
        return true;
    }


    public AccountModel AuthenticateGoogle(GoogleJsonWebSignature.Payload payload)
    {
        var account = _accounts.Where(x => x.Subject == payload.Subject).FirstOrDefault();
        if (account != null)
        {
            //Login existing user
            account.Token = IssueAuthToken(account.Id);
            account.RefreshToken = IssueRefreshToken(account.Id);
            return account;
        }

        //Add new user
        //TODO confirm email is present, etc
        var newAccount = new AccountModel()
        {
            EmailAddress = payload.Email,
            FirstName = payload.GivenName.Split(" ").FirstOrDefault() ?? string.Empty,
            LastName = payload.FamilyName.Split(" ").FirstOrDefault() ?? string.Empty,
            Birthdate = null,
            Subject = payload.Subject,
            DisplayName = payload.Name,
            Avatar = new ImageModel()
            {
                Key = string.Empty,
                Url = payload.Picture,
            },
        };
        _accounts.Add(newAccount);

        _context.SaveChanges();
        newAccount.Token = IssueAuthToken(newAccount.Id);
        newAccount.RefreshToken = IssueRefreshToken(newAccount.Id);

        return newAccount;
    }

    public AccountModel? Login(AccountLoginDTO login)
    {

        var account = _accounts.Where(
            x => 
                x.EmailAddress == login.EmailAddress
                && x.Subject == null
            ).FirstOrDefault();
        if (account == null || string.IsNullOrEmpty(login.Password))
        {
            return null;
        }

        var hasher = new PasswordHasher<IdentityUser>();
        IdentityUser identityUser = new IdentityUser(account.Id.ToString());

        if (PasswordVerificationResult.Failed == hasher.VerifyHashedPassword(identityUser, account.HashedPassword, login.Password))
        {
            return null;
        }

        account.Token = IssueAuthToken(account.Id);
        account.RefreshToken = IssueRefreshToken(account.Id);
        return account;
    }

    public AccountModel? Get()
    {
        return _accounts.Where(x => x.Id == _accountProvider.GetAccountId()).FirstOrDefault();
    }
    public long Update()
    {
        return _context.SaveChanges();
    }

    public string IssueAuthToken(long accountId)
    {
        var tokenDescriptor = new AccountIdentity(_configuration).TokenDescriptor(accountId);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


    public AccountModel SaveDetail(AccountDetailDTO accountDetail)
    {
        var accountId = _accountProvider.GetAccountId();
        var account = _accounts.Where(x => x.Id == accountId).FirstOrDefault();
        if (account == null) throw new UnauthorizedAccessException("No account found.");

        //TODO validation?
        account.Birthdate = accountDetail.Birthdate;
        account.FirstName = accountDetail.FirstName;
        account.LastName = accountDetail.LastName;
        account.EmailAddress = accountDetail.EmailAddress;
        account.TermsConditions = accountDetail.TermsConditions;
        account.TermsConditionsDate = DateTime.Now;
        _context.SaveChanges();

        return account;
    }
    public AccountModel SaveAccountProfile(AccountProfileDTO accountComplete)
    {
        var accountId = _accountProvider.GetAccountId();
        var account = _accounts.Where(x => x.Id == accountId).FirstOrDefault();
        if (account == null) throw new UnauthorizedAccessException("No account found.");

        account.DisplayName = accountComplete.DisplayName;
        account.Description = accountComplete.Description;
        account.DiscussionTopics = JsonConvert.SerializeObject(accountComplete.DiscussionTopicsIds) ?? string.Empty;
        _context.SaveChanges();

        return account;
    }



    public class SmtpCredentials
    {
        public string User { get; set; }
        public string Password { get; set; }
    }

    public async Task<bool> SendResetEmail(string email)
    {
        if (email == null) throw new ArgumentNullException("email");
        var account = _accounts.FirstOrDefault(x => x.EmailAddress == email && x.Subject == null);
        if (account == null) return false;

        var hasher = new PasswordHasher<IdentityUser>();
        IdentityUser identityUser = new IdentityUser(account.Id.ToString());

        account.ResetToken = Guid.NewGuid().ToString();
        account.ResetTime = DateTime.Now.AddHours(1);
        _context.SaveChanges();

        var creds = _secrets.Get<SmtpCredentials>("ses/smtp");

        var smtpClient = new SmtpClient("email-smtp.us-east-2.amazonaws.com")
        {
            Port = 25,
            Credentials = new NetworkCredential(creds.User, creds.Password),
            EnableSsl = true,
        };
        smtpClient.Send("accounts@connect-her.org", account.EmailAddress, "Password reset request", $"Click or copy this URL to reset your password: https://connect-her.org/password-reset/{account.ResetToken} \r\n\r\nThis link expires in one hour.");

        return true;
    }

    public bool ValidateEmailResetToken(string token)
    {
        var account = _accounts.FirstOrDefault(x => x.ResetToken == token && x.ResetTime > DateTime.Now);
        return account != null;
    }

    public bool PasswordReset(string token, string password)
    {
        var account = _accounts.FirstOrDefault(x => x.ResetToken == token && x.ResetTime > DateTime.Now);
        if (account == null)
        {
            return false;
        }

        var hasher = new PasswordHasher<IdentityUser>();
        IdentityUser identityUser = new IdentityUser(account.Id.ToString());
        account.HashedPassword = hasher.HashPassword(identityUser, password);
        _context.SaveChanges();

        return true;

    }


    public string IssueRefreshToken(long accountId)
    {

        var refresh = new RefreshTokens()
        {
            Id = Guid.NewGuid(),
            AccountId = accountId
        };

        var tokenDescriptor = new AccountIdentity(_configuration).ResetTokenDescriptor(refresh);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);


        refresh.Token = tokenHandler.WriteToken(token);

        _context.Refreshes.Add(refresh);
        _context.SaveChanges();

        return refresh.Token;
    }
    public bool ValidateRefreshToken(string token, JwtSecurityToken jsonToken, HttpContext httpContext)
    {
        
        long? accountId;
        Guid? id;
        try
        {
            accountId = long.Parse(jsonToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value);
            id = new Guid(jsonToken.Claims.First(x => x.Type == "Id").Value.ToUpper());
        } catch (Exception)
        {
            return false;
        }

        if (jsonToken.ValidTo < DateTime.Now) return false;

        var refresh = _context.Refreshes.FirstOrDefault(x =>
            x.Id == id
            && x.AccountId == accountId
            && x.Invalidated == false
        );

        if (refresh == null) return false;
        
        if (refresh.Consumed)
        {
            refresh.Reconsumption = $"{httpContext?.Connection?.RemoteIpAddress?.ToString()} | {JsonConvert.SerializeObject(httpContext?.Request.Headers)}";
            refresh.ReconsumptionTime = DateTime.Now;

            _context.Refreshes.Where(x => x.AccountId == accountId).ToList().ForEach(x => 
            {
                x.Invalidated = true;
            });

            _context.SaveChanges();
            return false;
        }

        refresh.Consumed = true;
        _context.SaveChanges();

        return true;
    }



    public bool Hide(long hiddenUserId)
    {
        var hide = new UserProfileHiddenModel()
        {
            HiddenUserId = hiddenUserId,
            UserId = _accountProvider.GetAccountId(),
        };
        _context.UserProfileHides.Add(hide);
        _context.SaveChanges();
        return true;
    }

    public List<UserProfileHiddenModel> GetHidden()
    {
        return _context.UserProfileHides
            .Where(x => x.UserId == _accountProvider.GetAccountId())
            .Include(x => x.HiddenUser).ToList();
    }

    public bool Unhide(long hiddenUserId)
    {
        var hide = _context.UserProfileHides.FirstOrDefault(x => x.HiddenUserId == hiddenUserId && x.UserId == _accountProvider.GetAccountId());
        if (hide != null)
        {
            _context.UserProfileHides.Remove(hide);
            _context.SaveChanges();
        }
        return true;
    }
}
