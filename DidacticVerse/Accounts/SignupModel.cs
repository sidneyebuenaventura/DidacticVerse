using EfVueMantle;
namespace DidacticVerse.Models;

[EfVueSource(typeof(AccountModel))]
[EfVueEndpoint("Signup")]
public class SignupModel: ModelBase
{
    public string EmailAddress { get; set; }
}