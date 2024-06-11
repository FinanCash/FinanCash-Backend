namespace UniqueTrip.Request;

public class MonthlyScheduleRequest
{
    public int Month { get; set; }
    public int Year { get; set; }
    public int ClientId { get; set; }
    public int ScheduleId { get; set; }
}