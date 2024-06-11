using Data.Model;
using Data.Persistencia.Impl;
using Domain.Service.Impl;
using System.Linq;

namespace Domain.Service;

public class LoanDomain : ILoanDomain
{
    private readonly ILoanData _loanData;
    private readonly IClientDomain _clientDomain;
    private readonly IContractDomain _contractDomain;

    public LoanDomain(ILoanData loanData, IClientDomain clientDomain, IContractDomain contractDomain)
    {
        _loanData = loanData;
        _clientDomain = clientDomain;
        _contractDomain = contractDomain;
    }
    public async Task<Loan> GetById(int id)
    {
        var result = await _loanData.GetById(id);
        if (result == null)
            throw new KeyNotFoundException("Loan not found");

        return result;
    }

    public async Task<List<Loan>> GetByTotalInstallments(Loan loan)
    {
        var result = await _loanData.GetByTotalInstallments(loan);
        if (result == null)
            throw new KeyNotFoundException("Dni not found");

        return result;
    }
    

    public async Task<IntermediateClass.LoanDetails> GetLoanDetailsByDni(string dni)
    {
        var client = await _clientDomain.GetByDni(dni);
        if (client == null)
        {
            throw new KeyNotFoundException("Cliente no encontrado.");
        }

        var contract = await _contractDomain.GetByClientId(client.Id);
        if (contract == null)
        {
            throw new KeyNotFoundException("Contrato no encontrado.");
        }
        var loanDetails = new IntermediateClass.LoanDetails
        {
            FirstName = client.FirstName,
            LastName = client.LastName,
            TypeRate = contract.TypeRate,
            Rate = contract.Rate,
            Period = contract.Period,
            PaymentDay = contract.PaymentDay,
            Tem  = contract.Tem,
            CreditLimit = client.CreditLimit
        };
        return loanDetails;
    }


    public async Task<List<Loan>> GetAll()
    {
        return await _loanData.GetAll();
    }

    public async Task<Loan> Create(Loan loan)
    {
        try
        {
            return await _loanData.Create(loan);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create loan", ex);
        }
    }

    public async Task<bool> Update(Loan loan, int id)
    {
        try
        {
            var existingLoan = await _loanData.GetById(id);
            if (existingLoan != null)
            {
                return await _loanData.Update(loan, id);
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var existingLoan = await _loanData.GetById(id);
            if (existingLoan != null)
            {
                return await _loanData.Delete(id);
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}