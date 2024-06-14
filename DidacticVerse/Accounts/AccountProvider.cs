using DidacticVerse.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace DidacticVerse.Accounts;

public class AccountProvider
{
    private IHttpContextAccessor _accessor;
    private long? _accountId { get; set; } = null;

    public AccountProvider(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public long GetAccountId()
    {
        if (_accountId != null) return (long)_accountId;
        var identity = _accessor?.HttpContext?.User.Identity;

        if (identity != null && identity.IsAuthenticated)
        {
            //cache account id for this request
            long accountId;
            if(long.TryParse(_accessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Name)?.Value, out accountId))
            {
                _accountId = accountId;
                return (long)_accountId;
            }
        }
        throw new UnauthorizedAccessException();
    }
}

