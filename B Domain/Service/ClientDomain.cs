using Data.Model;
using Data.Persistencia.Impl;
using Domain.Service.Impl;

namespace Domain.Service;

public class ClientDomain : IClientDomain
{
    private readonly IClientData _clientData;

    public ClientDomain(IClientData clientData)
    {
        _clientData= clientData;
    }

    public async Task<Client> GetById(int id)
    {
        var result = await _clientData.GetById(id);
        if (result == null)
            throw new KeyNotFoundException("Client not found");

        return result;
    }

    public async Task<Client> GetByDni(string dni)
    {
        var result = await _clientData.GetByDni(dni);
        if (result == null)
            throw new KeyNotFoundException("Dni not found");

        return result;
    }
    

    public async Task<List<Client>> GetAll()
    {
        return await _clientData.GetAll();    
    }

    public async Task<Client> Create(Client client)
    {
        try
        {
            var existingClient = await _clientData.GetByDni(client.Dni);
            if (existingClient != null)
                throw new Exception("Client already exists");
            return await _clientData.Create(client);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> Update(Client client, int id)
    {
        try
        {
            var existingClient = await _clientData.GetById(id);
            if (existingClient != null)
            {
                return await _clientData.Update(client, id);
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
            var existingClient = await _clientData.GetById(id);
            if (existingClient != null)
            {
                return await _clientData.Delete(id);
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}