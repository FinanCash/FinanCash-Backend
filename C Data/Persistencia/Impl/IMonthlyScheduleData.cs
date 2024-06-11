using Data.Model;

namespace Data.Persistencia.Impl;

public interface IMonthlyScheduleData
{
    public Task<MonthlySchedule> GetById(int id);
    public Task<MonthlySchedule> GetByMonthYearClientAndSchedule(int month, int year, int clientId, int scheduleId);
    public Task<List<MonthlySchedule>> GetByMonthAndYear(int month, int year);
    public Task<List<MonthlySchedule>> GetAll();
    public Task<MonthlySchedule> Create(MonthlySchedule monthlySchedule);
    public Task<bool> Update(MonthlySchedule monthlySchedule, int id);
    public Task<bool> Delete(int id);
}