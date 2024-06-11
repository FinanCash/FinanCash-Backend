using Data.Model;

namespace Domain.Service;

public class FinancialUtils
{
    public double CalculateDefaultInterest(double rate, DateTime paymentDay)//Se le pasa el tem
    {
        int capitalizationFrequency = 1;
        int timeToConvert = 30;
        int td = (int)(DateTime.Now - paymentDay).TotalDays;
        int n1 = timeToConvert/capitalizationFrequency;
        double tem = Math.Pow(1 + rate, (double) td/ n1) - 1;
        return tem;
    }
    public double NominalRateToTem(double rate, string period)
    {
        int capitalizationFrequency = 1;
        int timeToConvert = 30;
        int m = GetFrequencyPeriod(period)/capitalizationFrequency;
        int n = timeToConvert/capitalizationFrequency;
        double tem = Math.Pow((1 + rate / m), n) - 1;
        return tem;
    }
    public double EffectiveRateToTem(double rate, string period)
    {
        int capitalizationFrequency = 1;
        int timeToConvert = 30;
        int n1 = GetFrequencyPeriod(period)/capitalizationFrequency;
        int n2 = timeToConvert/capitalizationFrequency;
        double tem = Math.Pow(1 + rate, (double)n2 / n1) - 1;
        return tem;
    }
    
    public double CalculateTEM(string typeRate, double rate, string period)
    {
        if (typeRate.ToLower() == "nominal")
        {
            return NominalRateToTem(rate, period);
        }
        else if (typeRate.ToLower() == "efectiva")
        {
            return EffectiveRateToTem(rate, period);
        }
        else
        {
            throw new ArgumentException("Invalid type of rate");
        }
    }
    public int GetFrequencyPeriod(string period)
    {
        switch (period.ToLower())
        {
            case "mensual":
                return 30;
            case "bimestral":
                return 60;
            case "trimestral":
                return 90;
            case "cuatrimestral":
                return 120;
            case "semestral":
                return 180;
            case "anual":
                return 360;
            default:
                throw new ArgumentException("Invalid period");
        }
    }
    public double CalculateFutureValue(double amount, double tem, DateTime PaymentDay)
    {
        int nDiasTrasladar = (int)(PaymentDay - DateTime.Now).TotalDays;
        int nDiasTem = 30;
        double futureValue = amount * Math.Pow(1 + tem, (double)nDiasTrasladar / nDiasTem);
        return futureValue;
    }

    public List<IntermediateClass.LoanPayment> CalculateFrenchInstallment(double amount, double tem, int totalInstallments)
    {
        List<IntermediateClass.LoanPayment> payments = new List<IntermediateClass.LoanPayment>();
        double cI = amount;//Monto Capital
        int nDxM = 30;
        double nCxM = (double)nDxM / nDxM;
        double tep = tem;
        for (int i = 1; i <= totalInstallments; i++)
        {
            IntermediateClass.LoanPayment payment = new IntermediateClass.LoanPayment();
            payment.NCuotas = i;
            payment.Tem = tep;
            payment.CI = cI;

            if (payments.Count > 0)
            {
                payment.SI = payments[i-1].SF;
            }
            else
            {
                payment.SI = cI;
            }

            payment.I = tep * payment.SI;
            payment.R = cI * (tep * Math.Pow((1 + tep), nCxM)) / (Math.Pow((1 + tep), nCxM) - 1);
            payment.A = payment.R - payment.I;
            payment.SF = payment.SI - payment.A;

            payments.Add(payment);

            cI = payment.SF;
        }

        return payments;
    }
    
    

    }