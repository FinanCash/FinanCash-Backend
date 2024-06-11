using Data.Model;

namespace Domain.Service.Impl;

public interface IScheduleDomain
{
    public Task<Schedule> GetById(int id);
    public Task<List<IntermediateClass.LoanPayment>> CalculateAndCreateFrenchInstallments(int contractId, int loanId);
    public Task<List<Schedule>> GetAll();

    public Task<bool> Update(Schedule schedule, int id);
    public Task<bool> Delete(int id);
}