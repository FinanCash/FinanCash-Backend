using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Model;

public class Client : Base
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Dni { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public double CreditLimit { get; set; }
    [Column(TypeName = "Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime BirthDate { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    [JsonIgnore]
    public List<Loan> Loans { get; set; }
    [JsonIgnore]
    public Contract Contract { get; set; }
    [JsonIgnore]
    public List<Schedule> Schedules { get; set; }
    [JsonIgnore]
    public List<MonthlySchedule> MonthlySchedules { get; set; }

}