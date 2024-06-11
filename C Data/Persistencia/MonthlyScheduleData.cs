using Data.Context;
using Data.Model;
using Data.Persistencia.Impl;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistencia;

public class MonthlyScheduleData : IMonthlyScheduleData
{
    private readonly AppDbContext _appDbContext;
    
    public MonthlyScheduleData(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<MonthlySchedule> GetById(int id)
    {
        return await _appDbContext.MonthlySchedules.Where(s => s.Id == id && s.IsActive == true).FirstOrDefaultAsync();    
    }

    public async Task<MonthlySchedule> GetByMonthYearClientAndSchedule(int month, int year, int clientId, int scheduleId)
    {
        return await _appDbContext.MonthlySchedules
            .Where(s => s.Month == month && s.Year == year && s.ClientId == clientId && s.ScheduleId == scheduleId && s.IsActive == true)
            .FirstOrDefaultAsync();
    }

    public async Task<List<MonthlySchedule>> GetByMonthAndYear(int month, int year)
    {
        return await _appDbContext.MonthlySchedules
            .Where(s => s.Month == month && s.Year == year && s.IsActive == true)
            .ToListAsync();
    }

    public async Task<List<MonthlySchedule>> GetAll()
    {
        return await _appDbContext.MonthlySchedules.Where(s => s.IsActive == true).ToListAsync();
    }

    public async Task<MonthlySchedule> Create(MonthlySchedule monthlySchedule)
    {
        try
        {
            var client = await _appDbContext.Clients.FirstOrDefaultAsync(u => u.Id == monthlySchedule.ClientId && u.IsActive == true);
            if (client == null)
            {
                throw new Exception("El cliente referenciado no está activo o no existe.");
            }
            _appDbContext.MonthlySchedules.Add(monthlySchedule);
            await _appDbContext.SaveChangesAsync();

            return monthlySchedule;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> Update(MonthlySchedule monthlySchedule, int id)
    {
        try
        {
            var monthlyScheduleUpdated = _appDbContext.MonthlySchedules.Where(s => s.Id == id).FirstOrDefault();

            if (monthlyScheduleUpdated != null)
            {
                monthlyScheduleUpdated.Status = monthlySchedule.Status;
                monthlyScheduleUpdated.DateUpdated = DateTime.Now;

                _appDbContext.MonthlySchedules.Update(monthlyScheduleUpdated);
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
            var monthlyScheduleDeleted = _appDbContext.MonthlySchedules.Where(s => s.Id == id).FirstOrDefault();

            if (monthlyScheduleDeleted != null)
            {
                monthlyScheduleDeleted.IsActive = false;
                monthlyScheduleDeleted.DateUpdated = DateTime.Now;

                _appDbContext.MonthlySchedules.Update(monthlyScheduleDeleted);
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