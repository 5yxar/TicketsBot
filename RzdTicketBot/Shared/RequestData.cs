namespace ConsoleApp3.Shared;

public class RequestData
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureDate { get; set; }
    public int TimeFrom { get; set; }
    public int TimeTo { get; set; }
    public string CarGrouping { get; set; }
    public bool GetByLocalTime { get; set; }
    public string SpecialPlacesDemand { get; set; }
}