using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Model;

public class Schedule : Base
{
    public int InstallmentNumber { get; set; }
    public double Amortization { get; set; }
    public double Interest { get; set; }
    public double InstallmentAmount { get; set; }
    public DateTime DueDate { get; set; }
    public int ClientId { get; set; }
    [ForeignKey("ClientId")]
    public Client Client { get; set; }
    public int LoanId { get; set; }
    [ForeignKey("LoanId")]
    public Loan Loan { get; set; }
    [JsonIgnore]
    public List<MonthlySchedule> MonthlySchedules { get; set; }




}