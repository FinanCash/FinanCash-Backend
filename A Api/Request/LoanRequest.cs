namespace UniqueTrip.Request;

public class LoanRequest
{
    public double Amount { get; set; }
    public int TotalInstallments { get; set; }
    public int ClientId { get; set; }
    public int ContractId { get; set; }
}