using System.Text.Json.Serialization;

namespace Data.Model;

public class User : Base
{
    public string Email { get; set; }
    public string Password { get; set; }
    [JsonIgnore]
    public List<Client> Clients { get; set; }
}