using DidacticVerse.Models;
using EfVueMantle;
using Microsoft.EntityFrameworkCore;

namespace DidacticVerse.Services;

public class ImageService : ServiceBase<ImageModel>
{
    private readonly DidacticVerseContext _context;
    private readonly DbSet<ImageModel> _images;

    public ImageService(DidacticVerseContext context) : base(context.Images, context)
    {
        _context = context;
        _images = context.Images;
    }
}
