using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace DidacticVerse.Helpers;

public static class GeometryHelper
{
    public static GeometryFactory GeometryFactory { get; set; }
        = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
    public static Point FromLatLng(double latitude, double longitude)
    {
        return GeometryFactory.CreatePoint(new Coordinate(longitude, latitude));
    }
}
