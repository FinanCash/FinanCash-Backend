using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqueTrip.Request;

public class ScheduleRequest
{
    public int InstallmentNumber { get; set; }
    public double Amortization { get; set; }
    public double Interest { get; set; }
    public double InstallmentAmount { get; set; }
    [Column(TypeName = "Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DueDate { get; set; }
    public int ClientId { get; set; }
    public int LoanId { get; set; }
}