using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DidacticVerse.Helpers;

public class PointLatLongConverter : JsonConverter<Point>
{
    public override void WriteJson(JsonWriter writer, Point? value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("latitude");
        writer.WriteValue(value?.Y);
        writer.WritePropertyName("longitude");
        writer.WriteValue(value?.X);
        writer.WriteEndObject();
        //writer.WriteStringValue($"{{latitude:{value.Y},longitude:{value.X}}}");
    }

    public override Point? ReadJson(JsonReader reader, Type objectType, Point? value, bool noIdea, JsonSerializer serializer)
    {
        var pointString = serializer.Deserialize<Dictionary<string, double>>(reader);
        if (pointString == null)
        {
            return null;
        }
        return GeometryHelper.FromLatLng(
            pointString.FirstOrDefault(x => x.Key == "latitude").Value,
            pointString.FirstOrDefault(x => x.Key == "longitude").Value
        );
    }
}
