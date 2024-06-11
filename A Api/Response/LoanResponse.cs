namespace UniqueTrip.Response;

public class LoanResponse
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public int TotalInstallments { get; set; }
    public int ClientId { get; set; }
    public ClientResponse Client { get; set; }
    public int ContractId { get; set; }
    public ContractResponse Contract { get; set; }
    public DateTime DateCreated { get; set; }
}