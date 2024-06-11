using Data.Model;

namespace Domain.Service.Impl;

public interface IMonthlyScheduleDomain
{
    public Task<MonthlySchedule> GetById(int id);
    public  Task<List<MonthlySchedule>> GetByMonthAndYear(int month, int year);
    public  Task<MonthlySchedule> GetByMonthYearClientAndSchedule(int month, int year, int clientId, int scheduleId);
    public Task<bool> HasPendingScheduleInMonthAndYear(int month, int year);
    public Task<List<IntermediateClass.DebtView>> GetDebtView();
    public Task UpdateMonthlyScheduleStatus(int monthlyScheduleId);
    public Task<List<MonthlySchedule>> GetAll();
    public Task<MonthlySchedule> Create(MonthlySchedule monthlySchedule);
    public Task<MonthlySchedule> CreateMonthlySchedule(int month, int year, int clientId, int scheduleId);
    public Task<bool> Update(MonthlySchedule monthlySchedule, int id);
    public Task<bool> Delete(int id);
}