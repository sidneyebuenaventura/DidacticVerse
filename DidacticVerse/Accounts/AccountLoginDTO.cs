using EfVueMantle;

namespace DidacticVerse.Models;

[EfVueSource(typeof(AccountModel))]
public class AccountLoginDTO: DataTransferObjectBase
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }
}
