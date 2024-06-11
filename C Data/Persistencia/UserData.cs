using Data.Context;
using Data.Model;
using Data.Persistencia.Impl;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistencia;

public class UserData : IUserData
{
    private readonly AppDbContext _appDbContext;
    
    public UserData(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<User> GetById(int id)
    {
        return await _appDbContext.Users.Where(u => u.Id == id && u.IsActive == true).FirstOrDefaultAsync();    }

    public async Task<User> GetByEmail(User user)
    {
        return await _appDbContext.Users.Where(u => u.Email.Contains(user.Email)
                                                         && u.IsActive == true).FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetAll()
    {
        return await _appDbContext.Users.Where(u => u.IsActive == true).ToListAsync();
    }

    public async Task<User> Register(User user)
    {
        try
        {
            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();

            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }    }

    public async Task<bool> Update(User user, int id)
    {
        try
        {
            var userUpdated = _appDbContext.Users.Where(u => u.Id == id).FirstOrDefault();
            if (userUpdated != null)
            {
                userUpdated.Email = user.Email;
                userUpdated.Password = user.Password;
                userUpdated.DateUpdated = DateTime.Now;
                
                _appDbContext.Users.Update(userUpdated);
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
            var userDeleted = _appDbContext.Users.Where(ir => ir.Id == id).FirstOrDefault();

            if (userDeleted != null)
            {
                userDeleted.IsActive = false;
                userDeleted.DateUpdated = DateTime.Now;
                _appDbContext.Users.Update(userDeleted);
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

 