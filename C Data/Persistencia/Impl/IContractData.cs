using Data.Model;

namespace Data.Persistencia.Impl;

public interface IContractData
{
    public Task<Contract> GetById(int id);
    public Task<List<Contract>> GetByTypeRate(Contract contract);
    public Task<List<Contract>> GetByTypePenaltyRate(Contract contract);
    public Task<Contract> GetByClientId(int clientId);
    public Task<List<Contract>> GetAll();
    public Task<Contract> Create(Contract contract);
    public Task<bool> Update(Contract contract, int id);
    public Task<bool> Delete(int id);
}