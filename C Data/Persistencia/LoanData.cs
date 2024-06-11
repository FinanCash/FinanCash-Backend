using Data.Context;
using Data.Model;
using Data.Persistencia.Impl;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistencia;

public class LoanData : ILoanData
{
    private readonly AppDbContext _appDbContext;
    
    public LoanData(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Loan> GetById(int id)
    {
        return await _appDbContext.Loans.Where(l => l.Id == id && l.IsActive == true).FirstOrDefaultAsync();    }

    public async Task<List<Loan>> GetByTotalInstallments(Loan loan)
    {
        return await _appDbContext.Loans.Where(l => l.TotalInstallments == loan.TotalInstallments
                                                    && l.IsActive == true).ToListAsync();
    }

    public async Task<List<Loan>> GetAll()
    {
        return await _appDbContext.Loans.Where(l => l.IsActive == true).ToListAsync();
    }

    public async Task<Loan> Create(Loan loan)
    {
        try
        {
            var client = await _appDbContext.Clients.FirstOrDefaultAsync(c => c.Id == loan.ClientId && c.IsActive == true);
            if (client == null)
            {
                throw new Exception("El cliente referenciado no está activo o no existe.");
            }
            
            var contract = await _appDbContext.Contracts.FirstOrDefaultAsync(c => c.Id == loan.ContractId && c.IsActive == true);
            if (contract == null)
            {
                throw new Exception("El contrato referenciado no está activo o no existe.");
            }
            
            _appDbContext.Loans.Add(loan);
            await _appDbContext.SaveChangesAsync();

            return loan;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> Update(Loan loan, int id)
    {
        try
        {
            var loanUpdated = _appDbContext.Loans.Where(l => l.Id == id).FirstOrDefault();

            if (loanUpdated != null)
            {
                loanUpdated.Amount = loan.Amount;
                loanUpdated.TotalInstallments = loan.TotalInstallments;
                loanUpdated.DateUpdated = DateTime.Now;

                _appDbContext.Loans.Update(loanUpdated);
            }

            await _appDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var loanDeleted = _appDbContext.Loans.Where(ir => ir.Id == id).FirstOrDefault();

            if (loanDeleted != null)
            {
                loanDeleted.IsActive = false;
                loanDeleted.DateUpdated = DateTime.Now;
                _appDbContext.Loans.Update(loanDeleted);
            }
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
