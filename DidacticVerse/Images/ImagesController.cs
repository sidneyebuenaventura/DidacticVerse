using DidacticVerse.Models;
using DidacticVerse.Services;
using EfVueMantle;
using Microsoft.AspNetCore.Authorization;

namespace DidacticVerse.Controllers;

[Authorize]
public class ImageController : ControllerBase<ImageModel, ImageService>
{
    public ImageService _imageService;
    public ImageController(ImageService imageService) : base(imageService)
    {
        _imageService = imageService;
    }
}
