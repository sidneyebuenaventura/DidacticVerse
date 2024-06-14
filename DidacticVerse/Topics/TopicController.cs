using DidacticVerse.Models;
using DidacticVerse.Services;
using EfVueMantle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DidacticVerse.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class TopicController : ControllerBase<TopicModel, TopicService>
{
    public TopicService _topicService;
    public TopicController(TopicService topicService) : base(topicService)
    {
        _topicService = topicService;
    }
}