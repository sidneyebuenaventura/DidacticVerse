using EfVueMantle;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidacticVerse.Models;

[EfVueSource(typeof(AccountModel))]
[EfVueEndpoint("Create")]
public class AccountCreateDTO: DataTransferObjectBase
{
    public long? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public DateTime Birthdate { get; set; }
    public string Password { get; set; }
    public bool TermsConditions { get; set; }
}
