using Data.Context;
using Data.Model;
using Data.Persistencia.Impl;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistencia;

public class ScheduleData : IScheduleData
{
    private readonly AppDbContext _appDbContext;
    
    public ScheduleData(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Schedule> GetById(int id)
    {
        return await _appDbContext.Schedules.Where(s => s.Id == id && s.IsActive == true).FirstOrDefaultAsync();    }
    
    public async Task<List<Schedule>> GetAll()
    {
        return await _appDbContext.Schedules.Where(s => s.IsActive == true).ToListAsync();
    }

    public async Task<bool> Update(Schedule schedule, int id)
    {
        try
        {
            var scheduleUpdated = _appDbContext.Schedules.Where(s => s.Id == id).FirstOrDefault();

            if (scheduleUpdated != null)
            {
                scheduleUpdated.InstallmentNumber = schedule.InstallmentNumber;
                scheduleUpdated.Amortization= schedule.Amortization;
                scheduleUpdated.Interest = schedule.Interest;
                scheduleUpdated.InstallmentAmount = schedule.InstallmentAmount;
                scheduleUpdated.DueDate = schedule.DueDate;
                scheduleUpdated.DateUpdated = DateTime.Now;

                _appDbContext.Schedules.Update(scheduleUpdated);
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
            var scheduleDeleted = _appDbContext.Schedules.Where(s => s.Id == id).FirstOrDefault();

            if (scheduleDeleted != null)
            {
                scheduleDeleted.IsActive = false;
                scheduleDeleted.DateUpdated = DateTime.Now;
                _appDbContext.Schedules.Update(scheduleDeleted);
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

 