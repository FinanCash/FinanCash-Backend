namespace UniqueTrip.Response;

public class ScheduleResponse
{
    public int Id { get; set; }
    public int InstallmentNumber { get; set; }
    public double Amortization { get; set; }
    public double Interest { get; set; }
    public double InstallmentAmount { get; set; }
    public DateTime DueDate { get; set; }
    public int ClientId { get; set; }
    public ClientResponse Client { get; set; }
    public int LoanId { get; set; }
    public LoanResponse Loan { get; set; }
    public DateTime DateCreated { get; set; }
}