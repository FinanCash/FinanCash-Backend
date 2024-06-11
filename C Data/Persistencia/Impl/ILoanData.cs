using Data.Model;

namespace Data.Persistencia.Impl;

public interface ILoanData
{
    public Task<Loan> GetById(int id);
    public Task<List<Loan>> GetByTotalInstallments(Loan loan);
    public Task<List<Loan>> GetAll();
    public Task<Loan> Create(Loan loan);
    public Task<bool> Update(Loan loan, int id);
    public Task<bool> Delete(int id);
}