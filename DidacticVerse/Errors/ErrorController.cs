using Microsoft.AspNetCore.Mvc;

namespace DidacticVerse.Errors;

public class ErrorController : Controller
{
    [HttpGet("/Error")]
    public IActionResult HandleError()
    {
        return Problem();
    }
}
