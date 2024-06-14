using EfVueMantle;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidacticVerse.Models;

[EfVueSource(typeof(AccountModel))]
[EfVueEndpoint("SaveProfile")]
public class AccountProfileDTO: DataTransferObjectBase
{
    //TODO dto's shouldn't require an id
    public long? Id { get; set; }
    public string DisplayName { get; set; }
    public string? Description { get; set; }
    public List<long> DiscussionTopicsIds { get; set; } = new();
}
