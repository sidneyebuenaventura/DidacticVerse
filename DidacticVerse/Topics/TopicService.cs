using DidacticVerse.Models;
using EfVueMantle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DidacticVerse.Services;

[AllowAnonymous]
public class TopicService : ServiceBase<TopicModel>
{
    public TopicService(DidacticVerseContext context) : base(context.Topics, context)
    {
    }
}
