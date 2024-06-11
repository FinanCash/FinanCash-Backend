namespace UniqueTrip.Response;

public class ClientResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Dni { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public double CreditLimit { get; set; }
    public DateTime BirthDate { get; set; }
    public int UserId { get; set; }
    public UserResponse User { get; set; }  
    public DateTime DateCreated { get; set; }
}