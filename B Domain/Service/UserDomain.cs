using Data.Context;
using Data.Model;
using Data.Persistencia.Impl;
using Domain.Service.Impl;
using Microsoft.EntityFrameworkCore;

namespace Domain.Service;

public class UserDomain : IUserDomain
{
    private readonly IUserData _userData;
    private readonly AppDbContext _appDbContext;

    public UserDomain(IUserData userData, AppDbContext appDbContext)
    {
        _userData = userData;
        _appDbContext = appDbContext;
    }

    public async Task<User> GetById(int id)
    {
        var result = await _userData.GetById(id);
        if (result == null)
            throw new KeyNotFoundException("User not found");

        return result;
    }

    public async Task<User> GetByEmail(User user)
    {
        var result = await _userData.GetByEmail(user);
        if (result == null)
            throw new KeyNotFoundException("Email not found");

        return result;
    }

    public async Task<List<User>> GetAll()
    {
        return await _userData.GetAll();
    }

    public async Task<string> Login(User user)
    {
        var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

        if (existingUser != null)
        {
            return "Login exitoso";
        }
        
        return "Usuario o contraseña incorrectos";
    }

    public async Task<User> Register(User user)
    {
        try
        {
            var existingUser = await _userData.GetByEmail(user);
            if (existingUser != null)
                throw new Exception("Client already exists");
            return await _userData.Register(user);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> Update(User user, int id)
    {
        try
        {
            var existingUser = GetById(id);
            if (existingUser != null)
            {
                return await _userData.Update(user, id);
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
            var existingUser = await _userData.GetById(id);
            if (existingUser != null)
            {
                return await _userData.Delete(id);
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}