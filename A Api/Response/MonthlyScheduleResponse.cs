namespace UniqueTrip.Response;

public class MonthlyScheduleResponse
{
    public int Id { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string Status { get; set; }
    public int ClientId { get; set; }
    public ClientResponse Client { get; set; }
    public int ScheduleId { get; set; }
    public ScheduleResponse Schedule { get; set; }
    public DateTime DateCreated { get; set; }
}