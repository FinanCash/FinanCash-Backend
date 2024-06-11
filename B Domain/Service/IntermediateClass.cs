
namespace Domain.Service;

public class IntermediateClass
{
    public class DebtView
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientDNI { get; set; }
        public DateTime DueDate { get; set; }
        public double TotalAmount { get; set; }
        public int ContractId { get; set; }
        public int ClientId { get; set; }
        public int MonthlyScheduleId { get; set; }
    }
    
    public class LoanPayment
    {
        public int NCuotas { get; set; }
        public double Tem { get; set; }
        public double CI { get; set; }//Cuota inicial
        public double SI { get; set; }//Saldo Inicial
        public double I { get; set; }//Interes
        public double R { get; set; }//Cuota
        public double A { get; set; }//Amortizacion
        public double SF { get; set; } //Saldo Final
    }
    public class LoanDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TypeRate { get; set; }
        public double Rate { get; set; }
        public string Period { get; set; }
        public double Tem { get; set; }
        public double CreditLimit { get; set; }
        public DateTime PaymentDay { get; set; }
    }
}