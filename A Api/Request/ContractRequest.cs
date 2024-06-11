using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqueTrip.Request;

public class ContractRequest
{
    public string TypeRate { get; set; }
    public double Rate { get; set; }
    public string Period { get; set; }
    public string TypePenaltyRate { get; set; }
    public double PenaltyRate { get; set; }
    public string PenaltyPeriod { get; set; }
    [Column(TypeName = "Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime PaymentDay { get; set; }
    public int ClientId { get; set; }
}