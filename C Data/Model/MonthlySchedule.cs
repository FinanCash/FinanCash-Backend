using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Model;

public class MonthlySchedule : Base
{
    public int Month { get; set; }
    public int Year { get; set; }
    private string _status = "Pendiente"; // Estado predeterminado
    // La propiedad Status se puede ver, pero no se puede modificar desde la vista
    [Display(Name = "Estado")]
    [NotMapped]
    public string Status
    {
        get { return _status; }
        set { _status = value; } 
    }
    public int ClientId { get; set; }
    [ForeignKey("ClientId")]
    public Client Client { get; set; }
    public int ScheduleId { get; set; }
    [ForeignKey("ScheduleId")]
    public Schedule Schedule { get; set; }
}