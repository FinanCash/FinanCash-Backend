using Data.Model;

namespace Domain.Service.Impl;

public interface IClientDomain
{
    public Task<Client> GetById(int id);
    public Task<Client> GetByDni(string dni);
    public Task<List<Client>> GetAll();
    public Task<Client> Create(Client client);
    public Task<bool> Update(Client client, int id);
    public Task<bool> Delete(int id);
}