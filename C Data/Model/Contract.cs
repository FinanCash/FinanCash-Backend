using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Model;

public class Contract : Base
{
    public string TypeRate { get; set; }
    public double Rate { get; set; }
    public string Period { get; set; }
    public string TypePenaltyRate { get; set; }
    public double PenaltyRate { get; set; }
    public string PenaltyPeriod { get; set; }
    public DateTime PaymentDay { get; set; }
    [NotMapped]
    public double Tem { get; set; } 
    public int ClientId { get; set; }
    [ForeignKey("ClientId")]
    public Client Client { get; set; }
    [JsonIgnore]
    public List<Loan> Loans { get; set; }
}