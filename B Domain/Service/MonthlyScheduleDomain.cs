using Data.Model;
using Data.Persistencia.Impl;
using Domain.Service.Impl;

namespace Domain.Service;

public class MonthlyScheduleDomain : IMonthlyScheduleDomain
{
    private readonly IMonthlyScheduleData _monthlyScheduleData;
    private readonly IClientDomain _clientDomain;
    private readonly IContractDomain _contractDomain;
    private readonly IScheduleDomain _scheduleDomain;
    private int _nextDebtId = 1;
    private List<IntermediateClass.DebtView> _debtViews;

    public MonthlyScheduleDomain(IMonthlyScheduleData monthlyScheduleData, IClientDomain clientDomain, IContractDomain contractDomain, IScheduleDomain scheduleDomain)
    {
        _monthlyScheduleData = monthlyScheduleData;
        _clientDomain = clientDomain;
        _contractDomain = contractDomain;
        _scheduleDomain = scheduleDomain;
        _debtViews = new List<IntermediateClass.DebtView>();
        _nextDebtId = 1;
    }
    
    public async Task<MonthlySchedule> GetById(int id)
    {
        var result = await _monthlyScheduleData.GetById(id);
        if (result == null)
            throw new KeyNotFoundException("MonthlySchedule not found");

        return result;
    }

    public async Task<List<MonthlySchedule>> GetByMonthAndYear(int month, int year)
    {
        try
        {
            var result = await _monthlyScheduleData.GetByMonthAndYear(month, year);
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to get monthlySchedule by month and year", ex);
        }
    }

    public async Task<MonthlySchedule> GetByMonthYearClientAndSchedule(int month, int year, int clientId, int scheduleId)
    {
        try
        {
            var result = await _monthlyScheduleData.GetByMonthYearClientAndSchedule(month, year, clientId, scheduleId);
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to get monthlySchedule by month and year", ex);
        }
    }

    public async Task<bool> HasPendingScheduleInMonthAndYear(int month, int year)
    {
        try
        {
            var result = await GetByMonthAndYear(month, year);
            if (result != null)
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
    public async Task<List<IntermediateClass.DebtView>> GetDebtView()
    {
        var schedules = await _scheduleDomain.GetAll();
        var contracts = await _contractDomain.GetAll();
        var clients = await _clientDomain.GetAll();

        foreach (var client in clients)
        {
            var clientContracts = contracts.Where(c => c.ClientId == client.Id).ToList();

            foreach (var contract in clientContracts)
            {
                bool hasPendingLoan = await HasPendingScheduleInMonthAndYear(contract.PaymentDay.Month, contract.PaymentDay.Year);

                if (hasPendingLoan)
                {
                    var clientSchedules = schedules.Where(s => s.ClientId == client.Id).ToList();
                    double totalAmount = clientSchedules.Sum(s => s.InstallmentAmount);
                    
                    var monthlySchedules = await _monthlyScheduleData.GetByMonthAndYear(contract.PaymentDay.Month, contract.PaymentDay.Year);
                    if (monthlySchedules == null || !monthlySchedules.Any())
                    {
                        // Maneja el caso en que monthlySchedules es null o vacío
                        continue;
                    }
                    foreach (var monthlySchedule in monthlySchedules)
                    {
                        _debtViews.Add(new IntermediateClass.DebtView
                        {
                            Id = _nextDebtId++,
                            ClientName = $"{client.FirstName} {client.LastName}",
                            ClientDNI = client.Dni,
                            DueDate = contract.PaymentDay,
                            TotalAmount = totalAmount,
                            ContractId = contract.Id,
                            ClientId = client.Id,
                            MonthlyScheduleId = monthlySchedule.Id
                        });
                    }
                }
            }
        }

        return _debtViews;
    }
    
    public async Task UpdateMonthlyScheduleStatus(int monthlyScheduleId)
    {
        var monthlySchedule = await _monthlyScheduleData.GetById(monthlyScheduleId);
        if (monthlySchedule != null)
        {
            monthlySchedule.Status = "Pagado";
            await _monthlyScheduleData.Update(monthlySchedule, monthlySchedule.Id);
        }
        else
        {
            throw new KeyNotFoundException($"MonthlySchedule with ID {monthlyScheduleId} not found");
        }
    }

    public async Task<List<MonthlySchedule>> GetAll()
    {
        return await _monthlyScheduleData.GetAll();
    }

    public async Task<MonthlySchedule> Create(MonthlySchedule monthlySchedule)
    {
        try
        {
            return await _monthlyScheduleData.Create(monthlySchedule);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create contract", ex);
        }
    }
    
    public async Task<MonthlySchedule> CreateMonthlySchedule(int month, int year, int clientId, int scheduleId)
    {
        var existingMonthlySchedule = await GetByMonthYearClientAndSchedule(month, year, clientId, scheduleId);

        if (existingMonthlySchedule != null)
        {
            return existingMonthlySchedule;
        }
        
        var newMonthlySchedule = new MonthlySchedule
        {
            Month = month,
            Year = year,
            Status = "Pendiente",
            ClientId = clientId,
            ScheduleId = scheduleId
        };

        return await _monthlyScheduleData.Create(newMonthlySchedule);
    }

    public async Task<bool> Update(MonthlySchedule monthlySchedule, int id)
    {
        try
        {
            var existingMonthlySchedule = await _monthlyScheduleData.GetById(id);
            if (existingMonthlySchedule != null)
            {
                return await _monthlyScheduleData.Update(monthlySchedule, id);
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
            var existingMonthlySchedule = await _monthlyScheduleData.GetById(id);
            if (existingMonthlySchedule!= null)
            {
                return await _monthlyScheduleData.Delete(id);
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}