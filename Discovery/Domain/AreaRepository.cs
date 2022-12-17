namespace Discovery.Domain;

public class AreaRepository
{
    private static readonly List<Area> Areas = new();

    public Area? Get(int id)
    {
        return Areas.FirstOrDefault(r => r.Id == id);
    }

    public Area Add(Area area)
    {
        var maxAreaId = Areas.Any() ? Areas.Max(r => r.Id) : 0;
        var newArea = area with {Id = maxAreaId + 1};
        Areas.Add(newArea);
        return newArea;
    }
}