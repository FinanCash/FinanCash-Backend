namespace UniqueTrip.Response;

public class ContractResponse
{
    public int Id { get; set; }
    public string TypeRate { get; set; }
    public double Rate { get; set; }
    public string Period { get; set; }
    public string TypePenaltyRate { get; set; }
    public double PenaltyRate { get; set; }
    public string PenaltyPeriod { get; set; }
    public DateTime PaymentDay { get; set; }
    public double Tem { get; set; }
    public int ClientId { get; set; }
    public ClientResponse Client { get; set; }
    public DateTime DateCreated { get; set; }
}