using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Model;

public class Loan : Base
{
    public double Amount { get; set; }
    public int TotalInstallments { get; set; }
    public int ClientId { get; set; }
    [ForeignKey("ClientId")]
    public Client Client { get; set; }
    public int ContractId { get; set; }
    [ForeignKey("ContractId")]
    public Contract Contract { get; set; }
    [JsonIgnore]
    public Schedule Schedule { get; set; }
}