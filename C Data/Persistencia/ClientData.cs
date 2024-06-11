using Data.Context;
using Data.Model;
using Data.Persistencia.Impl;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistencia;

public class ClientData : IClientData
{
    private readonly AppDbContext _appDbContext;
    
    public ClientData(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Client> GetById(int id)
    {
        return await _appDbContext.Clients.Where(c => c.Id == id && c.IsActive == true).FirstOrDefaultAsync();    }

    public async Task<Client> GetByDni(string dni)
    {
        return await _appDbContext.Clients.Where(c => c.Dni.Contains(dni)
                                                      && c.IsActive == true).FirstOrDefaultAsync();
    }
    
    public async Task<List<Client>> GetAll()
    {
        return await _appDbContext.Clients.Where(c => c.IsActive == true).ToListAsync();
    }

    public async Task<Client> Create(Client client)
    {
        try
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == client.UserId && u.IsActive == true);
            if (user == null)
            {
                throw new Exception("El usuario referenciado no está activo o no existe.");
            }
            _appDbContext.Clients.Add(client);
            await _appDbContext.SaveChangesAsync();

            return client;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> Update(Client client, int id)
    {
        try
        {
            var clientUpdated = _appDbContext.Clients.Where(c => c.Id == id).FirstOrDefault();

            if (clientUpdated != null)
            {
                clientUpdated.FirstName = client.FirstName;
                clientUpdated.LastName = client.LastName;
                clientUpdated.Dni = client.Dni;
                clientUpdated.Phone = client.Phone;
                clientUpdated.Address = client.Address;
                clientUpdated.CreditLimit = client.CreditLimit;
                clientUpdated.BirthDate = client.BirthDate;
                clientUpdated.DateUpdated = DateTime.Now;

                _appDbContext.Clients.Update(clientUpdated);
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
            var clientDeleted = _appDbContext.Clients.Where(c => c.Id == id).FirstOrDefault();

            if (clientDeleted != null)
            {
                clientDeleted.IsActive = false;
                clientDeleted.DateUpdated = DateTime.Now;
                _appDbContext.Clients.Update(clientDeleted);
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