using Data.Model;

namespace Data.Persistencia.Impl;

public interface IScheduleData
{
    public Task<Schedule> GetById(int id);
    public Task<List<Schedule>> GetAll();
    public Task<bool> Update(Schedule schedule, int id);
    public Task<bool> Delete(int id);
}