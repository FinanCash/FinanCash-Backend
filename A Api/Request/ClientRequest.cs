using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace UniqueTrip.Request;

public class ClientRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Dni { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public double CreditLimit { get; set; }
    [Column(TypeName = "Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime BirthDate { get; set; }
    public int UserId { get; set; }
}

