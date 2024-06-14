using DidacticVerse.Helpers;
using EfVueMantle;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidacticVerse.Models;

public class ImageModel : ModelBase
{
    public string Url { get; set; }

    [JsonIgnore]
    public string Key { get; set; }
}
