using Data.Model;
using Data.Persistencia.Impl;
using Domain.Service.Impl;

namespace Domain.Service;

public class ContractDomain : IContractDomain
{
    private readonly IContractData _contractData;
    private readonly FinancialUtils _financialUtils;
    //private readonly IMonthlyScheduleDomain _monthlyScheduleDomain;

    public ContractDomain(IContractData contractData, FinancialUtils financialUtils)
    {
        _contractData = contractData;
        _financialUtils = financialUtils;
        //_monthlyScheduleDomain = monthlyScheduleDomain;
    }

    public async Task<Contract> GetById(int id)
    {
        var result = await _contractData.GetById(id);
        if (result == null)
            throw new KeyNotFoundException("Contract not found");

        return result;
    }

    public async Task<List<Contract>> GetByTypeRate(Contract contract)
    {
        var result = await _contractData.GetByTypeRate(contract);
        if (result == null)
            throw new KeyNotFoundException("TypeRate not found");

        return result;
    }

    public async Task<List<Contract>> GetByTypePenaltyRate(Contract contract)
    {
        var result = await _contractData.GetByTypePenaltyRate(contract);
        if (result == null)
            throw new KeyNotFoundException("TypePenaltyRate not found");

        return result;
    }

    public async Task<Contract> GetByClientId(int clientId)
    {
        var result = await _contractData.GetByClientId(clientId);
        if (result == null)
            throw new KeyNotFoundException("ClientId not found in Contract");

        return result;
    }

    public async Task SetTem(Contract contract)
    {
        //var pendingSchedules = await _monthlyScheduleDomain.HasPendingScheduleInMonthAndYear(contract.PaymentDay.Month, contract.PaymentDay.Year);
        if (DateTime.Now > contract.PaymentDay)
        {
            contract.Tem = _financialUtils.CalculateDefaultInterest(
                _financialUtils.CalculateTEM(contract.TypePenaltyRate, contract.PenaltyRate, contract.PenaltyPeriod),
                contract.PaymentDay
            );
            contract.PaymentDay = contract.PaymentDay.AddMonths(1);
            await Update(contract, contract.Id);
        }
        else
        {
            contract.Tem = _financialUtils.CalculateTEM(contract.TypeRate, contract.Rate, contract.Period);
        }
    }

    public async Task<List<Contract>> GetAll()
    {
        return await _contractData.GetAll(); 
    }

    public async Task<Contract> Create(Contract contract)
    {
        try
        {
            return await _contractData.Create(contract);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create contract", ex);
        }
    }

    public async Task<bool> Update(Contract contract, int id)
    {
        try
        {
            var existingContract = await _contractData.GetById(id);
            if (existingContract != null)
            {
                return await _contractData.Update(contract, id);
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
            var existingContract = await _contractData.GetById(id);
            if (existingContract != null)
            {
                return await _contractData.Delete(id);
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}