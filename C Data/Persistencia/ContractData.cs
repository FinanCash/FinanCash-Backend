using Data.Context;
using Data.Model;
using Data.Persistencia.Impl;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistencia;

public class ContractData : IContractData
{
    private readonly AppDbContext _appDbContext;
    
    public ContractData(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Contract> GetById(int id)
    {
        return await _appDbContext.Contracts.Where(ir => ir.Id == id && ir.IsActive == true).FirstOrDefaultAsync();    }

    public async Task<List<Contract>> GetByTypeRate(Contract contract)
    {
        return await _appDbContext.Contracts.Where(ir => ir.TypeRate.Contains(contract.TypeRate)
                                                                   && ir.IsActive == true).ToListAsync();
    }

    public async Task<List<Contract>> GetByTypePenaltyRate(Contract contract)
    {
        return await _appDbContext.Contracts.Where(ir => ir.TypePenaltyRate.Contains(contract.TypePenaltyRate)
                                                         && ir.IsActive == true).ToListAsync();
    }

    public async Task<Contract> GetByClientId(int clientId)
    {
        return await _appDbContext.Contracts.Where(ir => ir.ClientId == clientId && ir.IsActive == true).FirstOrDefaultAsync();    
    }
    
    public async Task<List<Contract>> GetAll()
    {
        return await _appDbContext.Contracts.Where(ir => ir.IsActive == true).ToListAsync();

    }

    public async Task<Contract> Create(Contract contract)
    {
        try
        {
            var client = await _appDbContext.Clients.FirstOrDefaultAsync(u => u.Id == contract.ClientId && u.IsActive == true);
            if (client == null)
            {
                throw new Exception("El cliente referenciado no está activo o no existe.");
            }
            _appDbContext.Contracts.Add(contract);
            await _appDbContext.SaveChangesAsync();

            return contract;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> Update(Contract contract, int id)
    {
        try
        {
            
            var contractUpdated = _appDbContext.Contracts.Where(ir => ir.Id == id).FirstOrDefault();

            if (contractUpdated != null)
            {
                contractUpdated.TypeRate = contract.TypeRate;
                contractUpdated.Rate = contract.Rate;
                contractUpdated.Period = contract.Period;
                contractUpdated.TypePenaltyRate = contract.TypePenaltyRate;
                contractUpdated.PenaltyRate = contract.PenaltyRate;
                contractUpdated.PenaltyPeriod = contract.PenaltyPeriod;
                contractUpdated.PaymentDay = contract.PaymentDay;
                contractUpdated.DateUpdated = DateTime.Now;

                _appDbContext.Contracts.Update(contractUpdated);
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
            var contractDeleted = _appDbContext.Contracts.Where(ir => ir.Id == id).FirstOrDefault();

            if (contractDeleted != null)
            {
                contractDeleted.IsActive = false;
                contractDeleted.DateUpdated = DateTime.Now;
                _appDbContext.Contracts.Update(contractDeleted);
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
