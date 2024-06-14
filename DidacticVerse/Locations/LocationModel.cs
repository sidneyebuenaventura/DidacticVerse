using DidacticVerse.Helpers;
using EfVueMantle;
using Microsoft.AspNetCore.Authorization;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace DidacticVerse.Models;

[Authorize]
public class LocationModel : ModelBase
{
    public string Title { get; set; }

    [JsonConverter(typeof(PointLatLongConverter))]
    public Point Location { get; set; }
}
