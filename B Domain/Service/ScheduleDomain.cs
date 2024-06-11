using Data.Context;
using Data.Model;
using Data.Persistencia.Impl;
using Domain.Service.Impl;

namespace Domain.Service;

public class ScheduleDomain : IScheduleDomain
{
    private readonly IScheduleData _scheduleData;
    private readonly FinancialUtils _financialUtils;
    private readonly IContractDomain _contractDomain;
    private readonly ILoanDomain _loanDomain;
    //private readonly IMonthlyScheduleDomain _monthlyScheduleDomain;
    private readonly AppDbContext _appDbContext;

    public ScheduleDomain(IScheduleData scheduleData, FinancialUtils financialUtils, AppDbContext appDbContext, IContractDomain contractDomain, ILoanDomain loanDomain)
    {
        _scheduleData = scheduleData;
        _financialUtils = financialUtils;
        _appDbContext = appDbContext;
        _contractDomain = contractDomain;
        _loanDomain = loanDomain;
        //_monthlyScheduleDomain = monthlyScheduleDomain;
    }
    public async Task<Schedule> GetById(int id)
    {
        var result = await _scheduleData.GetById(id);
        if (result == null)
            throw new KeyNotFoundException("Schedule not found");

        return result;
    }

    public async Task<List<IntermediateClass.LoanPayment>> CalculateAndCreateFrenchInstallments(int contractId, int loanId)
    {
        var contract = await _contractDomain.GetById(contractId);
        var loan = await _loanDomain.GetById(loanId);
        if (contract.Tem == 0)
        {
            await _contractDomain.SetTem(contract);
        }
        var installmentList = _financialUtils.CalculateFrenchInstallment(loan.Amount, contract.Tem, loan.TotalInstallments);
        if (installmentList == null || installmentList.Count == 0)
        {
            throw new Exception("La lista de cuotas está vacía o nula.");
        }
        List<Schedule> scheduleList = new List<Schedule>();
        
        bool monthIncreased = false;

        foreach (var installment in installmentList)
        {
            //if (installmentNumber > 1 && !monthIncreased)
            //{
            //    contract.PaymentDay = contract.PaymentDay.AddMonths(1);
            //    monthIncreased = true;
            //}
            
            //var monthlySchedule = await _monthlyScheduleDomain.GetOrCreateMonthlySchedule(contract.PaymentDay.Month, contract.PaymentDay.Year);


            scheduleList.Add(new Schedule
            {
                InstallmentNumber = installment.NCuotas,
                Amortization = installment.A,
                Interest = installment.I,
                InstallmentAmount = installment.R,
                DueDate = contract.PaymentDay, 
                LoanId = loan.Id,
                ClientId = contract.ClientId,
            });
        }

        await _appDbContext.Schedules.AddRangeAsync(scheduleList);
        await _appDbContext.SaveChangesAsync();
        // Agregar registros de depuración
        Console.WriteLine("ScheduleList Count: " + scheduleList.Count);
        foreach (var schedule in scheduleList)
        {
            Console.WriteLine("DueDate: " + schedule.DueDate);
        }
        return installmentList;
    }

    public async Task<List<Schedule>> GetAll()
    {
        return await _scheduleData.GetAll();
    }

    public async Task<bool> Update(Schedule schedule, int id)
    {
        try
        {
            var existingSchedule = await _scheduleData.GetById(id);
            if (existingSchedule != null)
            {
                return await _scheduleData.Update(schedule, id);
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
            var existingSchedule = await _scheduleData.GetById(id);
            if (existingSchedule != null)
            {
                return await _scheduleData.Delete(id);
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
